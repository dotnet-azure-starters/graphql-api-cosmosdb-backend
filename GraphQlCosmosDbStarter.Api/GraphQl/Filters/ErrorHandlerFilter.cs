using GraphQlCosmosDbStarter.Api.Exceptions;

namespace GraphQlCosmosDbStarter.Api.GraphQl.Filters
{
    public class ErrorHandlerFilter : IErrorFilter
    {
        public IError OnError(IError error)
        {
            if (error.Exception is null)
                return error;

            var errorMessages = new List<string>();
            if (error.Exception is BaseException baseException)
                errorMessages = baseException.Messages;

            var errorDetails = error.Exception switch
            {
                EntityAlreadyExistsException entityAlreadyExistsException => ErrorDetails.GenerateEntityAlreadyExistsError(errorMessages, error),
                EntityNotFoundException entityNotFoundException => ErrorDetails.Generate404Error(errorMessages, error),
                _ => ErrorDetails.Generate500Error()
            };

            return errorDetails;
        }
    }

    public class ErrorDetails
    {
        public static IError Generate500Error() => ErrorBuilder.New().SetMessage("An unexpected error has occurred, please contact support.").Build();

        public static IError Generate404Error(List<string> messages, IError error) => GenerateErrors(messages, "The resource was not found.", error);

        public static IError GenerateEntityAlreadyExistsError(List<string> messages, IError error) => GenerateErrors(messages, "That item with the same name/id already exists.", error);

        private static IError GenerateErrors(List<string> messages, string defaultMessage, IError error) => messages.Any() ? error.WithMessage(defaultMessage) : error.WithMessage(string.Join(". ", messages));
    }
}
