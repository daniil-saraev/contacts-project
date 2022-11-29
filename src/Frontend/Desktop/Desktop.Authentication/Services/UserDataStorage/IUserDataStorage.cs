using static Desktop.Authentication.Models.User;
using Desktop.Common.Exceptions;

namespace Desktop.Authentication.Services;

/// <summary>
/// Service for managing user data on disk.
/// </summary>
public interface IUserDataStorage
{
    /// <summary>
    /// Tries to load <see cref="UserData"/> from disk.
    /// </summary>
    /// <returns><see cref="UserData"/> if found, otherwise null.</returns>
    /// <exception cref="ReadingDataException"></exception>
    Task<UserData?> LoadData();

    /// <summary>
    /// Tries to save <see cref="UserData"/> to disk.
    /// </summary>
    /// <exception cref="WritingDataException"></exception>
    Task SaveData(UserData userData);

    /// <summary>
    /// Removes existing <see cref="UserData"/> from disk.
    /// </summary>
    /// <exception cref="DataNotFoundException"></exception>
    Task RemoveData();
}
