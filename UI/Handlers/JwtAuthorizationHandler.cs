using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net.Http.Headers;

namespace UI.Handlers
{
    public class JwtAuthorizationHandler : DelegatingHandler
    {
        private readonly ProtectedLocalStorage _localStorage;
        public JwtAuthorizationHandler(ProtectedLocalStorage localStorage)
        {
            _localStorage = localStorage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var result = await _localStorage.GetAsync<string>("access_token");
            if (result.Success && !string.IsNullOrEmpty(result.Value))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.Value);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}