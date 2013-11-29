using UnityEngine;
using System.Collections;

public class BUnit : MonoBehaviour {

	public Unit unit;
	public BCombatMenu bCombatMenu;

	public void Init(Unit unit, BCombatMenu bCombatMenu) {
		this.unit = unit;
		this.bCombatMenu = bCombatMenu;
	}

	public void PopupCombatMenu() 
	{
		bCombatMenu.OpenForBUnit(this);
	}
}
