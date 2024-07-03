using MediatR;
using TMS.APP.Models;
using TMS.Infrastructure.Repository;

namespace TMS.APP.Commands.Table.Create
{
    public class CreateTableCommand : IRequest<bool>
    {
        public CreateTableRequest table { get; set; }
    }
}
