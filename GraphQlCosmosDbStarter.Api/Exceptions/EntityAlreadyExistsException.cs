namespace GraphQlCosmosDbStarter.Api.Exceptions
{
    public class EntityAlreadyExistsException : BaseException
    {
        public EntityAlreadyExistsException(List<string> messages) : base(messages)
        {
        }

        public EntityAlreadyExistsException(string message) : base(new List<string> { message })
        {
        }
    }
}
