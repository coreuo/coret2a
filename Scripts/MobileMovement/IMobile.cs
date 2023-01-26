using Core.Abstract.Attributes;

namespace Scripts.MobileMovement
{
    [Entity("Mobile")]
    public interface IMobile
    {
        byte Notoriety { get; }
    }
}
