using APICore.Common.DTO.Request;
using APICore.Data.Entities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace APICore.Services
{
    public interface IAccountService
    {
        Task SignUpAsync(SignUpRequest suRequest);

        Task<(User user, string accessToken, string refreshToken)> LoginAsync(LoginRequest loginRequest);

        Task LogoutAsync(string accessToken, ClaimsIdentity claimsIdentity);

        Task<ClaimsPrincipal> GetPrincipalFromExpiredTokenAsync(string token);

        Task GetRefreshTokenAsync(RefreshTokenRequest refreshToken, string userId);

        Task<(string accessToken, string refreshToken)> GenerateNewTokensAsync(string token, string refreshToken);

        Task ChangePasswordAsync(ChangePasswordRequest changePassword, int userId);

        Task GlobalLogoutAsync(ClaimsIdentity claimsIdentity);

        Task<User> UpdateProfileAsync(UpdateProfileRequest updateProfile, int userId);

        Task<bool> ValidateTokenAsync(string token);

        Task<User> GetUserAsync(int userId);

        Task ChangeAccountStatusAsync(ChangeAccountStatusRequest changeAccountStatus, int userId);

        Task<User> UploadAvatar(IFormFile file, int currentUser);

        Task<string> ForgotPasswordAsync(string email);
    }
}