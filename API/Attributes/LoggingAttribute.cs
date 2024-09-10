
namespace API.Attributes
{
    public class LoggingAttribute :Attribute
    {
        public LoggingType loggingType;

        public LoggingAttribute(LoggingType loggingType)
        {
            this.loggingType = loggingType;
        }
    }
    public enum LoggingType
    {
        None = 0,             // No logging
        Full = 1,             // Log full request and response details
        General = 2,      // Log the general info (API call information)
        ExceptBody = 3,     // Log only the response body
    }
}
