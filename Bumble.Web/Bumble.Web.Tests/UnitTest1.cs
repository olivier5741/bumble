using System;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Testing;

namespace Bumble.Web.Tests
{
    [TestFixture]
    public class UnitTests
    {
        private readonly ServiceStackHost appHost;

        public UnitTests()
        {
            appHost = new BasicAppHost(typeof(ContactService).Assembly)
            {
                ConfigureContainer = container =>
                {
                    //Add your IoC dependencies here
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
