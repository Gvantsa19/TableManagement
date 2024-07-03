using MediatR;
using TMS.APP.Models;

namespace TMS.APP.Commands.Table.Update
{
    public class UpdateTableCommand : IRequest<bool>
    {
        public int TableId { get; set; }
        public string TableName { get; set; }
        public List<ColumnDefinition> Columns { get; set; }
    }
}
