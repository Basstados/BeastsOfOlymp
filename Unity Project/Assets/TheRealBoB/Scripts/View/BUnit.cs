using UnityEngine;
using System.Collections;

public class BUnit : MonoBehaviour {

	public GameObject renderObject;
	public GameObject meshContainer;
	public Animator animator;
	public BUnitUI unitUI;
	public float movementSpeed = 4;

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
			renderObject.renderer.material.color = Color.blue;
		} else {
			renderObject.renderer.material.color = Color.gray;
		}

		defaultColor = renderObject.renderer.material.color;
		flashColor = Color.red;

		unitUI.Init(this);
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

	public void PlayAttack(BUnit target, Attack attack, bool hit)
	{
		meshContainer.transform.LookAt(target.transform.position);
		StartCoroutine(AttackRoutine(target,attack, hit));

	}

	IEnumerator AttackRoutine(BUnit target, Attack attack, bool hit)
	{
		animator.SetBool("attack",true);
		yield return new WaitForSeconds(0.3f); //TODO make this controlable
		animator.SetBool("attack",false);
		target.PlayHitAnimation(hit);
		bCombatMenu.ActionCompleted();
	}

	public void PlayHitAnimation(bool hit)
	{
		if(hit) {
			unitUI.UpdateLivebar();
			StartCoroutine(DamageFlashRoutine());
		} else {
			EventProxyManager.FireEvent(this, new EventDoneEvent());
		}
	}

	public void Died()
	{
		renderObject.renderer.enabled = false;
		unitUI.gameObject.SetActive(false);
	}

	private IEnumerator DamageFlashRoutine() 
	{
		renderObject.renderer.material.color = flashColor;
		yield return new WaitForSeconds(0.5f);
		renderObject.renderer.material.color = defaultColor;
		EventProxyManager.FireEvent(this, new EventDoneEvent());
	}

	/// <summary>
	/// This routine does the movement animation.
	/// </summary>
	/// <returns>Nothing; IEnumerator is just for coroutines</returns>
	/// <param name="path">The pathe we want to move along</param>
	private IEnumerator MoveRoutine(BMapTile[] path)
	{
		for (int i = 1; i < path.Length; i++) {
			if(animator == null) {
				// old moveanimation without animator
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
			} else {
				animator.SetBool("walking", true);
				Vector3 nextWp = path[i].transform.position;
				Vector3 lookPoint = nextWp;
				lookPoint.y = 0;
				meshContainer.transform.LookAt(lookPoint);
				do {
//					Debug.Log("Pos: " + transform.position + " WP: " + nextWp);
					Vector3 translation = nextWp - transform.position;
					float distance = translation.magnitude;
					translation = translation.normalized * Time.deltaTime * movementSpeed;
//					Debug.Log("Distance: " + distance + " Translation: " + translation.magnitude);
					if(distance < translation.magnitude) {
						transform.position = nextWp;
						break;
					} else {
						transform.Translate( transform.InverseTransformDirection(translation) );
					}
					yield return 0;
				} while(transform.position != nextWp);
			}
		}
		animator.SetBool("walking", false);
		EventProxyManager.FireEvent(this, new EventDoneEvent());
	}
}
