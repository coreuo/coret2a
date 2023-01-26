using Core.Abstract.Attributes;

namespace Scripts.MobileEquip
{
    [Entity("Mobile")]
    public interface IMobile
    {
        bool Combat { get; set; }
    }
}
