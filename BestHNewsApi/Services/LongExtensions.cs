namespace BestHNewsApi.Services
{
    public static class LongExtensions
    {
        public static DateTime ToDateTime(this long unixTime)
        {
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime convertedDateTime = unixEpoch.AddSeconds(unixTime).ToLocalTime();

            return convertedDateTime;
        }
    }
}
