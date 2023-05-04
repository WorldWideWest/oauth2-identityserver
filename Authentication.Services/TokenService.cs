using Authentication.Models.Configuration;
using Authentication.Models.Constants.Identity;
using Authentication.Models.DTOs.Requests;
using Authentication.Models.DTOs.Responses;
using Authentication.Models.Interfaces.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Authentication.Services
{
    public class TokenService : ITokenService
    {
        private readonly ILogger<TokenService> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly Application _configuration;


        public TokenService(ILogger<TokenService> logger, IHttpClientFactory clientFactory, IOptions<Application> application)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _configuration = application.Value;
        }


        public async Task<CustomToken> TokenAsync(Token request)
        {
            try
            {
                var result = await AcquireTokenAsync(request);
                
                if(result.IsError)
                {
                    IdentityError error = new()
                    {
                        Code = result.HttpStatusCode.ToString(),
                        Description = result.Error
                    };

                    return CustomToken.Failed(error);
                }
                    
                return new() {
                    AccessToken = result.AccessToken,
                    RefreshToken = result.RefreshToken,
                    ExpiresIn = result.ExpiresIn
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, nameof(TokenAsync));
                throw ex;
            }
        }

        private async Task<TokenResponse> AcquireTokenAsync(Token request)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var disco = await GetDiscoveryCacheAsync();

                switch (request.GrantType)
                {
                    case GrantType.Password:
                        return await client.RequestPasswordTokenAsync(new()
                        {
                            Address = disco.TokenEndpoint,
                            ClientId = request.ClientId,
                            ClientSecret = request.ClientSecret,
                            Scope = request.Scope,
                            UserName = request.Username,
                            Password = request.Password,
                        }).ConfigureAwait(false);

                    case GrantType.ClientCredentials:
                        return await client.RequestClientCredentialsTokenAsync(new()
                        {
                            Address = disco.TokenEndpoint,
                            ClientId = request.ClientId,
                            ClientSecret = request.ClientSecret,
                            Scope = request.Scope
                        }).ConfigureAwait(false);

                    case GrantType.RefreshToken:
                       return await client.RequestRefreshTokenAsync(new()
                       {
                           Address = disco.TokenEndpoint,
                           ClientId = request.ClientId,
                           ClientSecret = request.ClientSecret,
                           RefreshToken = request.RefreshToken
                       }).ConfigureAwait(false);

                    default:
                        throw new Exception("Unsuported Grant Type");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, nameof(AcquireTokenAsync));
                throw ex;
            }
        }

        private async Task<DiscoveryDocumentResponse> GetDiscoveryCacheAsync()
        {
            try
            {
                var cache = new DiscoveryCache(_configuration.AuthUri);

                var result = await cache.GetAsync()
                    .ConfigureAwait(false);

                if (!result.IsError)
                    return result;

                throw result.Exception;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, nameof(GetDiscoveryCacheAsync));
                throw ex;
            }
        }

    }
}