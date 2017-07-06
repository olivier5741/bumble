using ServiceStack;

namespace Bumble.Web
{
    [Route("/cms/contact/find","GET")]
    public class ContactFind : QueryDb<Contact>, ILeftJoin<Contact,ContactTag>
    {
        public string NameContains { get; set; }
        public string ContactTagValue { get; set; } // TODO should be renamed to tag
    }
}