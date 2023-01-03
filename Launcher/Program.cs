using Core.Abstract.Attributes;
using Launcher;
using Launcher.Domain;

[assembly: Launcher]

using var save = new Save();

var (login, shard) = Initialize(save);

shard.Listen();

login.Listen();

while (true)
{
    login.Slice();

    shard.Slice();

    await Task.Delay(1);
}

static (LoginServer, ShardServer) Initialize(Save save)
{
    var login = save.LoginServerStore.Lease();

    "127.0.0.1".CopyTo(login.IpAddress);

    login.Port = 2593;

    var account = save.AccountStore.Lease();

    "Admin".CopyTo(account.Name);

    "password".CopyTo(account.Password);

    login.Accounts.Add(account);

    var character = save.MobileStore.Lease();

    "Captain Jack".CopyTo(character.Name);

    //"a".CopyTo(character.Password);

    account.Characters.Add(character);

    var shard = save.ShardServerStore.Lease();

    "127.0.0.1".CopyTo(shard.IpAddress);

    shard.Port = 2594;

    "Pirates".CopyTo(shard.Name);

    login.Shards.Add(shard);

    return (login, shard);
}

