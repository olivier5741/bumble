using System;
using System.Collections.Generic;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.Messaging;
using ServiceStack.OrmLite;
using ServiceStack.Testing;
using ServiceStack.Text;

namespace Bumble.Web.Tests
{
    [TestFixture]
    public class ContactServiceTests
    {
        private ServiceStackHost _appHost;
        private ContactService _contactService;
        private readonly Guid _tenantId = Guid.NewGuid();
        private readonly string _tenantKey = "test";
        private Contact _contact1;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _appHost = new BasicAppHost(typeof(ContactService).Assembly)
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
            };

            _appHost.Plugins.Add(new AutoQueryFeature());

            _appHost.RegisterTypedRequestFilter<IBelonger>((req, res, dtoInterface) =>
            {
                if (dtoInterface.TenantKey != null && dtoInterface.TenantKey.ToLower() == _tenantKey)
                    dtoInterface.TenantId = _tenantId;
            });

            _appHost.Init();

            _contactService = _appHost.Container.Resolve<ContactService>();
        }

        [OneTimeTearDown]
        public void TestFixtureTearDown()
        {
            _appHost.Dispose();
        }

        [Test]
        [Order(1)]
        public void CreateContact()
        {
            _contact1 = _contactService.Post(new Contact {TenantId = _tenantId, Name = "Sandy"});
            Assert.That(_contact1.TenantId, Is.EqualTo(_tenantId));
            Assert.That(_contact1.Name, Is.EqualTo("Sandy"));
        }

        [Test]
        [Order(2)]
        public void PatchContact()
        {
            var contact = _contactService.Patch(new Contact
            {
                Id = _contact1.Id,
                TenantId = _tenantId,
                Tags = new List<ContactTag> {new ContactTag {Value = "Person"}}
            });
            Assert.That(contact.Name, Is.EqualTo("Sandy"));
            Assert.AreEqual(1, contact.Tags.Count);
        }
        
        [Test]
        [Order(3)]
        public void FindContact()
        {
            var res = _appHost.ServiceController.Execute(new ContactFind
            {
                ContactTagValue = "Person",
                TenantId = _tenantId,
                NameContains = "Sa"
            });
            
            res.PrintDump();
            
            Assert.That(((QueryResponse<Contact>)res).Total, Is.EqualTo(1));
        }
    }
}