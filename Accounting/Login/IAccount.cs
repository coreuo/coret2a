using Core.Abstract.Attributes;
using Core.Abstract.Extensions;

namespace Accounting.Login;

[Entity("Login", "Account")]
public interface IAccount
{
    [Size(30)]
    Span<char> Name { get; }

    [Size(30)]
    Span<char> Password { get; }

    int AccessKey { get; set; }

    internal void GenerateAccessKey()
    {
        AccessKey = Utility.Random.Next() | (Utility.Random.Next() % 2 > 0 ? 1 << 31 : 0);
    }
}