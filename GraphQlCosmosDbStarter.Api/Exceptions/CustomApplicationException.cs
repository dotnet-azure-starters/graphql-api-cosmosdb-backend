namespace GraphQlCosmosDbStarter.Api.Exceptions
{
    public class CustomApplicationException : BaseException
    {
        public CustomApplicationException(List<string> messages) : base(messages)
        {
        }

        public CustomApplicationException(string message) : base(new List<string> { message })
        {
        }
    }
}
