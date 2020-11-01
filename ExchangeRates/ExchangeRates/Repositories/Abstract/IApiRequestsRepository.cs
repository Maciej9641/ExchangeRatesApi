using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRates.Repositories.Abstract
{
    public interface IApiRequestsRepository
    {
        Task SaveApiRequestToDatabase(HttpRequestMessage requestMessage);
    }
}
