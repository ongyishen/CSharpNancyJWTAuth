using Nancy;
using Nancy.Security;

namespace CSharpNancyJWTAuth
{
    public class MainModule : NancyModule
    {
        public MainModule()
        {

            Get("/", _ =>
            {
                return "Hello Nancy JwtBearer Authentication Demo";
            });


            Get("/test", _ =>
            {
                this.RequiresAuthentication();
                return "From JwtBearer Authentication";
            });
        }
    }
}
