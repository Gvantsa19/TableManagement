using FluentValidation;
using TMS.APP.Commands.Table.Create;

namespace TMS.APP.Commands.Table.Update
{
    public class UpdateTableCommandValidator : AbstractValidator<UpdateTableCommand>
    {
        public UpdateTableCommandValidator()
        {
            RuleFor(x => x.TableId).NotEmpty().GreaterThan(0);
            RuleFor(x => x.TableName).NotEmpty().MaximumLength(100);
            RuleForEach(x => x.Columns).SetValidator(new ColumnDefinitionValidator());
        }
    }
}
