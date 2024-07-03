using FluentValidation;
using MediatR;
using TMS.Infrastructure.Entities;
using TMS.Infrastructure.Repository;

namespace TMS.APP.Commands.Table.Create.Handler
{
    public class CreateTableCommandHandler : IRequestHandler<CreateTableCommand, bool>
    {
        private readonly ITableRepository _repository;
        private readonly IValidator<CreateTableCommand> _validator;

        public CreateTableCommandHandler(ITableRepository repository, IValidator<CreateTableCommand> validator)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<bool> Handle(CreateTableCommand request, CancellationToken cancellationToken)
        {
            await _repository.CreateTable(request.table);
            return true;
        }
    }
}
