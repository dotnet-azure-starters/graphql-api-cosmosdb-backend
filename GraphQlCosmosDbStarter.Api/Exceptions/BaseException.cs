namespace GraphQlCosmosDbStarter.Api.Exceptions
{
    public class BaseException : Exception
    {
        public readonly List<string> Messages;

        public BaseException(List<string> messages) => Messages = messages;
    }
}
