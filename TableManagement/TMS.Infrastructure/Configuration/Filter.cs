namespace TMS.Infrastructure.Configuration
{
    public class Filter
    {
        public int Id { get; set; }
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public string? ColumnNameFilter { get; set; }
    }
}
