using Galilei.Models;
using Galilei.Models.Dashboards;

namespace Galilei.Services
{
    public interface IPriceAlertService
    {
        Task CheckAndSendAsync(int userId, IReadOnlyCollection<UserAsset> assets, IReadOnlyCollection<AssetData> marketData);
    }
}
