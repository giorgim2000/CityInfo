using CityInfo.Api.Services;
using Microsoft.AspNetCore.SignalR;

namespace CityInfo.Api.Hubs
{
    public class TestHub : Hub
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        public TestHub(ICityInfoRepository cityInfoRepository)
        {
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
        }

        public async Task GetUpdateForCity()
        {
            
        }
    }
}
