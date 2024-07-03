namespace TMS.APP.Models
{
    public class ColumnDefinition
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public bool IsNullable { get; set; }
        public int? Length { get; set; }
        public int? Precision { get; set; }
        public int? Scale { get; set; }
        public bool IsPrimaryKey { get; set; }
        public string DefaultValue { get; set; }
    }
}
