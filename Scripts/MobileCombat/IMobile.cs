using Core.Abstract.Attributes;

namespace Scripts.MobileCombat
{
    [Entity("Mobile")]
    public interface IMobile
    {
        bool Combat { get; set; }
    }
}
