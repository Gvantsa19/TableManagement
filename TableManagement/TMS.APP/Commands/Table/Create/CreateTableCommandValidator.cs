using FluentValidation;
using TMS.APP.Models;

namespace TMS.APP.Commands.Table.Create
{
    public class CreateTableCommandValidator : AbstractValidator<CreateTableCommand>
    {
        public CreateTableCommandValidator()
        {
            RuleFor(x => x.table.TableName).NotEmpty().MaximumLength(100);
        }
    }

    public class ColumnDefinitionValidator : AbstractValidator<ColumnDefinition>
    {
        public ColumnDefinitionValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.DataType).NotEmpty().MaximumLength(50);
        }
    }
}
