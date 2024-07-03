using FluentValidation;
using MediatR;
using TMS.Infrastructure.Configuration;
using TMS.Infrastructure.Entities;
using TMS.Infrastructure.Repository;

namespace TMS.APP.Commands.Table.Update.Handler
{
    public class UpdateTableCommandHandler : IRequestHandler<UpdateTableCommand, bool>
    {
        private readonly ITableRepository _repository;
        private readonly IValidator<UpdateTableCommand> _validator;

        public UpdateTableCommandHandler(ITableRepository repository, IValidator<UpdateTableCommand> validator)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<bool> Handle(UpdateTableCommand request, CancellationToken cancellationToken)
        {
            //var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            //if (!validationResult.IsValid)
            //{
            //    throw new ValidationException(validationResult.Errors);
            //}
            //var filter = new Filter();
            //filter.Id = request.TableId;
            //var existingTable = await _repository.GetTableByIdAsync(filter);

            //if (existingTable == null)
            //{
            //    throw new Exception($"Table with ID {request.TableId} not found.");
            //}

            //existingTable.TableName = request.TableName;

            //existingTable.Columns.Clear();

            //foreach (var columnDef in request.Columns)
            //{
            //    var columnInfo = new ColumnInfo
            //    {
            //        ColumnName = columnDef.Name,
            //        DataType = columnDef.DataType,
            //        IsNullable = columnDef.IsNullable
            //    };

            //    existingTable.Columns.Add(columnInfo);
            //}
            //var table = new DynamicTable
            //{
            //    TableName = existingTable.TableName,
            //    Id = existingTable.Id,
            //    Columns = existingTable.Columns,
            //    LastChangeDate = DateTime.Now,
            //    IsDeleted = existingTable.IsDeleted,
            //};
            //await _repository.UpdateTableAsync(table);

            return true;
        }
    }
}
