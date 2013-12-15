using UnityEngine;
using System.Collections;

public class BCombatLog : MonoBehaviour {

	public UILabel lastEntry;
	public UILabel fullLog;

	CombatLog combatLog;

	public void Init(CombatLog combatLog) 
	{
		this.combatLog = combatLog;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(combatLog != null) {
			lastEntry.text = combatLog.GetLast();
			fullLog.text = combatLog.ToString();
		}
	}
}
