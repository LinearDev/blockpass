using System;
namespace Blockpasstest.Custom
{
	public class Formatter
	{
		public Formatter()
		{
		}

		public string TimeFormatter(string unix)
		{
            long.TryParse(unix, out long uinxLong);
            DateTimeOffset dateTime = DateTimeOffset.FromUnixTimeSeconds(uinxLong);
            DateTimeOffset currentTime = DateTimeOffset.Now.ToOffset(DateTimeOffset.Now.Offset).Date;
            dateTime = dateTime.ToOffset(currentTime.Offset);

            string result;

            if (dateTime.Date == currentTime)
            {
                result = "Today";
            }
            else if (dateTime.Date == currentTime.AddDays(-1))
            {
                result = "Yesterday";
            }
            else if (dateTime.Year == currentTime.Year && dateTime.Month == currentTime.Month)
            {
                result = dateTime.ToString("dd");
            }
            else if (dateTime.Year == currentTime.Year && dateTime.Month != currentTime.Month)
            {
                result = dateTime.ToString("dd MMM");
            }
            else
            {
                result = dateTime.ToString("dd MMM yyyy");
            }

            return result;
		}
	}
}

