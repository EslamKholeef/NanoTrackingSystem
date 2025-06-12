using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IValidationService
    {
        Task<ValidationServiceResult> ValidateStepAsync(
            string validationEndpoint,
            int processId,
            string stepName,
            string action,
            CancellationToken cancellationToken);
    }

    public class ValidationServiceResult
    {
        public bool IsValid { get; set; }
        public string? Response { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
