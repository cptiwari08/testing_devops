using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using EY.CapitalEdge.ChatOrchestrator.Utils.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace EY.CapitalEdge.ChatOrchestrator.Utils
{
    public class Common : ICommon
    {
        private static readonly JsonSerializerOptions _jsonSerializerOptionsDeserialize = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        private static readonly JsonSerializerOptions _jsonSerializerOptionsSerializeToCamelCase = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        /// <summary>
        /// Deserialize a JSON string to an object.
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="json">json</param>
        /// <returns>T object</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="JsonException"></exception>
        public T Deserialize<T>(string? json)
        {
            T? result;
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentNullException(nameof(json), "The JSON string is empty or null.");

            result = JsonSerializer.Deserialize<T>(json, _jsonSerializerOptionsDeserialize);

            if (result is null)
                throw new JsonException("The deserialization from JSON to the specified type has failed.");

            return result;
        }

        /// <summary>
        /// Serialize an object to a JSON string.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string SerializeToCamelCase(object obj)
        {
            return JsonSerializer.Serialize(obj, _jsonSerializerOptionsSerializeToCamelCase);
        }

        /// <summary>
        /// Validate the JWT token using the expected audience, issuer, and public key.
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="token">token</param>
        /// <param name="issuer">issuer</param>
        /// <param name="publicKey">publicKey</param>
        /// <returns>True or False depending if token it is valid</returns>
        public bool ValidateToken(ILogger logger, string token, string issuer, string publicKey)
        {
            token = token.Replace("Bearer ", string.Empty);

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
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

                tokenHandler.ValidateToken(token, validationParameters, out _);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Token validation failed.");
                return false;
            }
        }

        /// <summary>
        /// Get token data from the JWT token.
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="token">token</param>
        /// <returns>Token data</returns>
        public Token GetTokenData(ILogger logger, string token)
        {
            Token tokenData = new Token();
            token = token.Replace("Bearer ", string.Empty);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(token);

                var claimActions = new Dictionary<string, Action<string>>
                {
                    { "unique_name", value => tokenData.UniqueName = value },
                    { "email", value => tokenData.Email = value },
                    { "family_name", value => tokenData.FamilyName = value },
                    { "given_name", value => tokenData.GivenName = value },
                    { "user_type", value => tokenData.UserType = value },
                    { "oid", value => tokenData.Oid = value },
                    { "ce_oid", value => tokenData.CeOid = value },
                    { "upn", value => tokenData.Upn = value },
                    { "sp_url", value => tokenData.SpUrl = value },
                    { "po_app_url", value => tokenData.PoAppUrl = value },
                    { "po_api_url", value => tokenData.PoApiUrl = value },
                    { "copilot_app_url", value => tokenData.CopilotAppUrl = value },
                    { "copilot_api_url", value => tokenData.CopilotApiUrl = value },
                    { "project_id", value => tokenData.ProjectId = value },
                    { "project_friendly_id", value => tokenData.ProjectFriendlyId = value },
                    { "scope", value => tokenData.Scope.Add(value) },
                    { "nbf", value => tokenData.Nbf = long.Parse(value) },
                    { "exp", value => tokenData.Exp = long.Parse(value) },
                    { "iat", value => tokenData.Iat = long.Parse(value) },
                    { "iss", value => tokenData.Iss = value },
                    { "aud", value => tokenData.Aud = value },
                    { "ai_search_instance_name", value => tokenData.AiSearchInstanceName = value },
                    { "metadata_index_name", value => tokenData.MetadataIndexName = value },
                };

                foreach (var claim in jwtToken.Claims)
                {
                    if (claimActions.TryGetValue(claim.Type, out var action))
                    {
                        action(claim.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting token data.");
                throw new CommonException("Error getting token data.", ex);
            }

            return tokenData;
        }

        /// <summary>
        /// Get environment variable value or throw an exception if it is not set.
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="variableName">variableName</param>
        /// <returns>Environment value</returns>
        /// <exception cref="ArgumentNullException"></exception>
        [ExcludeFromCodeCoverage]
        public virtual string GetEnvironmentVariable(ILogger logger, string variableName)
        {
            string? variableValue = Environment.GetEnvironmentVariable(variableName);

            if (string.IsNullOrEmpty(variableValue))
            {
                logger.LogError("The environment variable {VariableName} does not have a value.", variableName);
                throw new ArgumentNullException($"The environment variable '{variableName}' does not have a value.");
            }

            return variableValue;
        }

        /// <summary>
        /// Is there any suggested question in the context.
        /// </summary>
        /// <param name="context">context</param>
        /// <returns>true/false depending if suggestion exist in context</returns>
        public bool IsThereAnySuggestedQuestion(Dictionary<string, object?>? context)
        {
            if (context == null)
                return false;

            if (!context.TryGetValue("suggestion", out object? value))
                return false;

            string contextString = value?.ToString() ?? "";

            if (string.IsNullOrWhiteSpace(contextString))
                return false;

            var suggestion = Deserialize<Suggestion>(contextString);

            if (string.IsNullOrWhiteSpace(suggestion.Source))
                return false;

            return true;
        }

        /// <summary>
        /// Get suggested question from the context.
        /// </summary>
        /// <param name="context">context</param>
        /// <returns>Suggestion</returns>
        public Suggestion GetSuggestedQuestion(Dictionary<string, object?>? context)
        {
            Suggestion suggestion = new Suggestion();

            if (context == null)
                return suggestion;

            if (!context.TryGetValue("suggestion", out object? value))
                return suggestion;

            string contextString = value?.ToString() ?? "";

            return Deserialize<Suggestion>(contextString);
        }

        /// <summary>
        /// Is the URL valid.
        /// </summary>
        /// <param name="urlString">urlString</param>
        /// <returns>True/False</returns>
        public bool IsValidUrl(string urlString)
        {
            return Uri.TryCreate(urlString, UriKind.Absolute, out Uri? uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        /// <summary>
        /// Is there any app info in the context
        /// </summary>
        /// <param name="context">Question context</param>
        /// <returns>True/False</returns>
        public bool IsThereAnyAppInfo(Dictionary<string, object?>? context)
        {
            if (context == null)
                return false;

            if (!context.TryGetValue("appInfo", out object? value))
                return false;

            string contextString = value?.ToString() ?? "";

            if (string.IsNullOrWhiteSpace(contextString))
                return false;

            var appInfo = Deserialize<AppInfo>(contextString);

            if (string.IsNullOrWhiteSpace(appInfo.Key) || appInfo.TeamTypeIds is null || appInfo.TeamTypeIds.Length == 0)
                return false;

            return true;
        }

        /// <summary>
        /// Get app info from the context
        /// </summary>
        /// <param name="context">context</param>
        /// <returns>Suggestion</returns>
        public AppInfo GetAppInfo(Dictionary<string, object?>? context)
        {
            AppInfo appInfo = new AppInfo();

            if (context == null)
                return appInfo;

            if (!context.TryGetValue("appInfo", out object? value))
                return appInfo;

            string contextString = value?.ToString() ?? "";

            return Deserialize<AppInfo>(contextString);
        }

        /// <summary>
        /// Is there any active PO apps in the context
        /// </summary>
        /// <param name="context">Question context</param>
        /// <returns>True/False</returns>
        public bool IsThereAnyActivePOApps(Dictionary<string, object?>? context)
        {
            if (context == null)
                return false;

            if (!context.TryGetValue("activePOApps", out object? value))
                return false;

            string contextString = value?.ToString() ?? "";

            if (string.IsNullOrWhiteSpace(contextString))
                return false;

            var activePOApps = Deserialize<List<ActivePOApps>>(contextString);

            if (activePOApps is null)
                return false;

            return activePOApps.Exists(w => !string.IsNullOrWhiteSpace(w.Key));
        }

        /// <summary>
        /// Get app info from the context
        /// </summary>
        /// <param name="context">context</param>
        /// <returns>Suggestion</returns>
        public List<ActivePOApps> GetActivePOApps(Dictionary<string, object?>? context)
        {
            List<ActivePOApps> activePOApps = new List<ActivePOApps>();

            if (context == null)
                return activePOApps;

            if (!context.TryGetValue("activePOApps", out object? value))
                return activePOApps;

            string contextString = value?.ToString() ?? "";

            activePOApps = Deserialize<List<ActivePOApps>>(contextString);

            return activePOApps.Where(w => !string.IsNullOrWhiteSpace(w.Key)).ToList();
        }
    }
}