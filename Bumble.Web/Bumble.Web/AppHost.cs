using Funq;
using ServiceStack;
using ServiceStack.Admin;
using ServiceStack.Api.Swagger;
using ServiceStack.Data;
using ServiceStack.Logging;
using ServiceStack.OrmLite;
using ServiceStack.Text;

namespace Bumble.Web
{
    //VS.NET Template Info: https://servicestack.net/vs-templates/EmptyAspNet
    public class AppHost : AppHostBase
    {
        /// <summary>
        /// Base constructor requires a Name and Assembly where web service implementation is located
        /// </summary>
        public AppHost()
            : base("Bumble.Web", typeof(ContactService).Assembly) { }

        /// <summary>
        /// Application specific configuration
        /// This method should initialize any IoC resources utilized by your web service classes.
        /// </summary>
        public override void Configure(Container container)
        {
            LogManager.LogFactory = new ConsoleLogFactory(debugEnabled:true); 
            
            JsConfig.DateHandler = DateHandler.ISO8601;
            JsConfig.EmitCamelCaseNames = true;
            JsConfig.ConvertObjectTypesIntoStringDictionary = true;
            
            container.Register<IDbConnectionFactory>(c => 
                new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider));

            using (var sess = container.Resolve<IDbConnectionFactory>().Open())
            {
                sess.CreateTable<Contact>();
                sess.CreateTable<ContactTag>();
                sess.CreateTable<ContactBucket>();
                sess.CreateTable<ContactBucketAssignement>();
            }
            
            Plugins.Add(new SwaggerFeature());
            Plugins.Add(new AutoQueryFeature());
            Plugins.Add(new AdminFeature());
            //Config examples
            //this.Plugins.Add(new PostmanFeature());
            //this.Plugins.Add(new CorsFeature());
        }
    }
}