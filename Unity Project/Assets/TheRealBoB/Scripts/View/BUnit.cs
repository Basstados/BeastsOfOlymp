using UnityEngine;
using System.Collections;

public class BUnit : MonoBehaviour {

	BView context;
	Action action;

	enum Action{
		MOVE,
		ATTACK,
		IDLE
	}


	public Unit unit;
	public BCombatMenu bCombatMenu;

	public void Init(BView context, Unit unit, BCombatMenu bCombatMenu) {
		this.context = context;
		this.unit = unit;
		this.bCombatMenu = bCombatMenu;
	}

	public void PopupCombatMenu() 
	{
		bCombatMenu.OpenForBUnit(this);
	}

	public void DisplayMovementRange()
	{
		action = Action.MOVE;
		context.DisplayRange(this, unit.MovementRange);
	}

	public void ClearDisplayRange ()
	{
		action = Action.IDLE;
		context.CleanMap();
	}

	public void SetTarget(BMapTile bMapTile)
	{
		switch(action) {
		case Action.MOVE:
			// use target for move
			context.controller.MoveUnit(unit, bMapTile.mapTile);
			break;
		case Action.ATTACK:
			// use target for attack
			break;
		}

		action = Action.IDLE;
	}

	public void MoveAlongPath(BMapTile[] path)
	{
		//TODO implement fancy animated movement
		transform.position = path[path.Length-1].transform.position;

		if (unit.canMove || unit.canAttack)
			PopupCombatMenu ();
	}

}
