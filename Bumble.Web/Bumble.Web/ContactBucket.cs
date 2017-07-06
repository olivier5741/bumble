using System;
using ServiceStack;

namespace Bumble.Web
{
    [Route("/cms/bucket", "POST,GET,PUT,DELETE")]
    public class ContactBucket
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}