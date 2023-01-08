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

    var loginAccount = save.LoginAccountStore.Lease();

    "Admin".CopyTo(loginAccount.Name);

    "password".CopyTo(loginAccount.Password);

    login.Accounts.Add(loginAccount);

    var shard = save.ShardServerStore.Lease();

    "127.0.0.1".CopyTo(shard.IpAddress);

    shard.Port = 2594;

    "Pirates".CopyTo(shard.Name);

    login.Shards.Add(shard);

    var shardAccount = save.ShardAccountStore.Lease();

    "Admin".CopyTo(shardAccount.Name);

    shard.Accounts.Add(shardAccount);

    var character = save.MobileStore.Lease();

    "Captain Jack".CopyTo(character.Name);

    character.Body = 0x190;

    character.Hue = 0x83EA;

    character.X = 0x660;

    character.Y = 0x68A;

    character.Z = 0xF;

    character.Direction = 0x4;

    var map = save.MapStore.Lease();

    map.Width = 0x1800;

    map.Height = 0x1000;

    character.Map = map;

    //"a".CopyTo(character.Password);

    foreach (var item in character.Skills)
    {
        var skill = item;

        skill.Value = (ushort)(item.Id * 0xA);
    }

    shardAccount.Characters.Add(character);

    return (login, shard);
}
