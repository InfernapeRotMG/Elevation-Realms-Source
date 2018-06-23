using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using wServer.realm.entities.player;

namespace wServer.realm.commands
{
    public class CommandHandler
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CommandHandler));

        private Dictionary<string, CommandBase> CommandMap { get; set; }

        public CommandHandler()
        {
            CommandMap = new Dictionary<string, CommandBase>();
        }

        public void LoadCommands()
        {
            foreach (var type in Assembly.GetAssembly(typeof(CommandBase)).GetTypes().Where(_ => _.IsClass && !_.IsAbstract && _.IsSubclassOf(typeof(CommandBase))))
            {
                var commandBase = (CommandBase)Activator.CreateInstance(type);
                CommandMap.Add(commandBase.CommandType, commandBase);
            }

            log.Info($"Loaded: {CommandMap.Count} Commands");
        }

        public void HandleCommand(Player player, string text)
        {
            var index = text.IndexOf(' ');
            var commandType = text.Substring(1, index == -1 ? text.Length - 1 : index - 1);

            if (!CommandMap.ContainsKey(commandType))
            {
                player.SendInfo($"Unrecognized Command: /{commandType}");
                return;
            }

            var command = CommandMap[commandType];
            if (command.RankType == RankType.Admin && !player.Client.Account.Admin)
            {
                player.SendInfo($"You are not an admin");
                return;
            }

            if (command.RankType == RankType.Donor && !player.Client.Account.Donator)
            {
                player.SendInfo($"You are not an donator");
                return;
            }

            command.Arguments = null;

            var args = (index == -1 ? "" : text.Substring(index + 1));

            command.Arguments = args.Split(' ');

            try
            {
                if (!(command.Arguments.Length < command.ArgumentAmount))
                    if (command.Handle(player))
                    {
                        player.UpdateCount++;
                        return;
                    }
            }
            catch
            {
            }

            if (command.SayUsage)
                player.SendInfo($"Usage: /{commandType} {command.Usage}");
            player.UpdateCount++;
        }
    }
}
