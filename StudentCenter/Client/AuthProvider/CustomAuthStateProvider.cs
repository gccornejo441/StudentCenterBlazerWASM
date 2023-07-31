using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using StudentCenter.Client.Features;
using System.Net.Http.Headers;
using System.Security.Claims;
using static DevExpress.Data.Helpers.FindSearchRichParser;

/// This file provides information about the user’s authentication state 
/// – whether they are authenticated or not, what are their claims, 
namespace StudentCenter.Client.AuthProvider
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationState _anonymous;

        public CustomAuthStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // This line fetches the authentication token from local storage, assuming it's stored with the key "authToken
            var token = await _localStorage.GetItemAsync<string>("authToken");

            // This line checks if the token is empty or contains only whitespace, indicating that the user is not authenticated. In this case, it returns an anonymous authentication state _anonymous
            if (string.IsNullOrEmpty(token))
                return _anonymous;

            // This line sets the Authorization header of the _httpClient to include the retrieved token, indicating that the user is authenticated for future API calls.
            // Authorization is a specific header used in HTTP requests to provide credentials for authentication. 
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            // This line creates and returns a new AuthenticationState object with a ClaimsPrincipal containing the claims parsed from the JWT token.
            // The "jwtAuthType" string is used as the authentication type for the ClaimsIdentity.
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType")));

        }

        // Notifies the application that a user has been authenticated.
        // The 'email' parameter represents the user's email address as the identifier.
        public void NotifyUserAuthentication(string token)
        {
            // Create a new ClaimsPrincipal representing the authenticated user.
            // A ClaimsPrincipal is an object that encapsulates the user's identity and any associated claims.
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType"));

            // Create a new AuthenticationState with the authenticated user.
            // An AuthenticationState represents the current authentication state of the application.
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));

            // Notify the application that the authentication state has changed.
            // The NotifyAuthenticationStateChanged method is used to trigger the authentication state change event.
            // This will allow components in the application to respond to the user being authenticated.
            NotifyAuthenticationStateChanged(authState);
        }

        // Notifies the application that the user has logged out.
        public void NotifyUserLogout()
        {
            // Create a new AuthenticationState with an anonymous user.
            // An anonymous user is represented by '_anonymous' which holds an empty ClaimsPrincipal.
            var authState = Task.FromResult(_anonymous);

            // Notify the application that the authentication state has changed.
            // The NotifyAuthenticationStateChanged method is used to trigger the authentication state change event.
            // This will allow components in the application to respond to the user being logged out.
            NotifyAuthenticationStateChanged(authState);
        }

    }
}
