using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorF.Servics
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _jsRuntime;
        private readonly NavigationManager _navigationManager;

        public AuthService(HttpClient http, IJSRuntime jsRuntime, NavigationManager navigationManager)
        {
            _http = http;
            _jsRuntime = jsRuntime;
            _navigationManager = navigationManager;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/User/authentication",
                    new { Email = email, Password = password });

                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadAsStringAsync();
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", token);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task LogoutAsync()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
            _navigationManager.NavigateTo("/");
        }
    }
}
