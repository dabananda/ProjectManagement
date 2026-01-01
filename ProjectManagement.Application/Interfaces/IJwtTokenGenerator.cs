using ProjectManagement.Domain.Entities;

namespace ProjectManagement.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser user, IList<string> roles);
    }
}
