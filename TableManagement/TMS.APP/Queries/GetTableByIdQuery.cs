using MediatR;
using TMS.APP.Models;
using TMS.Infrastructure.Configuration;
using TMS.Infrastructure.Entities;
using TMS.Infrastructure.Repository;

namespace TMS.APP.Queries
{
    public class GetTableByIdQuery : IRequest<DynamicTableDto>
    {
        public Filter Filter { get; set; }
    }
}
