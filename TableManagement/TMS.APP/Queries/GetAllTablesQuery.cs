using MediatR;
using TMS.APP.Models;
using TMS.Infrastructure.Repository;

namespace TMS.APP.Queries
{
    public class GetAllTablesQuery : IRequest<List<DynamicTableDto>>
    {
    }
}
