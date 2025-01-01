using AcopioAPIs.DTOs.User;

namespace AcopioAPIs.Repositories
{
    public interface IUser
    {
        Task<List<UserResultDto>> GetAll(string? name, string? userName, bool? estado);
        Task<UserDto> GetById(int id);
        Task<UserResultDto> Insert(UserInsertDto insertDto);
        Task<UserResultDto> Update(UserUpdateDto updateDto);
        Task<bool> Delete(UserDeleteDto deleteDto);
    }
}
