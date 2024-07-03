using TMS.Infrastructure.Entities.Abstract;

namespace TMS.Infrastructure.Entities
{
    public class ColumnInfo : BaseDeletableModel<int>
    {
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public bool IsNullable { get; set; }
        public int TableId { get; set; }
        public DynamicTable Table { get; set; }

        public int? Length { get; set; }       
        public int? Precision { get; set; }  
        public int? Scale { get; set; } 
        public bool IsPrimaryKey { get; set; } 
        public string DefaultValue { get; set; }
    }
}
