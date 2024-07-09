namespace Agent.Common
{
    public class DateUtilities
    {
        public static int UtcTimestamp()
        {
            return (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}