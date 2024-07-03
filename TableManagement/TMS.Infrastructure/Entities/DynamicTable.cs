using System.Text.Json.Serialization;
using TMS.Infrastructure.Entities.Abstract;

namespace TMS.Infrastructure.Entities
{
    public class DynamicTable : BaseDeletableModel<int>
    {
        public string TableName { get; set; }
        [JsonIgnore]
        public List<ColumnInfo> Columns { get; set; }

        public DynamicTable()
        {
            Columns = new List<ColumnInfo>();
        }
    }
}
