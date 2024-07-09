namespace Agent.Common
{
    public class IdGenerator
    {
        public static string Generate()
        {
            return Guid.NewGuid().ToString();
        }
    }
}