namespace ShedulingReminders.Application.Common.Interfaces;

/// <summary>
/// Represents an interface for accessing the current user's information.
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Gets the user ID of the current user.
    /// </summary>
    string? UserName { get; }
}
