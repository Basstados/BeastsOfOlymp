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
		if(unitQueue.Count == 0) {
			FillUnitQueue(ref unitList);
		}

		// count rounds
        round++;
		turn = 0;
		// reset units abilitys
		foreach (Unit unit in unitList) {
			unit.ResetTurn();	
		}

        EventProxyManager.FireEvent(this, new RoundSetupEvent(unitList));
    }

	/// <summary>
	/// Find first Unit alive in queue; return it and enqueue this unit again
	/// </summary>
	/// <returns>The next unit.</returns>
    public Unit GetNextUnit()
    {
		Unit unit = unitQueue.Dequeue();
		while(!unit.Alive && unitQueue.Count > 0) {
			unit = unitQueue.Dequeue();
		}
		unitQueue.Enqueue(unit);
		if(unitQueue.Count == 0) return null;
		return unit;
    }

	public int TurnsLeft()
	{
		return (unitQueue.Count - turn);
	}

	void FillUnitQueue(ref List<Unit> unitList) 
	{
		// sort unit list
		unitList.Sort();
		// first round, use list to fill queue
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

