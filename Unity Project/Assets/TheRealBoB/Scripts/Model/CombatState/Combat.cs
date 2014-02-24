using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class Combat
{
	public int round = 0;
	public int turn = 0;
	Queue<Unit> currentRound = new Queue<Unit>();
	List<Unit> activeUnits = new List<Unit>();

	public void Init(List<Unit> unitList)
	{
		unitList.Sort();
		activeUnits = new List<Unit>(unitList);
	}

    public void SetupRound()
    {
		currentRound.Clear();
		foreach(Unit unit in activeUnits) {
			unit.ResetTurn();
			currentRound.Enqueue(unit);
		}

		// count rounds
        round++;
		turn = 0;

		EventProxyManager.FireEvent(this, new RoundSetupEvent(currentRound.ToList<Unit>()));
    }

	/// <summary>
	/// Find first Unit alive in queue; return it and enqueue this unit again
	/// </summary>
	/// <returns>The next unit.</returns>
    public Unit GetNextUnit()
    {
		if(currentRound.Count == 0) return null;
		Unit unit = currentRound.Dequeue();

		if(!unit.Alive) {
			activeUnits.Remove(unit);
		}

		return unit;
    }

	public int TurnsLeft()
	{
		return (currentRound.Count);
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

