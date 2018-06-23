using db;
using db.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using wServer.networking;
using wServer.networking.svrPackets;
using wServer.realm.entities;
using wServer.realm.entities.player;
using wServer.realm.setpieces;
using wServer.realm.worlds;

namespace wServer.realm.commands
{
	internal class VanishCommand : CommandBase
	{
		public VanishCommand()
		: base("vanish", RankType.Admin)
		{
		}

		public override int ArgumentAmount => 1;
		public override string Usage => "<vanish>";
		public override bool Handle(Player player)
		{
			if (!player.isNotVisible)
			{
				player.isNotVisible = true;
				player.Owner.PlayersCollision.Remove(player);			
				player.SendInfo("You're now hidden from all players!");
				return true;
			}
			player.isNotVisible = false;

			player.SendInfo("You're now visible to all players!");
			return true;
		}
	}
	internal class InvisibleCommand : CommandBase
	{
		public InvisibleCommand()
		: base("invisible", RankType.Admin)
		{
		}
		public override int ArgumentAmount => 1;
		public override string Usage => "<invisible>";
		public override bool Handle(Player player)
		{
			if (player.HasConditionEffect(ConditionEffectIndex.Invisible))
			{
				player.ApplyConditionEffect(new ConditionEffect
				{
					Effect = ConditionEffectIndex.Invisible,
					DurationMS = 0
				});
				player.SendInfo("Invisible Mode Off");
			}
			else
			{
				player.ApplyConditionEffect(new ConditionEffect
				{
					Effect = ConditionEffectIndex.Invisible,
					DurationMS = -1
				});
				player.SendInfo("Invisible Mode On");
			}
			return true;
		}
	}
	internal class GiftCommand : CommandBase
	{
		public GiftCommand()
			: base("gift", RankType.Admin)
		{
		}
		public override int ArgumentAmount => 1;
		public override string Usage => "<gift>";
		public override bool Handle(Player player)
		{
			if (Arguments.Length == 1)
			{
				player.SendHelp("Usage: /gift <Playername> <Itemname>");
				return false;
			}
			string name = string.Join(" ", Arguments.Skip(1).ToArray()).Trim();
			var plr = player.Manager.FindPlayer(Arguments[0]);
			ushort objType;
			Dictionary<string, ushort> icdatas = new Dictionary<string, ushort>(player.Manager.GameData.IdToObjectType,
				StringComparer.OrdinalIgnoreCase);
			if (!icdatas.TryGetValue(name, out objType))
			{
				player.SendError("Item not found, PERHAPS YOUR A RETARD LIKE LYNX WHO CAN'T SPELL SHIT?");
				return false;
			}
			if (!player.Manager.GameData.Items[objType].Secret || player.Client.Account.Rank >= 2)
			{
				for (int i = 0; i < plr.Inventory.Length; i++)
					if (plr.Inventory[i] == null)
					{
						plr.Inventory[i] = player.Manager.GameData.Items[objType];
						plr.UpdateCount++;
						plr.SaveToCharacter();
						player.SendInfo("Success sending " + name + " to " + plr.Name);
						plr.SendInfo("You got a " + name + " from " + player.Name);
						break;
					}
			}
			else
			{
				player.SendError("Item failed sending to " + plr.Name + ", make sure you spelt the command right, and their name!");
				return false;
			}
			return true;
		}
	}
	internal class Summon : CommandBase
		{
			public Summon()
				: base("summon", RankType.Admin)
			{
			}

			public override int ArgumentAmount => 1;
			public override string Usage => "<summon>";
			public override bool Handle(Player player)
			{
				foreach (KeyValuePair<string, Client> i in player.Manager.Clients)
				{
					if (i.Value.Player.Name.EqualsIgnoreCase(Arguments[0]))
					{
						Packet pkt;
						if (i.Value.Player.Owner == player.Owner)
						{
							i.Value.Player.Move(player.X, player.Y);
							pkt = new GotoPacket
							{
								ObjectId = i.Value.Player.Id,
								Position = new Position(player.X, player.Y)
							};
							i.Value.Player.UpdateCount++;
							player.SendInfo("Player summoned!");
						}
						else
						{
							pkt = new ReconnectPacket
							{
								GameId = player.Owner.Id,
								Host = "",
								IsFromArena = false,
								Key = player.Owner.PortalKey,
								KeyTime = -1,
								Name = player.Owner.Name,
								Port = -1
							};
							player.SendInfo("Player will connect to you now!");
						}

						i.Value.SendPacket(pkt);

						return true;
					}
				}
				player.SendError(string.Format("Player '{0}' could not be found!", Arguments));
				return false;
			}
		}
		internal class LevelCommand : CommandBase
		{
			public LevelCommand()
				: base("level", RankType.Admin)
			{
			}

			public override int ArgumentAmount => 1;
			public override string Usage => "<level>";
			public override bool Handle(Player player)
			{
				try
				{
					if (Arguments.Length == 0)
					{
						player.SendHelp("Use /level <ammount>");
						return false;
					}
					if (Arguments.Length == 1)
					{
						player.Client.Character.Level = int.Parse(Arguments[0]);
						player.Client.Player.Level = int.Parse(Arguments[0]);
						player.UpdateCount++;
						player.SendInfo("Success!");
					}
				}
				catch
				{
					player.SendError("Error!");
					return false;
				}
				return true;
			}
		}

		internal class SetFameCommand : CommandBase
		{
			public SetFameCommand() : base("setfame", RankType.Admin)
			{
			}

			public override int ArgumentAmount => 1;
			public override string Usage => "<setfame>";
			public override bool Handle(Player player)
			{
				if (string.IsNullOrEmpty(Arguments[0]))
				{
					player.SendHelp("Usage: /setfame <fame>");
					return false;
				}
				player.Manager.Database.DoActionAsync(db =>
				{
					var cmd = db.CreateQuery();
					cmd.CommandText = "UPDATE `stats` SET `fame`=@cre WHERE accId=@accId";
					cmd.Parameters.AddWithValue("@cre", Arguments[0]);
					cmd.Parameters.AddWithValue("@accId", player.AccountId);
					if (cmd.ExecuteNonQuery() == 0)
					{
						player.SendError("Error setting gold!");
					}
					else
					{
						player.SendInfo("Success!");
					}
				});
				return true;
			}
		}
		internal class SetGoldCommand : CommandBase
		{
			public SetGoldCommand() : base("setgold", RankType.Admin)
			{
			}

			public override int ArgumentAmount => 1;
			public override string Usage => "<setgold>";
			public override bool Handle(Player player)
			{
				if (string.IsNullOrEmpty(Arguments[0]))
				{
					player.SendHelp("Usage: /setgold <gold>");
					return false;
				}
				player.Manager.Database.DoActionAsync(db =>
				{
					var cmd = db.CreateQuery();
					cmd.CommandText = "UPDATE `stats` SET `credits`=@cre WHERE accId=@accId";
					cmd.Parameters.AddWithValue("@cre", Arguments[0]);
					cmd.Parameters.AddWithValue("@accId", player.AccountId);
					if (cmd.ExecuteNonQuery() == 0)
					{
						player.SendError("Error setting gold!");
					}
					else
					{
						player.SendInfo("Success!");
					}
				});
				return true;
			}
		}
		internal class Kick : CommandBase
		{
			public Kick()
				: base("kick", RankType.Admin)
			{
			}

			public override int ArgumentAmount => 1;
			public override string Usage => "<kick>";
			public override bool Handle(Player player)
			{
				if (Arguments.Length == 0)
				{
					player.SendHelp("Usage: /kick <playername>");
					return false;
				}
				try
				{
					foreach (KeyValuePair<int, Player> i in player.Owner.Players)
					{
						if (i.Value.Name.ToLower() == Arguments[0].ToLower().Trim())
						{
							player.SendInfo("Player Disconnected");
							i.Value.Client.Disconnect();
						}
					}
				}
				catch
				{
					player.SendError("Cannot kick!");
					return false;
				}
				return true;
			}
		}
		internal class StarTypeCommand : CommandBase
		{
			public StarTypeCommand() : base("startype", RankType.Admin)
			{
			}
			public override int ArgumentAmount => 1;
			public override string Usage => "<startype>";
			public override bool Handle(Player player)
			{
				if (Arguments.Length == 0)
				{
					player.SendInfo(ListStarTypes());
					return true;
				}
				if (Arguments.Length > 0 && TryGetStarType(string.Join(" ", Arguments), out var starType))
				{
					player.StarType = starType;
					player.UpdateCount++;

					player.Manager.Database.DoActionAsync(db =>
					{
						var cmd = db.CreateQuery();
						cmd.CommandText = "UPDATE stats SET starType=@st WHERE accId=@accId;";
						cmd.Parameters.AddWithValue("@accId", player.AccountId);
						cmd.Parameters.AddWithValue("@st", starType);
						cmd.ExecuteNonQuery();
					});
					player.SendInfo(
						$"Stars: {player.Stars} (Override: {player.Client.Account.Stats.StarsOverride}) Type: {player.StarType}");
					return true;
				}
				player.SendInfo(ListStarTypes());
				return false;
			}

			private string ListStarTypes()
			{
				var type = typeof(StarsUtil);
				var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
					.Where(f => f.IsLiteral && !f.IsInitOnly).ToArray();
				return string.Join(", ", fields.Select(f => $"{f.Name.Replace("_TYPE", "")} ({f.GetRawConstantValue()})"));
			}

			private bool TryGetStarType(string str, out int starType)
			{
				var type = typeof(StarsUtil);
				var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
					.Where(f => f.IsLiteral && !f.IsInitOnly
					&& (string.Equals(f.Name.Replace("_TYPE", ""), str.Replace("_TYPE", ""), StringComparison.OrdinalIgnoreCase)
					|| int.TryParse(str, out int d) && d == (int)f.GetRawConstantValue())).ToArray();

				if (fields.Length == 0)
				{
					starType = 0;
					return false;
				}
				starType = (int)fields.First().GetRawConstantValue();
				return true;
			}
		}

		internal class MaxStarsCommand : CommandBase
		{
			public MaxStarsCommand() : base("maxstars", RankType.Admin)
			{
			}
			public override int ArgumentAmount => 1;
			public override string Usage => "<maxstars>";
			public override bool Handle(Player player)
			{
				var stars = player.Manager.GameData.ObjectDescs.Count(od => od.Value.Player) * StarsUtil.StarCount;
				player.Stars = stars;
				player.UpdateCount++;

				player.Manager.Database.DoActionAsync(db =>
				{
					var cmd = db.CreateQuery();
					cmd.CommandText = "UPDATE stats SET starOverride=@st WHERE accId=@accId;";
					cmd.Parameters.AddWithValue("@accId", player.AccountId);
					cmd.Parameters.AddWithValue("@st", stars);
					cmd.ExecuteNonQuery();
				});
				return true;
			}
		}


		internal class GodLandsCommand : CommandBase
		{
			public GodLandsCommand()
				: base("gland", RankType.Player)
			{
			}

			public override int ArgumentAmount => 1;
			public override string Usage => "<gland>";
			public override bool Handle(Player player)
			{
				{
					if (!(player.Owner is GameWorld))
					{
						player.SendError("There isn't a gangster lands here.");
						return false;
					}
				}
				{
					int x, y;
					try
					{
						x = 1000;
						y = 1000;
					}
					catch
					{
						player.SendError("Invalid coordinates!");
						return false;
					}
					player.Move(x + 0.5f, y + 0.5f);
					player.UpdateCount++;
					player.Owner.BroadcastPacket(new GotoPacket
					{
						ObjectId = player.Id,
						Position = new Position
						{
							X = player.X,
							Y = player.Y
						}
					}, null);
				}
				return true;
			}
		}
		internal class Lefttomax : CommandBase
		{
			public Lefttomax()
				: base("lefttomax", RankType.Player)
			{
			}

			public override int ArgumentAmount => 1;
			public override string Usage => "<lefttomax>";
			public override bool Handle(Player player)
			{
				{
					player.SendInfo("Your level " + player.Level + " " + player.ObjectDesc.ObjectId + " needs");
					if (((player.ObjectDesc.MaxHitPoints - player.Stats[0]) / 5) * 5 <=
						((player.ObjectDesc.MaxHitPoints - player.Stats[0]) / 5))
					{
						player.SendInfo((player.ObjectDesc.MaxHitPoints - player.Stats[0]) + " (" +
										(((player.ObjectDesc.MaxHitPoints - player.Stats[0])) / 5) + ") to max health");
					}
					else if (((player.ObjectDesc.MaxHitPoints - player.Stats[0]) / 5) * 5 >=
							 ((player.ObjectDesc.MaxHitPoints - player.Stats[0]) / 5))
					{
						player.SendInfo((player.ObjectDesc.MaxHitPoints - player.Stats[0]) + " (" +
										(((player.ObjectDesc.MaxHitPoints - player.Stats[0])) / 5 + 1) + ") to max health");
					}
					if (((player.ObjectDesc.MaxMagicPoints - player.Stats[1]) / 5) * 5 <=
						((player.ObjectDesc.MaxMagicPoints - player.Stats[1]) / 5))
					{
						player.SendInfo((player.ObjectDesc.MaxMagicPoints - player.Stats[1]) + " (" +
										(((player.ObjectDesc.MaxMagicPoints - player.Stats[1])) / 5) + ") to max mana");
					}
					else if (((player.ObjectDesc.MaxMagicPoints - player.Stats[1]) / 5) * 5 >=
							 ((player.ObjectDesc.MaxMagicPoints - player.Stats[1]) / 5))
					{
						player.SendInfo((player.ObjectDesc.MaxMagicPoints - player.Stats[1]) + " (" +
										(((player.ObjectDesc.MaxMagicPoints - player.Stats[1])) / 5 + 1) + ") to max mana");
					}
					player.SendInfo((player.ObjectDesc.MaxAttack - player.Stats[2]) + " (" +
									(((player.ObjectDesc.MaxAttack - player.Stats[2]))) + ") to max attack");
					player.SendInfo((player.ObjectDesc.MaxDefense - player.Stats[3]) + " (" +
									(((player.ObjectDesc.MaxDefense - player.Stats[3]))) + ") to max defense");
					player.SendInfo((player.ObjectDesc.MaxSpeed - player.Stats[4]) + " (" +
									(((player.ObjectDesc.MaxSpeed - player.Stats[4]))) + ") to max speed");
					player.SendInfo((player.ObjectDesc.MaxHpRegen - player.Stats[5]) + " (" +
									(((player.ObjectDesc.MaxHpRegen - player.Stats[5]))) + ") to max vitality");
					player.SendInfo((player.ObjectDesc.MaxMpRegen - player.Stats[6]) + " (" +
									(((player.ObjectDesc.MaxMpRegen - player.Stats[6]))) + ") to max wisdom");
					player.SendInfo((player.ObjectDesc.MaxDexterity - player.Stats[7]) + " (" +
									(((player.ObjectDesc.MaxDexterity - player.Stats[7]))) + ") to max dexterity");
					player.SendInfo((player.ObjectDesc.MaxMight - player.Stats[8]) + " (" +
									(((player.ObjectDesc.MaxMight - player.Stats[8]))) + ") to max might");
					player.SendInfo((player.ObjectDesc.MaxLuck - player.Stats[9]) + " (" +
									(((player.ObjectDesc.MaxLuck - player.Stats[9]))) + ") to max luck");
				}
				return true;
			}
		}
		internal class GodCommand : CommandBase
		{
			public GodCommand()
				: base("god", RankType.Admin)
			{
			}

			public override int ArgumentAmount => 1;
			public override string Usage => "</god>";
			public override bool Handle(Player player)
			{
				if (player.HasConditionEffect(ConditionEffects.Invincible))
				{
					player.ApplyConditionEffect(new ConditionEffect()
					{
						Effect = ConditionEffectIndex.Invincible,
						DurationMS = 0,
					});
					player.SendInfo("Godmode Deactivated");
					return false;
				}
				else
				{
					player.ApplyConditionEffect(new ConditionEffect()
					{
						Effect = ConditionEffectIndex.Invincible,
						DurationMS = -1
					});
					player.SendInfo("Godmode Activated");
				}
				return true;
			}
		}

		internal class VisitCommand : CommandBase
		{
			public VisitCommand()
				: base("visit", RankType.Admin)
			{
			}

			public override int ArgumentAmount => 1;
			public override string Usage => "<player name>";
			public override bool Handle(Player player)
			{
				foreach (KeyValuePair<string, Client> i in player.Manager.Clients)
				{
					if (i.Value.Player.Name.EqualsIgnoreCase(Arguments[0]))
					{
						Packet pkt;
						if (i.Value.Player.Owner == player.Owner)
						{
							player.Move(i.Value.Player.X, i.Value.Player.Y);
							pkt = new GotoPacket
							{
								ObjectId = player.Id,
								Position = new Position(i.Value.Player.X, i.Value.Player.Y)
							};
							i.Value.Player.UpdateCount++;
							player.SendInfo("He is here already. git fast.");
						}
						else
						{
							player.Client.Reconnect(new ReconnectPacket()
							{
								GameId = i.Value.Player.Owner.Id,
								Host = "",
								IsFromArena = false,
								Key = Empty<byte>.Array,
								KeyTime = -1,
								Name = i.Value.Player.Owner.Name,
								Port = -1,
							});
							player.SendInfo("You are visiting " + i.Value.Player.Owner.Id + " now");
						}

						return true;
					}
				}
				player.SendError(string.Format("Player '{0}' could not be found!", Arguments));
				return false;
			}
		}

		internal class SizeCommand : CommandBase ///wip dont use no work
		{
			public SizeCommand() : base("size", RankType.Donor)
			{
			}

			public override int ArgumentAmount => 1;
			public override string Usage => "<size>";
			public override bool Handle(Player player)
			{
				player.Size = int.Parse(Arguments[0]);
				player.UpdateCount++;
				player.SendInfo("Your size has been changed!");
				return true;
			}
		}

		public class GiveCommand : CommandBase
		{
			public GiveCommand() : base("give", RankType.Admin) { }

			public override int ArgumentAmount => 1;
			public override string Usage => "<item>";
			public override bool Handle(Player player)
			{
				var item = string.Join(" ", Arguments);

				if (player.Manager.GameData.IdToObjectType.ContainsKey(item))
				{
					for (var i = 4; i < player.Inventory.Length; i++)
						if (player.Inventory[i] == null)
						{
							player.Inventory[i] = player.Manager.GameData.Items[player.Manager.GameData.IdToObjectType[item]];
							break;
						}
					return true;
				}
				SayUsage = true;
				return false;
			}
		}

	class SpawnCommand : CommandBase
	{
		public SpawnCommand() : base("spawn", RankType.Admin) { }

		public override int ArgumentAmount => 1;
		public override string Usage => "<spawn>";
		public override bool Handle(Player player)
		{
			if (player.Owner.Name != "Nexus")
			{
				int num;
				if (Arguments.Length > 0 && int.TryParse(Arguments[0], out num)) //multi
				{
					string name = string.Join(" ", Arguments.Skip(1).ToArray());
					ushort objType;
					//creates a new case insensitive dictionary based on the XmlDatas
					Dictionary<string, ushort> icdatas = new Dictionary<string, ushort>(
						player.Manager.GameData.IdToObjectType,
						StringComparer.OrdinalIgnoreCase);
					if (!icdatas.TryGetValue(name, out objType) ||
						!player.Manager.GameData.ObjectDescs.ContainsKey(objType))
					{
						player.SendInfo("Unknown entity!");
						return false;
					}
					int c = int.Parse(Arguments[0]);
					for (int i = 0; i < num; i++)
					{
						Entity entity = Entity.Resolve(player.Manager, objType);
						entity.Move(player.X, player.Y);
						player.Owner.EnterWorld(entity);
					}
					player.SendInfo("Success!");
				}
				else
				{
					string name = string.Join(" ", Arguments);
					ushort objType;
					//creates a new case insensitive dictionary based on the XmlDatas
					Dictionary<string, ushort> icdatas = new Dictionary<string, ushort>(
						player.Manager.GameData.IdToObjectType,
						StringComparer.OrdinalIgnoreCase);
					if (!icdatas.TryGetValue(name, out objType) ||
						!player.Manager.GameData.ObjectDescs.ContainsKey(objType))
					{
						player.SendHelp("Usage: /spawn <entityname>");
						return false;
					}
					Entity entity = Entity.Resolve(player.Manager, objType);
					entity.Move(player.X, player.Y);
					player.Owner.EnterWorld(entity);
				}
			}
			else
			{
				player.SendInfo("You cannot spawn in Nexus.");
				return false;
			}
			return true;
		}
	}

	public class SpawnXCommand : CommandBase
		{
			public SpawnXCommand() : base("spawnx", RankType.Admin) { }

			public override int ArgumentAmount => 2;
			public override string Usage => "<amount> <entity>";
			public override bool Handle(Player player)
			{
				var amount = int.Parse(Arguments[0]);
				if (amount > 25)
					return false;

				var entity = string.Join(" ", Arguments.Skip(1).ToArray());

				if (player.Manager.GameData.IdToObjectType.ContainsKey(entity))
				{
					for (var i = 0; i < amount; i++)
					{
						var e = Entity.Resolve(player.Manager, entity);
						e.Move(player.X, player.Y);
						player.Owner.EnterWorld(e);
					}
					return true;
				}
				SayUsage = true;
				return false;
			}
		}

		public class MaxCommand : CommandBase
		{
			public MaxCommand() : base("max", RankType.Admin) { }

			public override bool Handle(Player player)
			{
				var objDesc = player.ObjectDesc;

				player.Stats[0] = objDesc.MaxHitPoints;
				player.Stats[1] = objDesc.MaxMagicPoints;
				player.Stats[2] = objDesc.MaxAttack;
				player.Stats[3] = objDesc.MaxDefense;
				player.Stats[4] = objDesc.MaxSpeed;
				player.Stats[5] = objDesc.MaxHpRegen;
				player.Stats[6] = objDesc.MaxMpRegen;
				player.Stats[7] = objDesc.MaxDexterity;
				player.Stats[8] = objDesc.MaxMight;
				player.Stats[9] = objDesc.MaxLuck;

				player.SaveToCharacter();
				player.UpdateCount++;
				player.Client.Save();
				player.SendInfo("Your stats has been maxed!");

				return true;
			}
		}

		public class AnnouncementCommand : CommandBase
		{
			public AnnouncementCommand() : base("announce", RankType.Admin) { }

			public override int ArgumentAmount => 1;
			public override string Usage => "<text>";

			public override bool Handle(Player player)
			{
				var announceText = string.Join(" ", Arguments);

				foreach (Client clients in player.Manager.Clients.Values)
				{
					clients.SendPacket(new TextPacket
					{
						BubbleTime = 0,
						Stars = -1,
						Name = "@ANNOUNCEMENT",
						Text = announceText
					});
				}

				return true;
			}
		}

		internal class SetCommand : CommandBase
		{
			public SetCommand()
				: base("setstat", RankType.Admin)
			{
			}
			public override int ArgumentAmount => 1;
			public override string Usage => "<setstat>";
			public override bool Handle(Player player)
			{
				if (Arguments.Length == 2)
				{
					try
					{
						string stat = Arguments[0].ToLower();
						int amount = int.Parse(Arguments[1]);
						switch (stat)
						{
							case "health":
							case "hp":
								player.Stats[0] = amount;
								break;

							case "mana":
							case "mp":
								player.Stats[1] = amount;
								break;

							case "atk":
							case "attack":
								player.Stats[2] = amount;
								break;

							case "def":
							case "defence":
								player.Stats[3] = amount;
								break;

							case "spd":
							case "speed":
								player.Stats[4] = amount;
								break;

							case "vit":
							case "vitality":
								player.Stats[5] = amount;
								break;

							case "wis":
							case "wisdom":
								player.Stats[6] = amount;
								break;

							case "dex":
							case "dexterity":
								player.Stats[7] = amount;
								break;

							case "mgt":
							case "might":
								player.Stats[8] = amount;
								break;

							case "luc":
							case "luck":
								player.Stats[9] = amount;
								break;

							default:
								player.SendError("Invalid Stat");
								player.SendHelp("Stats: Health, Mana, Attack, Defence, Speed, Vitality, Wisdom, Dexterity, Might, Luck");
								player.SendHelp("Shortcuts: Hp, Mp, Atk, Def, Spd, Vit, Wis, Dex, Mgt, Luc");
								return false;
						}
						player.SaveToCharacter();
						player.Client.Save();
						player.UpdateCount++;
						player.SendInfo("Success");
					}
					catch
					{
						player.SendError("Error while setting stat");
						return false;
					}
					return true;
				}
				else if (Arguments.Length == 3)
				{
					foreach (Client i in player.Manager.Clients.Values)
					{
						if (i.Account.Name.EqualsIgnoreCase(Arguments[0]))
						{
							try
							{
								string stat = Arguments[1].ToLower();
								int amount = int.Parse(Arguments[2]);
								switch (stat)
								{
									case "health":
									case "hp":
										i.Player.Stats[0] = amount;
										break;

									case "mana":
									case "mp":
										i.Player.Stats[1] = amount;
										break;

									case "atk":
									case "attack":
										i.Player.Stats[2] = amount;
										break;

									case "def":
									case "defence":
										i.Player.Stats[3] = amount;
										break;

									case "spd":
									case "speed":
										i.Player.Stats[4] = amount;
										break;

									case "vit":
									case "vitality":
										i.Player.Stats[5] = amount;
										break;

									case "wis":
									case "wisdom":
										i.Player.Stats[6] = amount;
										break;

									case "dex":
									case "dexterity":
										i.Player.Stats[7] = amount;
										break;

									default:
										player.SendError("Invalid Stat");
										player.SendHelp(
											"Stats: Health, Mana, Attack, Defence, Speed, Vitality, Wisdom, Dexterity");
										player.SendHelp("Shortcuts: Hp, Mp, Atk, Def, Spd, Vit, Wis, Dex");
										return false;
								}
								i.Player.SaveToCharacter();
								i.Player.Client.Save();
								i.Player.UpdateCount++;
								player.SendInfo("Success");
							}
							catch
							{
								player.SendError("Error while setting stat");
								return false;
							}
							return true;
						}
					}
					player.SendError(string.Format("Player '{0}' could not be found!", Arguments));
					return false;
				}
				else
				{
					player.SendHelp("Usage: /setStat <Stat> <Amount>");
					player.SendHelp("or");
					player.SendHelp("Usage: /setStat <Player> <Stat> <Amount>");
					player.SendHelp("Shortcuts: Hp, Mp, Atk, Def, Spd, Vit, Wis, Dex");
					return false;
				}
			}
		}
	}

