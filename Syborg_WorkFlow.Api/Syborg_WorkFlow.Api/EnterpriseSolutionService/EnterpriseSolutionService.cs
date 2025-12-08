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

        public async Task<List<ApplicationListDTO>> GetApplicationListAsync()
        {
            var url = "https://localhost:7013/api/Application/GetApplicationLists";

            var response = await _httpClient.GetStringAsync(url);

            var Apps = JsonSerializer.Deserialize<List<ApplicationListDTO>>(response,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return Apps;
        }

        public async Task<List<ModuleListDTO>> GetModuleListAsync()
        {
            var url = "https://localhost:7013/api/Module/GetModuleList";

            var response = await _httpClient.GetStringAsync(url);

            var module = JsonSerializer.Deserialize<List<ModuleListDTO>>(response,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return module;
        }

        public async Task<List<ModuleListDTO>?> GetModuleByApplicationIdAsync(Guid applicationId)
        {
            var url = $"https://localhost:7013/api/Module/GetModuleByApplicationId/{applicationId}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                // Server gave 404 → return null instead of throwing exception
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<ModuleListDTO>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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

        public async Task<List<ApplicationPage>?> GetApplicationPageByModuleIdAsync(Guid moduleId)
        {
            var url = $"https://localhost:7013/api/ApplicationPage/GetApplicationPageByModuleId/{moduleId}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                // Server gave 404 → return null instead of throwing exception
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<ApplicationPage>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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


        public async Task<List<PageSectionList>?> GetSectionByApplicationPageIdAsync(Guid applicationPageId)
        {
            var url = $"https://localhost:7013/api/Section/GetSectionByApplicationPageId/{applicationPageId}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                // Server gave 404 → return null instead of throwing exception
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<PageSectionList>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<List<RoleList>> GetRoleListAsync()
        {
            var url = "https://localhost:7013/api/Role/GetRoleList";

            var response = await _httpClient.GetStringAsync(url);

            var role = JsonSerializer.Deserialize<List<RoleList>>(response,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return role;
        }
    }

}




