namespace GraphQlCosmosDbStarter.Api.Responses
{
    public class BaseResponse
    {
        /// <summary>
        /// Indicates if there an error
        /// </summary>
        public bool HasErrors { get; set; }

        /// <summary>
        /// Contains a collection of all messages
        /// </summary>
        public List<string> Messages { get; set; }
    }
}
