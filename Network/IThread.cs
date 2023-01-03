namespace Network;

public interface IThread
{
    bool Locked { get; set; }

    bool Running { get; set; }
}