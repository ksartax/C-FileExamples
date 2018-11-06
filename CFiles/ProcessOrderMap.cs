using CsvHelper.Configuration;

namespace CFiles
{
    public class ProcessOrderMap : ClassMap<ProcessOrder>
    {
        public ProcessOrderMap()
        {
            AutoMap();

            Map(m => m.Amount).Name("Quantity").TypeConverter<RomanTypeConverter>();
            Map(m => m.Customer).Name("CustomerNumber");
        }
    }
}
