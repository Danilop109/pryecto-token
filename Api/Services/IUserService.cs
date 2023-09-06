using Api.Dtos;
namespace Api.Services;
    public interface IUserService
    {
        Task<string> RegisterAsync(RegisterDto model);
        Task<DatoUsuarioDto> GetTokenAsync(LoginDto model);
        Task<string> AddRoleAsync(AddRoleDto model);

    }
