#region

using db;
using MySql.Data.MySqlClient;
using wServer.networking.cliPackets;
using wServer.networking.svrPackets;
using wServer.realm;
using wServer.realm.entities.player;
using FailurePacket = wServer.networking.svrPackets.FailurePacket;

#endregion

namespace wServer.networking.handlers
{
    internal class CreateHandler : PacketHandlerBase<CreatePacket>
    {
        public override PacketID ID
        {
            get { return PacketID.CREATE; }
        }

        protected override void HandlePacket(Client client, CreatePacket packet)
        {
            using (var dbx = new Database())
            {
                var cmd = dbx.CreateQuery();
                var nextCharId = 1;
                if (client.Account != null)
                {
                    nextCharId = dbx.GetNextCharId(client.Account);

                    cmd.CommandText = "SELECT maxCharSlot FROM accounts WHERE id=@accId;";
                    cmd.Parameters.AddWithValue("@accId", client.Account.AccountId);
                    var maxChar = (int) cmd.ExecuteScalar();

                    cmd = dbx.CreateQuery();
                    cmd.CommandText = "SELECT COUNT(id) FROM characters WHERE accId=@accId AND dead = FALSE;";
                    cmd.Parameters.AddWithValue("@accId", client.Account.AccountId);
                    var currChar = (int) (long) cmd.ExecuteScalar();

                    if (currChar >= maxChar)
                    {
                        client.Disconnect();
                        return;
                    }
                }
                client.Character = Database.CreateCharacter(client.Manager.GameData, (ushort) packet.ClassType, nextCharId);

                var stats = new[]
                {
                    client.Character.MaxHitPoints,
                    client.Character.MaxMagicPoints,
                    client.Character.Attack,
                    client.Character.Defense,
                    client.Character.Speed,
                    client.Character.Dexterity,
                    client.Character.HpRegen,
                    client.Character.MpRegen,
                    client.Character.Might,
                    client.Character.Luck
                };

                if (client.Account.Donator)
                {
                    Client.Character.Backpack = new[] { -1, -1, -1, -1, -1, -1, -1, -1 };
                    Client.Character.HasBackpack = 1;
                    dbx.SaveBackpacks(Client.Character, Client.Account);
                }

                var skin = client.Account.OwnedSkins.Contains(packet.SkinType) ? packet.SkinType : 0;
                client.Character.Skin = skin;
                cmd = dbx.CreateQuery();
                cmd.Parameters.AddWithValue("@accId", client.Account.AccountId);
                cmd.Parameters.AddWithValue("@charId", nextCharId);
                cmd.Parameters.AddWithValue("@charType", packet.ClassType);
                cmd.Parameters.AddWithValue("@items", Utils.GetCommaSepString(client.Character.EquipSlots()));
                cmd.Parameters.AddWithValue("@stats", Utils.GetCommaSepString(stats));
                cmd.Parameters.AddWithValue("@fameStats", client.Character.FameStats.ToString());
                cmd.Parameters.AddWithValue("@skin", skin);
                cmd.Parameters.AddWithValue("@hasBackpack", client.Account.Donator);
                cmd.CommandText = "INSERT INTO characters (accId, hasBackpack, charId, charType, level, exp, fame, items, hp, mp, stats, dead, pet, fameStats, skin) VALUES (@accId, @hasBackpack, @charId, @charType, 1, 0, 0, @items, 100, 100, @stats, FALSE, -1, @fameStats, @skin);";

                if (cmd.ExecuteNonQuery() > 0)
                {
                    var target = client.Manager.Worlds[client.TargetWorld];
                    client.SendPacket(new Create_SuccessPacket
                    {
                        CharacterID = client.Character.CharacterId,
                        ObjectID =
                            client.Manager.Worlds[client.TargetWorld].EnterWorld(
                                client.Player = new Player(client.Manager, client))
                    });
                    client.Stage = ProtocalStage.Ready;
                }
                else
                {
                    client.SendPacket(new FailurePacket
                    {
                        ErrorDescription = "Failed to Load character."
                    });
                }
            }
        }
    }
}