using System;
using System.Linq;
using wServer.networking.cliPackets;
using wServer.networking.svrPackets;
using wServer.realm.entities;
using wServer.realm.entities.player;

namespace wServer.realm.commands
{
    public class TradeCommand : CommandBase
    {
        public TradeCommand() : base("trade", RankType.Player) { }

        public override int ArgumentAmount => 1;
        public override string Usage => "<player name>";
        public override bool Handle(Player player)
        {
            var playerName = Arguments[0];
            if (String.IsNullOrWhiteSpace(playerName))
            {
                SayUsage = true;
                return false;
            }

            player.Manager.Logic.AddPendingAction(t => player.RequestTrade(t, new RequestTradePacket() { Name = playerName }));
            return true;
        }
    }
    public class ServerCommand : CommandBase
    {
        public ServerCommand() : base("server", RankType.Player) { }

        public override bool Handle(Player player)
        {
            player.SendInfo($"You are in: {player.Owner.Name}");
            return true;
        }
    }
    public class PauseCommand : CommandBase
    {
        public PauseCommand() : base("pause", RankType.Player) { }

        public override bool Handle(Player player)
        {
            var isPaused = player.HasConditionEffect(ConditionEffectIndex.Paused);
            var playerInfoText = isPaused ? "Game resumed." : "Game paused.";

            if (player.Owner.EnemiesCollision.HitTest(player.X, player.Y, 8).OfType<Enemy>().Where(_ => _.ObjectDesc.Enemy).Count() > 0)
            {
                player.SendInfo("Not safe to pause.");
                return false;
            }

            player.ApplyConditionEffect(new ConditionEffect
            {
                Effect = ConditionEffectIndex.Paused,
                DurationMS = isPaused ? -1 : 0
            });
            player.SendInfo(playerInfoText);
            return true;
        }
    }
    public class TeleportCommand : CommandBase
    {
        public TeleportCommand() : base("teleport", RankType.Player) { }

        public override int ArgumentAmount => 1;
        public override string Usage => "<player name>";
        public override bool Handle(Player player)
        {
            var playerName = Arguments[0].ToLower();

            if (String.IsNullOrWhiteSpace(playerName))
            {
                SayUsage = true;
                return false;
            }

            if (String.Equals(player.Name.ToLower(), playerName))
            {
                player.SendInfo("You are already at yourself, and always will be!");
                return false;
            }

            var playerTarget = player.Owner.GetPlayerByName(playerName);
            if (playerTarget == null)
            {
                player.SendInfo($"Player {playerName} not found");
                return false;
            }
            player.Manager.Logic.AddPendingAction(t => player.Teleport(t, new TeleportPacket() { ObjectId = playerTarget.Id }));
            return true;
        }
        public class TellCommand : CommandBase
        {
            public TellCommand() : base("tell", RankType.Player) { }

            public override int ArgumentAmount => 2;
            public override string Usage => "<player name> <text>";
            public override bool Handle(Player player)
            {
                if (!player.NameChosen)
                {
                    player.SendError("Choose a name!");
                    return false;
                }

                var playerName = Arguments[0].Trim().ToLower();
                var text = string.Join(" ", Arguments, 1, Arguments.Length - 1);

                if (String.Equals(player.Name.ToLower(), playerName))
                {
                    player.SendInfo("Quit telling yourself!");
                    return false;
                }

                var playerTarget = player.Owner.GetPlayerByName(playerName);
                if (playerTarget == null)
                {
                    player.SendError($"{playerName} not found");
                    return false;
                }

                if (playerTarget.NameChosen)
                {
                    player.Client.SendPacket(new TextPacket()
                    {
                        ObjectId = player.Id,
                        BubbleTime = 10,
                        Stars = player.Stars,
                        Name = player.Name,
                        Recipient = playerTarget.Name,
                        Text = text.ToSafeText(),
                        CleanText = ""
                    });

                    playerTarget.Client.SendPacket(new TextPacket()
                    {
                        ObjectId = playerTarget.Owner.Id == player.Owner.Id ? player.Id : -1,
                        BubbleTime = 10,
                        Stars = player.Stars,
                        Name = player.Name,
                        Recipient = playerTarget.Name,
                        Text = text.ToSafeText(),
                        CleanText = ""
                    });
                }

                return true;
            }
        }
    }
}