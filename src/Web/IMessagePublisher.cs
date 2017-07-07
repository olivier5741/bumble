namespace Bumble.Web
{
    public interface IMessagePublisher
    {
        void Publish<T>(T message) where T : class;
        void Publish<T>(T message, string topic) where T : class;
    }
}