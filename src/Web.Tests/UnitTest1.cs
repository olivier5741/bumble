using System;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.Messaging;
using ServiceStack.OrmLite;
using ServiceStack.Testing;

namespace Bumble.Web.Tests
{
    [TestFixture]
    public class ContactServiceTests
    {
        private readonly ServiceStackHost appHost;

        public ContactServiceTests()
        {
            appHost = new BasicAppHost(typeof(ContactService).Assembly)
            {
                ConfigureContainer = container =>
                {
                    OrmLiteConfig.SqlExpressionSelectFilter = q =>
                    {
                        if (q.ModelDef.ModelType.HasInterface(typeof(ISoftDelete)))
                            q.Where<ISoftDelete>(x => x.IsDeleted != true);
                    };
            
                    container.Register<IDbConnectionFactory>(c => 
                        new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider));

                    container.Register<IMessageService>(new InMemoryTransientMessageService());

                    using (var sess = container.Resolve<IDbConnectionFactory>().Open())
                    {
                        sess.CreateTable<Contact>();
                        sess.CreateTable<ContactTag>();
                    }
                }
            }
            .Init();
        }

        [OneTimeTearDown]
        public void TestFixtureTearDown()
        {
            appHost.Dispose();
        }

        [Test]
        public void CreateContact()
        {
            var service = appHost.Container.Resolve<ContactService>();

            var response = service.Post(new Contact { Name = "World" });

            Assert.That(response.Name, Is.EqualTo("World"));
        }
    }
}
