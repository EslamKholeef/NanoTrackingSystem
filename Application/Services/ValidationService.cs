using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ValidationService : IValidationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ValidationService> _logger;

        public ValidationService(HttpClient httpClient, ILogger<ValidationService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<ValidationServiceResult> ValidateStepAsync(
            string validationEndpoint,
            int processId,
            string stepName,
            string action,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Starting validation for Process {ProcessId}, Step {StepName}", processId, stepName);

                if (validationEndpoint.Contains("simulate"))
                {
                    return await SimulateValidation(stepName, action);
                }

                var validationRequest = new
                {
                    ProcessId = processId,
                    StepName = stepName,
                    Action = action,
                    Timestamp = DateTime.UtcNow
                };

                var json = JsonSerializer.Serialize(validationRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(validationEndpoint, content, cancellationToken);
                var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var validationResponse = JsonSerializer.Deserialize<ExternalValidationResponse>(responseContent);

                    _logger.LogInformation("Validation successful for Process {ProcessId}", processId);

                    return new ValidationServiceResult
                    {
                        IsValid = validationResponse?.IsValid ?? false,
                        Response = responseContent,
                        ErrorMessage = validationResponse?.ErrorMessage
                    };
                }
                else
                {
                    _logger.LogWarning("Validation API returned error for Process {ProcessId}: {StatusCode}", processId, response.StatusCode);

                    return new ValidationServiceResult
                    {
                        IsValid = false,
                        Response = responseContent,
                        ErrorMessage = $"Validation API error: {response.StatusCode}"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Validation failed for Process {ProcessId}", processId);

                return new ValidationServiceResult
                {
                    IsValid = false,
                    ErrorMessage = $"Validation service error: {ex.Message}"
                };
            }
        }

        private async Task<ValidationServiceResult> SimulateValidation(string stepName, string action)
        {
            // Simulate API delay
            await Task.Delay(500);

            return stepName.ToLower() switch
            {
                "finance approval" when action == "approve" => new ValidationServiceResult
                {
                    IsValid = DateTime.UtcNow.Second % 2 == 0, // 50% success rate ....... just for demo
                    Response = "{ \"budget_check\": \"passed\", \"amount_valid\": true }",
                    ErrorMessage = DateTime.UtcNow.Second % 2 != 0 ? "Budget exceeded" : null
                },

                "manager approval" when action == "approve" => new ValidationServiceResult
                {
                    IsValid = true,
                    Response = "{ \"manager_authority\": \"verified\" }"
                },

                _ => new ValidationServiceResult
                {
                    IsValid = true,
                    Response = "{ \"validation\": \"passed\" }"
                }
            };
        }

        private class ExternalValidationResponse
        {
            public bool IsValid { get; set; }
            public string? ErrorMessage { get; set; }
        }
    }
}
