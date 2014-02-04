using UnityEngine;
using System.Collections;

public class BUnit : MonoBehaviour {

	public GameObject renderObject;
	public GameObject meshContainer;
	public GameObject effectAnchor;
	public Animator animator;
	public BUnitUI unitUI;
	public float movementSpeed = 4;

	public AudioSource deathSound;
	public AudioSource attackSound;
	public AudioSource selectionSound;

	BView context;
	Action action;
	public Action CurrentAction {get{return action;}}

	public enum Action{
		MOVE,
		CONFIRMMOVE,
		ATTACK,
		CONFIRMATTACK,
		IDLE
	}

	public Unit unit;
	public BCombatMenu bCombatMenu;

	Attack defaultAttack;
	Attack selectedAttack;
	Color flashColor;
	Color defaultColor;

	BMapTile target;

	public void Init(BView context, Unit unit, BCombatMenu bCombatMenu) {
		this.context = context;
		this.unit = unit;
		this.bCombatMenu = bCombatMenu;

		defaultAttack = unit.attacks[unit.defaultAttack];

		if(unit.team == Unit.Team.PLAYER) {
			renderObject.renderer.material.color = new Color(0.25490f, 0.85882f, 0.23529f);
		} else {
			renderObject.renderer.material.color = new Color(0.77255f, 0.21961f, 0.21961f);;
		}

		defaultColor = renderObject.renderer.material.color;
		flashColor = Color.red;

		unitUI.Init(this);
	}

	public void Activate()
	{
		selectionSound.Play();
		PopupCombatMenu();
	}

	public void PopupCombatMenu() 
	{
		bCombatMenu.OpenForBUnit(this);

		//TODO!!!!! tmp position
		unitUI.UpdateAPBar();
	}

	public void DisplayMovementRange()
	{
		action = Action.MOVE;
		context.DisplayRange(this, unit.ActionPoints, DisplayRangeMode.ALL_CLICKABLE);
	}

	public void SelectMovementTarget(BMapTile bMapTile)
	{
		ClearDisplayRange();
		DisplayMovementRange();
		// display calculated path
		Path path = context.HighlightMovementPath(this, bMapTile);
		context.SetMovementMarker(bMapTile);

		unitUI.MarkAP(path.Cost);

		// save selected target
		target = bMapTile;
		action = Action.CONFIRMMOVE;
	}

	public void DisplayAttackRange(Attack attack)
	{
		if (attack == null) {
			selectedAttack = defaultAttack;
		} else {
			selectedAttack = attack;
		}
		unitUI.MarkAP(attack.apCost);
		action = Action.ATTACK;

		// set display mode depending on unit team
		// only units of the enemy team will be marked as clickable
		int mode = (unit.team == Unit.Team.PLAYER) ? DisplayRangeMode.TEAM_0_CLICKABLE : DisplayRangeMode.TEAM_1_CLICKABLE;

		context.DisplayRange(this, selectedAttack.range, mode);
	}

	public void ClearDisplayRange ()
	{
		action = Action.IDLE;
		// reset ap marker
		unitUI.MarkAP(0);
		// reset map marker
		context.CleanMap();
	}

	public void SetTarget(BMapTile bMapTile)
	{
		switch(action) {
		case Action.MOVE:
			SelectMovementTarget(bMapTile);
			break;
		case Action.CONFIRMMOVE:
			if(bMapTile == target) {
				// use target for move
				context.controller.MoveUnit(unit, bMapTile.mapTile);
				action = Action.IDLE;
			} else {
				SelectMovementTarget(bMapTile);
			}
			break;
		case Action.ATTACK:
			// use target for attack
			if(bMapTile.mapTile.unit != null) {
				context.controller.AttackUnit(this.unit, bMapTile.mapTile.unit, selectedAttack);
			}
			action = Action.IDLE;
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
		unitUI.UpdateAPBar();
		StartCoroutine(MoveRoutine(path));
	}

	public void PlayAttack(BUnit target, Attack attack, bool hit, int damage)
	{
		meshContainer.transform.LookAt(target.transform.position);
		unitUI.UpdateAPBar();
		StartCoroutine(AttackRoutine(target,attack, hit, damage));
	}

	IEnumerator AttackRoutine(BUnit target, Attack attack, bool hit, int damage)
	{
		// sound effect
		attackSound.Play();
		// animation
		animator.SetTrigger("AttackTrigger");
		yield return new WaitForSeconds(0.6f);
		target.PlayHitAnimation(hit);
		target.unitUI.ShowDamage(damage);
		bCombatMenu.ActionCompleted();
	}

	public void PlayHitAnimation(bool hit)
	{
		if(hit) {
			unitUI.UpdateLivebar();
			StartCoroutine(DamageFlashRoutine());
			StartCoroutine(ShakeRoutine(0.1f,0.1f));
		} else {
			EventProxyManager.FireEvent(this, new EventDoneEvent());
		}
	}

	IEnumerator ShakeRoutine(float magnitude, float duration) {
		
		float elapsed = 0.0f;
		
		Vector3 originalCamPos = Camera.main.transform.position;
		
		while (elapsed < duration) {
			
			elapsed += Time.deltaTime;          
			
			float percentComplete = elapsed / duration;         
			float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);
			
			// map value to [-1, 1]
			float x = Random.value * 2.0f - 1.0f;
			float y = Random.value * 2.0f - 1.0f;
			x *= magnitude * damper;
			y *= magnitude * damper;
			
			Camera.main.transform.position = new Vector3(originalCamPos.x + x, originalCamPos.y + y, originalCamPos.z);
			
			yield return null;
		}
		
		Camera.main.transform.position = originalCamPos;
	}


	public void Died()
	{
		StartCoroutine(DeathRoutine());
	}

	private IEnumerator DeathRoutine()
	{
		yield return new WaitForSeconds(1f);
		deathSound.Play();
		animator.SetTrigger("DeathTrigger");
		yield return new WaitForSeconds(2f);
		renderObject.renderer.enabled = false;
		unitUI.gameObject.SetActive(false);
	}

	private IEnumerator DamageFlashRoutine() 
	{
		BParticleManager.PlayEffect("HitEffect", effectAnchor.transform.position);
		animator.SetTrigger("DamagedTrigger");
//		renderObject.renderer.material.color = flashColor;
		yield return new WaitForSeconds(0.5f);
//		renderObject.renderer.material.color = defaultColor;
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
		EventProxyManager.FireEvent(this, new EventDoneEvent());
	}
}
