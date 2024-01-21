namespace Booking.Auth.Application.Common;

//TODO: вынести в общий репозиторий
public static class Roles
{
    public const string Client = "Client";
    public const string Admin = "Admin";
    public const string SuperAdmin = "SuperAdmin";

    public static Dictionary<string, Guid> GetAllRolesWithIds()
    {
        return new Dictionary<string, Guid>
        {
            {Client, Guid.Parse("29c3471d-5cb2-4cab-b815-3ed6718268a0")},
            {Admin, Guid.Parse("60e26b53-f985-4cd8-ad5f-3f75a56368ce")},
            {SuperAdmin, Guid.Parse("6cefb619-7056-4004-9ef8-c96a988774da")}
        };
    }
}