using System;
using System.Collections.Generic;

public class Combat
{
	public int round = 0;
	public int turn = 0;
	Queue<Unit> unitQueue = new Queue<Unit>();

	public CombatLog log;

	public Combat() 
	{
		log = new CombatLog();
		log.Init();
		EventProxyManager.FireEvent(this, new CombatLogInitializedEvent(log));
	}

    public void SetupRound(List<Unit> unitList)
    {
		// count rounds
        round++;
		turn = 0;
		// reset units abilitys
		foreach (Unit unit in unitList) {
			unit.ResetTurn();	
		}

		foreach(Unit unit in unitList) {
			EventProxyManager.FireEvent(this, new DebugLogEvent("Unsorded Unit: " + unit.Name + " " + unit.team));
		}

		// since this include sorting the list we use a refrence type
		// so we can use the sorted list after
        FillUnitQueue(ref unitList);

		foreach(Unit unit in unitList) {
			EventProxyManager.FireEvent(this, new DebugLogEvent("Sorted Unit: " + unit.Name + " " + unit.team));
		}

        EventProxyManager.FireEvent(this, new RoundSetupEvent(unitList));
    }

    public Unit GetNextUnit()
    {
		return unitQueue.Dequeue();
    }

	public int TurnsLeft()
	{
		return unitQueue.Count;
	}

	void FillUnitQueue(ref List<Unit> unitList) 
	{
		// sort unit list
		unitList.Sort();
		// clear queue and refill with sorted list
		if(unitQueue != null)
			unitQueue.Clear();
		unitQueue = new Queue<Unit>();

		foreach(Unit unit in unitList)
		{
			unitQueue.Enqueue(unit);
		}
	}
}

public class CombatLogInitializedEvent : EventProxyArgs
{
	public CombatLog combatLog;

	public CombatLogInitializedEvent (CombatLog combatLog)
	{
		this.combatLog = combatLog;
		this.name = EventName.CombatLogInitialized;
	}
	
}

public class RoundSetupEvent : EventProxyArgs
{
	public List<Unit> sortedList;

	public RoundSetupEvent(List<Unit> unitList) {
		this.sortedList = unitList;
		this.name = EventName.RoundSetup;
	}
}

