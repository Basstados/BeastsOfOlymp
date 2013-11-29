using System;
using System.Collections.Generic;

public class Combat
{
	public int round;
	public Queue<Unit> unitTurnQueue;

	void FillUnitQueue(List<Unit> unitList) 
	{
		// sort unit list
		unitList.Sort();
		// clear queue and refill with sorted list
		if(unitTurnQueue != null)
			unitTurnQueue.Clear();
		unitTurnQueue = new Queue<Unit>(unitList);
	}
}

