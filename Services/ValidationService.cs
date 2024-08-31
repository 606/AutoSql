using AutoSql.Interfaces;
using System.Collections.Generic;

namespace AutoSql.Services
{
    public class ValidationService
    {
        private readonly List<IValidator> _validators;

        public ValidationService()
        {
            _validators = new List<IValidator>();
        }

        public void AddValidator(IValidator validator)
        {
            _validators.Add(validator);
        }

        public bool ValidateAll(string input, string repoPath, out List<string> errorMessages)
        {
            errorMessages = new List<string>();
            bool isValid = true;

            foreach (var validator in _validators)
            {
                if (!validator.Validate(input, repoPath, out string errorMessage))
                {
                    isValid = false;
                    errorMessages.Add(errorMessage);

                    // if we have error - break the process
                    break;
                }
            }

            return isValid;
        }
    }
}