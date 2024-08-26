using System.Threading.Tasks;

namespace JdMarketplace.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}