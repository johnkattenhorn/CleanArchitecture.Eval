using System.Net.Http.Json;
using Api.FunctionalTests.Contracts;
using SharedKernel;

namespace Api.FunctionalTests.Extensions;

internal static class HttpResponseMessageExtensions
{
    internal static async Task<CustomProblemDetails> GetProblemDetails(
        this HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException("Successful response");
        }

        CustomProblemDetails problemDetails = await response
            .Content
            .ReadFromJsonAsync<CustomProblemDetails>();

        Ensure.NotNull(problemDetails);

        return problemDetails;
    }
}
