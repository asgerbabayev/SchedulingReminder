namespace ShedulingReminders.Application.Common.Extensions;

public static class StringExtension
{
    /// <summary>
    /// Validates if the given Telegram ID is in a valid format.
    /// </summary>
    public static bool IsValidTelegramId(this string telegramId)
    {
        string pattern = @"^\d+$";
        return Regex.IsMatch(telegramId, pattern);
    }

    /// <summary>
    /// Validates if the given email address is in a valid format.
    /// </summary>
    public static bool IsValidEmail(this string email)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, pattern);
    }
    /// <summary>
    /// Validates if the given method is a valid reminder method.
    /// </summary>
    public static bool IsValidMethod(this string method)
    {
        foreach (var item in Enum.GetValues(typeof(Methods)))
        {
            if (method.Trim().ToLower().Equals(item.ToString().Trim().ToLower()))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Extension method to get the normalized value of a string by removing leading and trailing whitespace and converting it to lowercase.
    /// </summary>
    /// <param name="value">The string value to normalize.</param>
    /// <returns>The normalized string value.</returns>
    public static string GetNormalizedValue(this string value)
    {
        return value.Trim().ToLower();
    }
}
