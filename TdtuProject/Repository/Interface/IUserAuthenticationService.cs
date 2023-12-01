using TdtuProject.Models.DTO;

namespace TdtuProject.Repository.Interface
{
    public interface IUserAuthenticationService
    {
        Task<Status> LoginAsync(LoginModel model);

        Task<Status> ResitrationAsync(RegistrationModel model);

        Task LogoutAsync();

        Task<Status> ChangePasswordAsync(ChangePasswordModel model, string username);

    }
}
