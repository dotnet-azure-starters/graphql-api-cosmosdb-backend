namespace GraphQlCosmosDbStarter.Api.GraphQl.Directives
{
    public class TitleCaseDirectiveType : DirectiveType
    {
        protected override void Configure(IDirectiveTypeDescriptor descriptor) =>
            descriptor
                .Name("title")
                .Location(DirectiveLocation.Field)
                .Use(next => async context =>
                {
                    await next.Invoke(context).ConfigureAwait(false);

                    if (context.Result is string value)
                    {
                        context.Result = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(value);
                    }
                });
    }
}
