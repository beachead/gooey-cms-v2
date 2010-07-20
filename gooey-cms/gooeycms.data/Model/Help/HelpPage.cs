
namespace Gooeycms.Data.Model.Help
{
    public class HelpPage : BasePersistedItem
    {
        public virtual string Path { get; set; }
        public virtual string Text { get; set; }
        public virtual string Hash { get; set; }
    }
}
