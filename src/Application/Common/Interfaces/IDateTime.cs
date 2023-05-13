namespace ShedulingReminders.Application.Common.Interfaces;

/// <summary>
/// Represents an interface for accessing the current date and time.
/// </summary>
public interface IDateTime
{
    /// <summary>
    /// Gets the current date and time.
    /// </summary>
    DateTime Now { get; }
}