namespace GraphQlCosmosDbStarter.Api.Exceptions
{
    public class EntityNotFoundException : BaseException
    {
        public EntityNotFoundException(List<string> messages) : base(messages)
        {
        }

        public EntityNotFoundException(string message) : base(new List<string> { message })
        {
        }
    }
}
