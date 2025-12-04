using Syborg_WorkFlow.Api.EnterpriseSolutionModel;
using Syborg_WorkFlow.Api.ServiceModel;
using System.Text.Json;


namespace Syborg_WorkFlow.Api.Service
{
    public class EnterpriseSolutionService
    {
        private readonly HttpClient _httpClient;

        public EnterpriseSolutionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ApplicationPage>> GetApplicationPageListAsync()
        {
            var url = "https://localhost:7013/api/ApplicationPage/GetApplicationPageList";

            var response = await _httpClient.GetStringAsync(url);

            var pages = JsonSerializer.Deserialize<List<ApplicationPage>>(response,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return pages;
        }

        public async Task<List<PageSectionList>> GetPageSectionListAsync()
        {
            var url = "https://localhost:7013/api/Section/PageSectionList";

            var response = await _httpClient.GetStringAsync(url);

            var pages = JsonSerializer.Deserialize<List<PageSectionList>>(response,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return pages;
        }

        public async Task<List<RoleList>> GetRoleListAsync()
        {
            var url = "https://localhost:7013/api/Role/GetRoleList";

            var response = await _httpClient.GetStringAsync(url);

            var pages = JsonSerializer.Deserialize<List<RoleList>>(response,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return pages;
        }
    }

}




