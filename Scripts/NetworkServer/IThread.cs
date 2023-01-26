namespace Scripts.NetworkServer;

public interface IThread
{
    bool Locked { get; set; }

    bool Running { get; set; }
}