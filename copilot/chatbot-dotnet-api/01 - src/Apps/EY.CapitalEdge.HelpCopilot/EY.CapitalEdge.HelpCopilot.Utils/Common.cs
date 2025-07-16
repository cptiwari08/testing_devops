using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;
using EY.CapitalEdge.HelpCopilot.Utils.Models;

namespace EY.CapitalEdge.HelpCopilot.Utils
{
    public class Common : ICommon
    {
        private readonly ILogger<Common> _logger;
        private readonly IConfiguration _configuration;

        public Common(ILogger<Common> logger, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Validate token
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="input">input</param>
        /// <returns>Bolean</returns>
        public bool ValidateToken(string token, BackendInput input)
        {
            string issuer = _configuration["JwtCe:Issuer"] ?? "";
            string publicKey = _configuration["JwtCe:PublicKey"]?
                .Replace("-----BEGIN RSA PUBLIC KEY----- ", "")
                .Replace(" -----END RSA PUBLIC KEY-----", "") ?? "";

            return ValidateToken(token, issuer, publicKey, input);
        }

        /// <summary>
        /// Validate the JWT token using the expected audience, issuer, and public key.
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="issuer">issuer</param>
        /// <param name="publicKey">publicKey</param>
        /// <param name="input">input</param>
        /// <returns>True or False depending if token it is valid</returns>
        [ExcludeFromCodeCoverage]
        private bool ValidateToken(string token, string issuer, string publicKey, BackendInput input)
        {
            token = token.Replace("Bearer ", string.Empty);

            var tokenHandler = new JwtSecurityTokenHandler();

            // Convert the public key from a base64 string to an RSAParameters object
            RSAParameters publicKeyParameters;
            using (var rsa = RSA.Create())
            {
                rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);
                publicKeyParameters = rsa.ExportParameters(false);
            }

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = false, // I do not validate this because it is not project specific and audience it is project specific 
                ValidIssuer = issuer,
                IssuerSigningKey = new RsaSecurityKey(publicKeyParameters)
            };

            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out _);
                return true;
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogError(ex, "[instanceId:{InstanceId}] [chatId:{ChatId}] [projectFriendlyId:{ProjectFriendlyId}] Token validation failed.", input.InstanceId, input.ChatId, input.ProjectFriendlyId);
                return false;
            }
        }

        /// <summary>
        /// Extract the value from the context
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="key">key</param>
        /// <returns>Value</returns>
        public string? ExtractValueFromContext(IDictionary<string, object> context, string key)
        {
            return context.TryGetValue(key, out object? value) ? value?.ToString() : null;
        }
    }
}
