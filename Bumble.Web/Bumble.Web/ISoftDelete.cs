namespace Bumble.Web
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}