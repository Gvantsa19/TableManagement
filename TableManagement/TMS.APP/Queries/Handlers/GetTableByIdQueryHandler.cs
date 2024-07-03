using MediatR;
using TMS.Infrastructure.Entities;
using TMS.Infrastructure.Repository;

namespace TMS.APP.Queries.Handlers
{
    public class GetTableByIdQueryHandler : IRequestHandler<GetTableByIdQuery, DynamicTableDto>
    {
        private readonly ITableRepository _repository;

        public GetTableByIdQueryHandler(ITableRepository repository)
        {
            _repository = repository;
        }

        public async Task<DynamicTableDto> Handle(GetTableByIdQuery request, CancellationToken cancellationToken)
        {
           throw new NotImplementedException();
        }
    }
}
