using ServiceStack.Text;

namespace Bumble.Web
{
    public class DummyBus : IMessagePublisher
    {
        public void Publish<T>(T message) where T : class
        {
            message.PrintDump();
        }

        public void Publish<T>(T message, string topic) where T : class
        {
            message.PrintDump();
        }
    }
}