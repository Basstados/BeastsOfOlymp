using UnityEngine;
using System.Collections;

public class BUnit : MonoBehaviour {

	public UILabel label;
	public float movementSpeed = 2;

	BView context;
	Action action;
	public Action CurrentAction {get{return action;}}

	public enum Action{
		MOVE,
		ATTACK,
		IDLE
	}

	public Unit unit;
	public BCombatMenu bCombatMenu;

	Attack defaultAttack;
	Color flashColor;
	Color defaultColor;

	public void Init(BView context, Unit unit, BCombatMenu bCombatMenu) {
		this.context = context;
		this.unit = unit;
		this.bCombatMenu = bCombatMenu;

		defaultAttack = unit.attacks[unit.defaultAttack];

		if(unit.team == Unit.Team.PLAYER) {
			renderer.material.color = Color.blue;
		} else {
			renderer.material.color = Color.gray;
		}

		defaultColor = renderer.material.color;
		flashColor = Color.red;

	}

	public void PopupCombatMenu() 
	{
		bCombatMenu.OpenForBUnit(this);
	}

	public void DisplayMovementRange()
	{
		action = Action.MOVE;
		context.DisplayRange(this, unit.ActionPoints);
	}

	public void DisplayAttackRange()
	{
		action = Action.ATTACK;
		Debug.Log(defaultAttack.name + " " + defaultAttack.range);
		context.DisplayRange(this, defaultAttack.range);
	}

	public void ClearDisplayRange ()
	{
		action = Action.IDLE;
		context.CleanMap();
	}

	public void SetTarget(BMapTile bMapTile)
	{
		Debug.Log("SetTarget for " + action);

		switch(action) {
		case Action.MOVE:
			// use target for move
			context.controller.MoveUnit(unit, bMapTile.mapTile);
			break;
		case Action.ATTACK:
			// use target for attack
			if(bMapTile.mapTile.unit != null) {
				context.controller.AttackUnit(this.unit, bMapTile.mapTile.unit, defaultAttack);
			}
			break;
		}

		action = Action.IDLE;
	}

	public void EndTurn()
	{
		context.EndTurn();
	}

	public void MoveAlongPath(BMapTile[] path)
	{
		bCombatMenu.ActionCompleted();
		StartCoroutine(MoveRoutine(path));
	}

	public void PlayAttack (Attack attack, bool hit)
	{
		bCombatMenu.ActionCompleted();
	}

	public void PlayHitAnimation (bool hit)
	{
		if(hit) {
			StartCoroutine(DamageFlashRoutine());
		} else {
			EventProxyManager.FireEvent(this, new EventDoneEvent());
		}
	}

	public void Died()
	{
		renderer.enabled = false;
		label.enabled = false; 
	}

	private IEnumerator DamageFlashRoutine() 
	{
		renderer.material.color = flashColor;
		yield return new WaitForSeconds(0.5f);
		renderer.material.color = defaultColor;
		EventProxyManager.FireEvent(this, new EventDoneEvent());
	}

	private IEnumerator MoveRoutine(BMapTile[] path)
	{
		for (int i = 1; i < path.Length; i++) {
			Vector3 nextWp = path[i].transform.position;
			do{
				Vector3 translation = nextWp - transform.position;
				float distance = translation.magnitude;
				translation = translation.normalized * Time.deltaTime * movementSpeed;
				if(distance < translation.magnitude) {
					transform.position = nextWp;
					break;
				} else {
					transform.Translate(translation);
				}
				yield return 0;
			} while(transform.position != nextWp);
		}
		EventProxyManager.FireEvent(this, new EventDoneEvent());
	}

	void Update() {
		label.text = unit.Name + " HP: " + unit.HealthPoints + "/" + unit.MaxHealthPoints + " AP: " + unit.ActionPoints + "/" + unit.MaxActionPoints;
	}
}
