using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Text;
using TMS.Infrastructure.Entities;
using TMS.Infrastructure.Persistence;

namespace TMS.Infrastructure.Repository
{
    public class TableRepository : ITableRepository
    {
        private readonly TMSDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public TableRepository(TMSDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<List<string>> GetAllTablesAsync()
        {
            var query = @"
                SELECT table_name
                FROM information_schema.tables
                WHERE table_schema = 'public'
                AND table_type = 'BASE TABLE';";

            var tables = new List<string>();

            using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;

                await _dbContext.Database.OpenConnectionAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        tables.Add(reader.GetString(0));
                    }
                }
            }

            return tables;
        }
        public async Task<DynamicTableDto> GetTableDataWithColumnsAsync(string tableName, int pageNumber, int pageSize)
        {
            var table = new DynamicTableDto();

            var columnNames = await GetColumnNames(tableName);

            table.TableName = tableName;
            table.Columns = columnNames.Select(cn => new ColumnInfo { ColumnName = cn }).ToList();
            table.Items = await GetTableItems(tableName, pageNumber, pageSize);

            table.PageNumber = pageNumber;
            table.PageSize = pageSize;

            return table;
        }
        public async Task CreateTable(CreateTableRequest request, int numberOfRows)
        {
            if (!IsValidTableName(request.TableName))
            {
                throw new ArgumentException("Invalid table name.");
            }
            var connectionString = _configuration.GetConnectionString("Connection");
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                var tableName = request.TableName;
                var columns = request.Columns;

                List<string> primaryKeyColumns = new List<string>();

                StringBuilder createTableQuery = new StringBuilder();
                createTableQuery.AppendFormat("CREATE TABLE IF NOT EXISTS \"{0}\" (", tableName);

                foreach (var column in columns)
                {
                    createTableQuery.AppendFormat("\"{0}\" {1}", column.ColumnName, column.DataType);

                    if (!column.IsNullable)
                    {
                        createTableQuery.Append(" NOT NULL");
                    }

                    if (!string.IsNullOrEmpty(column.DefaultValue))
                    {
                        createTableQuery.AppendFormat(" DEFAULT '{0}'", column.DefaultValue);
                    }

                    createTableQuery.Append(", ");

                    if (column.IsPrimaryKey)
                    {
                        primaryKeyColumns.Add(column.ColumnName);
                    }
                }
                createTableQuery.Length -= 2; 
                createTableQuery.Append(");");

                if (primaryKeyColumns.Any())
                {
                    createTableQuery.AppendFormat("ALTER TABLE \"{0}\" ADD PRIMARY KEY ({1});",
                        tableName,
                        string.Join(", ", primaryKeyColumns));
                }

                using (var command = new NpgsqlCommand(createTableQuery.ToString(), connection))
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Table created successfully.");
                }

                numberOfRows = 50;

                for (int i = 0; i < numberOfRows; i++)
                {
                    var insertQuery = new StringBuilder();
                    insertQuery.AppendFormat("INSERT INTO \"{0}\" (", tableName);
                    insertQuery.Append(string.Join(", ", columns.Select(c => $"\"{c.ColumnName}\"")));
                    insertQuery.Append(") VALUES (");
                    insertQuery.Append(string.Join(", ", columns.Select(c => FormatValue(GenerateRandomData(c.DataType)))));
                    insertQuery.Append(");");

                    using (var insertCommand = new NpgsqlCommand(insertQuery.ToString(), connection))
                    {
                        insertCommand.ExecuteNonQuery();
                    }
                }

                Console.WriteLine($"{numberOfRows} rows inserted successfully.");
            }
        }
        public async Task InsertData(string tableName, List<string> columnNames, List<object> values)
        {
            if (columnNames.Count != values.Count)
            {
                throw new ArgumentException("Number of column names must match number of values.");
            }

            var connectionString = _configuration.GetConnectionString("Connection");
            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;

                    StringBuilder sqlBuilder = new StringBuilder();
                    sqlBuilder.Append($"INSERT INTO {tableName} (");

                    for (int i = 0; i < columnNames.Count; i++)
                    {
                        sqlBuilder.Append($"{columnNames[i]}");
                        if (i < columnNames.Count - 1)
                        {
                            sqlBuilder.Append(", ");
                        }
                    }

                    sqlBuilder.Append(") VALUES (");

                    for (int i = 0; i < values.Count; i++)
                    {
                        sqlBuilder.Append($"@param{i}");
                        if (i < values.Count - 1)
                        {
                            sqlBuilder.Append(", ");
                        }
                    }

                    sqlBuilder.Append(")");

                    command.CommandText = sqlBuilder.ToString();

                    for (int i = 0; i < values.Count; i++)
                    {
                        // Check if value is null
                        object paramValue = values[i];
                        if (paramValue == null)
                        {
                            command.Parameters.AddWithValue($"@param{i}", DBNull.Value);
                        }
                        else
                        {
                            NpgsqlParameter parameter = new NpgsqlParameter($"@param{i}", paramValue);
      
                            command.Parameters.Add(parameter);
                        }
                    }

                    await command.ExecuteNonQueryAsync();
                }
            }
        }


        private async Task<List<string>> GetColumnNames(string tableName)
        {
            var query = $"SELECT * FROM \"{tableName}\" LIMIT 1;";

            var columnNames = new List<string>();

            using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                await _dbContext.Database.OpenConnectionAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        columnNames.Add(reader.GetName(i));
                    }
                }
            }

            return columnNames;
        }
        private async Task<List<Dictionary<string, object>>> GetTableItems(string tableName, int pageNumber, int pageSize)
        {
            int offset = (pageNumber - 1) * pageSize;

            var query = $"SELECT * FROM \"{tableName}\" LIMIT {pageSize} OFFSET {offset};";

            var tableItems = new List<Dictionary<string, object>>();

            using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                await _dbContext.Database.OpenConnectionAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var rowData = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var columnName = reader.GetName(i);
                            var columnValue = reader.GetValue(i);

                            rowData.Add(columnName, columnValue);
                        }

                        tableItems.Add(rowData);
                    }
                }
            }

            return tableItems;
        }

        private string FormatValue(object value)
        {
            if (value is string)
            {
                return $"'{value}'";
            }
            if (value is bool)
            {
                return (bool)value ? "TRUE" : "FALSE";
            }
            if (value is DateTime)
            {
                return $"'{((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss")}'";
            }
            return value.ToString();
        }
        private object GenerateRandomData(string dataType)
        {
            var random = new Random();
            switch (dataType.ToLower())
            {
                case "integer":
                case "int":
                    return random.Next(1, 1000);
                case "bigint":
                    return random.NextInt64(1, long.MaxValue);
                case "real":
                case "float":
                    return (float)random.NextDouble();
                case "double precision":
                case "double":
                    return random.NextDouble();
                case "boolean":
                    return random.Next(0, 2) == 0 ? false : true;
                case "text":
                case "varchar":
                case "character varying":
                    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                    return new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
                case "uuid":
                    return Guid.NewGuid();
                case "date":
                    return DateTime.Now.AddDays(random.Next(-365, 365)).ToString("yyyy-MM-dd");
                case "timestamp":
                case "timestamp without time zone":
                    return DateTime.Now.AddMinutes(random.Next(-525600, 525600)).ToString("yyyy-MM-dd HH:mm:ss");
                default:
                    throw new ArgumentException($"Unsupported data type: {dataType}");
            }
        }
        private bool IsValidTableName(string tableName)
        {
            var regex = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z0-9_]+$");
            return regex.IsMatch(tableName);
        }

    }


    public class DynamicTableDto
    {
        public string TableName { get; set; }
        public List<ColumnInfo> Columns { get; set; }
        public new List<Dictionary<string, object>> Items { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
    public class CreateTableRequest
    {
        public string TableName { get; set; }
        public List<ColumnDefinition> Columns { get; set; }
    }
    public class ColumnDefinition
    {
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public bool IsNullable { get; set; }
        public string? DefaultValue { get; set; }
        public bool IsPrimaryKey { get; set; }
    }

}
