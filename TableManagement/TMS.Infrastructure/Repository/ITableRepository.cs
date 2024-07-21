using TMS.Infrastructure.Configuration;
using TMS.Infrastructure.Entities;

namespace TMS.Infrastructure.Repository
{
    public interface ITableRepository
    {
        Task<List<string>> GetAllTablesAsync();
        Task<DynamicTableDto> GetTableDataWithColumnsAsync(string tableName, int pageNumber, int pageSize);
        Task InsertData(string tableName, List<string> columnNames, List<object> values);
        Task CreateTable(CreateTableRequest request, int numberOfRows);
    }
}
