using System.Collections;
using System.Collections.Generic;

public class CombatLog 
{
	List<ILogEntry> log = new List<ILogEntry>();

	public void Init() 
	{
		EventProxyManager.RegisterForEvent(EventName.UnitMoved, HandleUnitMoved);
		EventProxyManager.RegisterForEvent(EventName.UnitAttacked, HandleUnitAttacked);
	}

	#region eventhandler
	void HandleUnitMoved (object sender, EventProxyArgs args)
	{
		log.Add(new LogEntryMove(args as UnitMovedEvent));
	}

	void HandleUnitAttacked (object sender, EventProxyArgs args)
	{
		log.Add(new LogEntryAttack(args as UnitAttackedEvent));
	}
	#endregion

	public string GetLast()
	{
		return log[log.Count-1].ToString();
	}

	public override string ToString ()
	{
		string str = "";
		foreach(ILogEntry entry in log) {
			str += entry.ToString() + "\n";
		}

		return str;
	}
}
