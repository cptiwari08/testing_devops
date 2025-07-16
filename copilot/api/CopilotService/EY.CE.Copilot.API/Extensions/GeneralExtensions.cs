using Polly.Contrib.WaitAndRetry;
using Polly;
using System.Text.Json;

namespace EY.CE.Copilot.API.Extensions
{
    public static class GeneralExtensions
    {
        #region HttpResponseMessage

        internal static T ToResponseModel<T>(this HttpResponseMessage httpResponseMessage) where T : class
        {
            return JsonSerializer.Deserialize<T>(httpResponseMessage.Content.ReadAsStringAsync().Result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        #endregion

        #region IHttpClientBuilder

        public static IHttpClientBuilder AddWaitAndRetryAsync(this IHttpClientBuilder builder, int retryCount = 3, TimeSpan? initialDelay = null)
        {
            initialDelay ??= TimeSpan.FromSeconds(1);

            return builder.AddTransientHttpErrorPolicy(policyBuilder =>
                policyBuilder.WaitAndRetryAsync(
                    Backoff.DecorrelatedJitterBackoffV2(initialDelay.Value, retryCount)));
        }

        #endregion
    }
}
