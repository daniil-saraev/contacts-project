using static Desktop.Authentication.Models.User;

namespace Desktop.Authentication.Services;

public interface IUserDataStorage
{
    Task<UserData?> LoadData();

    Task SaveData(UserData userData);

    Task RemoveData();
}
