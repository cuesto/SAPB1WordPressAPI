using Newtonsoft.Json.Converters;

namespace SAPB1WordPressAPI.DataModel.Services
{
    public class DateFormatConverter : IsoDateTimeConverter
    {
        public DateFormatConverter(string format)
        {
            DateTimeFormat = format;
        }
    }
}
