using MediatR;
using TMS.APP.Models;
using TMS.Infrastructure.Repository;

namespace TMS.APP.Queries.Handlers
{
    public class GetAllTablesQueryHandler : IRequestHandler<GetAllTablesQuery, List<DynamicTableDto>>
    {
        private readonly ITableRepository _repository;

        public GetAllTablesQueryHandler(ITableRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<DynamicTableDto>> Handle(GetAllTablesQuery request, CancellationToken cancellationToken)
        {
            var tables = await _repository.GetAllTablesAsync();
            var results = new List<DynamicTableDto>();

            //foreach (var item in tables)
            //{
            //    var dto = new DynamicTableDto
            //    {
            //        Id = item.Id,
            //        TableName = item.TableName,
            //        Columns = item.Columns
            //    };

            //    results.Add(dto);
            //}

            return results;
        }
    }
}
