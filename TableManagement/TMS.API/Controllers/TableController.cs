using MediatR;
using Microsoft.AspNetCore.Mvc;
using TMS.Infrastructure.Repository;

namespace TMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class TableController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITableRepository _tableRepository;

        public TableController(IMediator mediator, ITableRepository tableRepository)
        {
            _mediator = mediator;
            _tableRepository = tableRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<string>>> GetAllTables()
        {
           return Ok(await _tableRepository.GetAllTablesAsync());
        }
        [HttpGet("getByName")]
        public async Task<ActionResult<DynamicTableDto>> GetTableDataWithColumnsAsync(string tableName, int pageNumber, int pageSize)
        {
            return Ok(await _tableRepository.GetTableDataWithColumnsAsync(tableName, pageNumber, pageSize));
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTablesss(CreateTableRequest request)
        {
            await _tableRepository.CreateTable(request);
            return Ok();
        }
        [HttpPost("insert")]
        public async Task<IActionResult> InsertData(string tableName, [FromBody] InsertRequestModel request)
        {
            try
            {
                await _tableRepository.InsertData(tableName, request.ColumnNames, request.Values);
                return Ok("Data inserted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
    public class InsertRequestModel
    {
        public List<string> ColumnNames { get; set; }
        public List<object> Values { get; set; }
    }
}
