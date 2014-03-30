using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BUnitAnimator))]
public class BUnit : MonoBehaviour {

	public enum Action{
		MOVE,
		CONFIRMMOVE,
		ATTACK,
		CONFIRMATTACK,
		IDLE
	}

	public BUnitUI unitUI;

	BView context;
	BUnitAnimator bUnitAnimator;
	Action action;
	public Action CurrentAction {get{return action;}}

	public Unit unit;
	public BCombatMenu bCombatMenu;

	Attack defaultAttack;
	Attack selectedAttack;

	BMapTile target;

	public void Init(BView context, BCombatMenu bCombatMenu) {
		this.context = context;
		this.bCombatMenu = bCombatMenu;
		this.bUnitAnimator = GetComponent<BUnitAnimator>();

		defaultAttack = unit.AttacksArray[unit.defaultAttackIndex];

		unitUI.Init(this);
		bUnitAnimator.Init(unit, this);
	}

	public void Init(BView context, Unit unit, BCombatMenu bCombatMenu) {
		this.unit = unit;
		Init(context, bCombatMenu);
	}

	public void Activate()
	{

		//PopupCombatMenu();
		ClearDisplayRange();
		if(unit.team == Unit.Team.PLAYER)
			DisplayMovementRange();
	}

	public void PopupCombatMenu() 
	{
		bCombatMenu.OpenForBUnit(this);
	}

	public void DisplayMovementRange()
	{
		action = Action.MOVE;
		context.DisplayRange(this, unit.MovePoints, DisplayRangeMode.ALL_CLICKABLE);
	}

	public void SelectMovementTarget(BMapTile bMapTile)
	{
		// save selected target
		target = bMapTile;
		
		ClearDisplayRange();
		DisplayMovementRange();
		// display calculated path
		context.HighlightMovementPath(this, bMapTile);
		context.SetFieldMarker(bMapTile);
		
		action = Action.CONFIRMMOVE;
	}

	public void SelectAttackTarget(BMapTile bMapTile)
	{
		// save selected target
		target = bMapTile;
		
		ClearDisplayRange();
		DisplayAttackRange(selectedAttack);
		context.DisplayArea(bMapTile,RotateArea(selectedAttack.area));
		context.SetFieldMarker(bMapTile);
		
		action = Action.CONFIRMATTACK;
	}
	
	private Vector[] RotateArea(Vector[] area)
	{
		Vector[] rotArea = new Vector[area.Length];
		
		Vector rotDir = new Vector(target.mapTile.x - unit.mapTile.x, target.mapTile.y - unit.mapTile.y);
		if (rotDir == Vector.zero) {
			for (int i = 0; i < area.Length; i++) {
				rotArea[i] = area[i].Clone();
			}
			return rotArea;
		}
		rotDir.NormalizeTo4Direction();
		int[,] rot = Vector.RotateToMatrix(rotDir);
		for (int i = 0; i < area.Length; i++) {
			rotArea[i] = area[i].Clone();
			rotArea[i].ApplyMatrix(rot);
		}
		return rotArea;
	}
	
	

	public void DisplayAttackRange(Attack attack)
	{
		if (attack == null) {
			selectedAttack = defaultAttack;
		} else {
			selectedAttack = attack;
		}
		action = Action.ATTACK;
		
		context.DisplayRange(this, selectedAttack.range, DisplayRangeMode.ALL_CLICKABLE);
	}

	public void ClearDisplayRange ()
	{
		action = Action.IDLE;
		// reset map marker
		context.CleanMap();
	}

	public void SetMoveTarget(BMapTile bMapTile)
	{
		Debug.Log("SetMoveTarget action="+action);
		switch (action) {
		case Action.MOVE:
			SelectMovementTarget(bMapTile);
			break;
		case Action.CONFIRMMOVE:
			if (bMapTile == target) {
				// use target for move
				context.controller.MoveUnit (unit, bMapTile.mapTile);
				action = Action.IDLE;
			} else {
				SelectMovementTarget (bMapTile);
			}
			break;
		}
	}

	public void SetAttackTarget(BMapTile bMapTile)
	{
		Debug.Log("SetAttackTarget action="+action);
		switch(action) {
		case Action.ATTACK:
			SelectAttackTarget(bMapTile);
			break;
		case Action.CONFIRMATTACK:
			if (bMapTile == target) {
				context.controller.AttackMapTile(this.unit, bMapTile.mapTile, selectedAttack);
				action = Action.IDLE;
			} else {
				SelectAttackTarget(bMapTile);
			}
			break;
		}
	}

	public void EndTurn()
	{
		context.EndTurn();
	}

	public void MoveAlongPath(BMapTile[] path)
	{
		bCombatMenu.ActionCompleted();
		bCombatMenu.Hide();
		StartCoroutine(bUnitAnimator.MoveRoutine(path, bCombatMenu));
	}

	public void PlayAttack(UnitAttackedEvent e, BMapTile target, BUnit[] victims)
	{
		StartCoroutine(bUnitAnimator.AttackRoutine(e, target, victims, bCombatMenu));
	}

	/// <summary>
	/// Plaies the hit animation.
	/// </summary>
	/// <param name="efficeny">0 = not effectiv, 1 = normal efficeny, 2 = very effectiv</param>
	public void PlayHitAnimation(byte efficeny, int damage)
	{
		if(damage > 0) {
			unitUI.UpdateLivebar();
			StartCoroutine(bUnitAnimator.DamageFlashRoutine());
			StartCoroutine(bUnitAnimator.ShakeRoutine(0.25f * efficeny,0.1f * efficeny));
		} else {
			EventProxyManager.FireEvent(this, new EventDoneEvent());
		}
	}

	public void Died()
	{
		StartCoroutine(bUnitAnimator.DeathRoutine(unitUI));
	}


}
