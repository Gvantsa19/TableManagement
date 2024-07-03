using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Linq;
using System.Text;
using TMS.Infrastructure.Configuration;
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
            List<string> tables = new List<string>();

            var connectionString = _configuration.GetConnectionString("Connection");

            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();

                // Query to get all table names
                var query = @"
            SELECT table_name
            FROM information_schema.tables
            WHERE table_schema = 'public' -- Adjust schema as needed
            AND table_type = 'BASE TABLE';";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            tables.Add(reader.GetString(0));
                        }
                    }
                }
            }

            return tables;
        }
        public async Task<DynamicTableDto> GetTableDataWithColumnsAsync(string tableName, int pageNumber, int pageSize)
        {
            var connectionString = _configuration.GetConnectionString("Connection");

            var table = new DynamicTableDto();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();

                // Fetch the column names
                var columnNames = new List<string>();
                var columnQuery = $"SELECT * FROM \"{tableName}\" LIMIT 1;";
                using (var columnCommand = new NpgsqlCommand(columnQuery, connection))
                {
                    using (var reader = await columnCommand.ExecuteReaderAsync())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            columnNames.Add(reader.GetName(i));
                        }
                    }
                }

                table = new DynamicTableDto
                {
                    TableName = tableName,
                    Columns = columnNames.Select(cn => new ColumnInfo { ColumnName = cn }).ToList(),
                    Items = new List<Dictionary<string, object>>(),
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                // Calculate the offset for pagination
                int offset = (pageNumber - 1) * pageSize;

                // Query to get the paginated data
                var itemQuery = $"SELECT * FROM \"{tableName}\" LIMIT @PageSize OFFSET @Offset;";
                using (var itemCommand = new NpgsqlCommand(itemQuery, connection))
                {
                    itemCommand.Parameters.AddWithValue("@PageSize", pageSize);
                    itemCommand.Parameters.AddWithValue("@Offset", offset);

                    using (var reader = await itemCommand.ExecuteReaderAsync())
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

                            table.Items.Add(rowData);
                        }
                    }
                }
            }

            return table;
        }
        public async Task CreateTable(CreateTableRequest request)
        {
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
                        command.Parameters.AddWithValue($"@param{i}", values[i]);
                    }

                    await command.ExecuteNonQueryAsync();
                }
            }
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
