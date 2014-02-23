using UnityEngine;
using System.Collections;

public class BUnitAnimator : MonoBehaviour {

	public Animator animator;
	public GameObject renderObject;
	public GameObject meshContainer;
	public float movementSpeed = 4;
	public AudioSource deathSound;
	public AudioSource attackSound;
	public AudioSource selectionSound;

	Color flashColor;
	Color defaultColor;

	BUnit parent;

	public void Init(Unit unit, BUnit bUnit)
	{
		this.parent = bUnit;

		if(unit.team == Unit.Team.PLAYER) {
			renderObject.renderer.material.color = new Color(0.25490f, 0.85882f, 0.23529f);
			meshContainer.transform.rotation = Quaternion.AngleAxis(90f,Vector3.up);
		} else {
			renderObject.renderer.material.color = new Color(0.77255f, 0.21961f, 0.21961f);
			meshContainer.transform.rotation = Quaternion.AngleAxis(-90f,Vector3.up);
		}
		
		defaultColor = renderObject.renderer.material.color;
		flashColor = Color.red;
	}

	public void Activate()
	{
		selectionSound.Play();
	}

	public IEnumerator DeathRoutine(BUnitUI unitUI)
	{
		yield return new WaitForSeconds(1f);
		deathSound.Play();
		animator.SetTrigger("DeathTrigger");
		yield return new WaitForSeconds(2f);
		renderObject.SetActive(false);
		unitUI.gameObject.SetActive(false);
		EventProxyManager.FireEvent(this, new EventDoneEvent());
	}
	
	public IEnumerator DamageFlashRoutine() 
	{
		BParticleManager.PlayEffect("HitEffect", this.transform.position);
		animator.SetTrigger("DamagedTrigger");
		//		renderObject.renderer.material.color = flashColor;
		yield return new WaitForSeconds(0.5f);
		//		renderObject.renderer.material.color = defaultColor;
		
	}
	/// <summary>
	/// Routine to performe a camera shake effect.
	/// </summary>
	/// <returns>IEnumerator is needed for co-routines.</returns>
	/// <param name="magnitude">The maginitude of the shake.</param>
	/// <param name="duration">The duration of the shake.</param>
	public IEnumerator ShakeRoutine(float magnitude, float duration) {
		
		float elapsed = 0.0f;
		Vector3 originalCamPos = Camera.main.transform.position;
		
		while (elapsed < duration) {
			
			elapsed += Time.deltaTime;          
			// damper is used to reduce shake over time lineary
			float percentComplete = elapsed / duration;         
			float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);
			
			// map value to [-1, 1]
			float x = Random.value * 2.0f - 1.0f;
			float y = Random.value * 2.0f - 1.0f;
			x *= magnitude * damper;
			y *= magnitude * damper;
			
			// perfomce the actual shake
			Camera.main.transform.position = new Vector3(originalCamPos.x + x, originalCamPos.y + y, originalCamPos.z);
			
			yield return null;
		}
		
		Camera.main.transform.position = originalCamPos;
	}

	/// <summary>
	/// Routine to perfome a attack animation
	/// </summary>
	/// <returns>IEnumerator is needed for co-routines.<</returns>
	/// <param name="target">The BUnit whom is the attack target.</param>
	/// <param name="attack">The attack which will be performed.</param>
	/// <param name="efficeny">0 = not effectiv, 1 = normal efficeny, 2 = very effectiv</param>
	/// <param name="damage">The amount of damage dealt by this attack.</param>
	public IEnumerator AttackRoutine(UnitAttackedEvent e, BMapTile target, BUnit[] victims, BCombatMenu bCombatMenu)
	{
		meshContainer.transform.LookAt(target.transform.position);
		bCombatMenu.Hide();
		// sound effect
		attackSound.Play();
		// animation
		animator.SetTrigger("AttackTrigger");
		yield return new WaitForSeconds(0.4f);
		Vector direction = new Vector(Mathf.FloorToInt(target.transform.position.x - this.transform.position.x),
		                              Mathf.FloorToInt(target.transform.position.y - this.transform.position.y));
		direction.NormalizeTo4Direction();
		BParticleManager.PlayEffect(e.attack.name, target.transform.position, new Vector3(direction.x, direction.y));
		yield return new WaitForSeconds(0.2f);
		foreach(BUnit bUnit in victims) {
			bUnit.PlayHitAnimation(e.efficiency, e.damage);
			bUnit.unitUI.ShowDamage(e.damage);
		}
		yield return new WaitForSeconds(0.6f);
		EventProxyManager.FireEvent(this, new EventDoneEvent());
	}
	
	/// <summary>
	/// This routine does the movement animation.
	/// </summary>
	/// <returns>Nothing; IEnumerator is just for coroutines</returns>
	/// <param name="path">The path we want to move along</param>
	/// <param name="bCombatMenu">This combat Menu will be hide during animation</param>
	public IEnumerator MoveRoutine(BMapTile[] path, BCombatMenu bCombatMenu)
	{
		bCombatMenu.Hide();
		for (int i = 1; i < path.Length; i++) {
			Vector3 nextWp = path[i].transform.position;
			Vector3 lookPoint = nextWp;
			lookPoint.y = 0;
			meshContainer.transform.LookAt(lookPoint);
			do {
				Vector3 translation = nextWp - transform.position;
				float distance = translation.magnitude;
				translation = translation.normalized * Time.deltaTime * movementSpeed;
				if(distance < translation.magnitude) {
					transform.position = nextWp;
					break;
				} else {
					transform.Translate( transform.InverseTransformDirection(translation) );
				}
				yield return 0;
			} while(transform.position != nextWp);
		}
		bCombatMenu.OpenForBUnit(parent);
		EventProxyManager.FireEvent(this, new EventDoneEvent());
	}
}
