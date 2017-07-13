using System;
using Funq;
using ServiceStack;
using ServiceStack.Admin;
using ServiceStack.Api.Swagger;
using ServiceStack.Configuration;
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
            LogManager.LogFactory = new ConsoleLogFactory(); 
            
            JsConfig.DateHandler = DateHandler.ISO8601;
            JsConfig.EmitCamelCaseNames = true;
            JsConfig.ConvertObjectTypesIntoStringDictionary = true;

            var euId = Guid.NewGuid();
            
            RegisterTypedRequestFilter<IBelonger>((req, res, dtoInterface) =>
            {
                if (dtoInterface.TenantKey != null && dtoInterface.TenantKey.ToLower() == "eu")
                    dtoInterface.TenantId = euId;
            });
            
//            OrmLiteConfig.SqlExpressionSelectFilter = q =>
//            {
// BUG this breaks autoquery
//                if (q.ModelDef.ModelType.HasInterface(typeof(ISoftDelete)))
//                    q.Where<ISoftDelete>(x => x.IsDeleted != true);
//            };
            
            // app settings through ormlite

            var appSettings = new MultiAppSettings(
                new EnvironmentVariableSettings(),
                new AppSettings());
            
            var connStr = appSettings.GetString("MYSQL_CONNECTION_STRING"); 
            
            container.Register<IDbConnectionFactory>(c => 
                new OrmLiteConnectionFactory(connStr, MySqlDialect.Provider));

            container.Register<IMessagePublisher>(c => new DummyBus());

            
            Plugins.Add(new SwaggerFeature());
//            Plugins.Add(new AutoQueryFeature());
//            Plugins.Add(new AdminFeature());
            //Config examples
            //this.Plugins.Add(new PostmanFeature());
            //this.Plugins.Add(new CorsFeature());
        }
    }
}