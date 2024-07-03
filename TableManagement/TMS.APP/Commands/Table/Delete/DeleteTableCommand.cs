using MediatR;

namespace TMS.APP.Commands.Table.Delete
{
    public class DeleteTableCommand : IRequest<bool>
    {
        public int TableId { get; set; }

        public DeleteTableCommand(int tableId)
        {
            TableId = tableId;
        }
    }
}
