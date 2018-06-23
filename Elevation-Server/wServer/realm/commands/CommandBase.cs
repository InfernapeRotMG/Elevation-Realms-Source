using wServer.realm.entities.player;

namespace wServer.realm.commands
{
	public enum RankType : int
	{
		Player = 0,
		Donor = 1,
		Admin = 2
	}

	public class CommandBase
	{
		public RankType RankType { get; set; }
		public string CommandType { get; set; }

		public bool SayUsage { get; set; }
		public string[] Arguments { get; set; }

		public CommandBase(string commandType, RankType rankType)
		{
			CommandType = commandType;
		}

		public virtual int ArgumentAmount => 0;
		public virtual string Usage => null;
		public virtual bool Handle(Player player) => false;
	}
}
