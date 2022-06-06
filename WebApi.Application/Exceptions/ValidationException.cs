using System.Runtime.Serialization;
using FluentValidation.Results;

namespace WebApi.Application.Exceptions
{
    [Serializable]
    public class ValidationException : ApplicationException
    {
        protected ValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public List<string> Errors { get; set; } = new List<string>();

        public ValidationException(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Errors.Add(error.ErrorMessage);
            }
        }
    }
}
