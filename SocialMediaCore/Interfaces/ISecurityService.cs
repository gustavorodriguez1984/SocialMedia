using SocialMediaCore.Entities;
using System.Threading.Tasks;



namespace SocialMediaCore.Interfaces
{
    public interface ISecurityService
    {
        Task<Security> GetLoginByCredentials(UserLogin userLogin);
        Task RegisterUser(Security security);

    }
}
