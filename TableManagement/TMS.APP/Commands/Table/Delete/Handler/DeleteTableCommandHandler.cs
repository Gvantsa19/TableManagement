using MediatR;
using TMS.Infrastructure.Repository;

namespace TMS.APP.Commands.Table.Delete.Handler
{
    public class DeleteTableCommandHandler : IRequestHandler<DeleteTableCommand, bool>
    {
        private readonly ITableRepository _repository;

        public DeleteTableCommandHandler(ITableRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<bool> Handle(DeleteTableCommand request, CancellationToken cancellationToken)
        {
            //await _repository.DeleteTableAsync(request.TableId);
            return true;
        }
    }
}
