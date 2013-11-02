using UnityEngine;
using System.Collections;

public class DemoEnemy : MonoBehaviour {
	
	public int health = 10;
	
	Color flashColor = Color.red;
	Color defaultColor;
	
	// Use this for initialization
	void Start () {
		defaultColor = renderer.material.color;
	}
	
	// Update is called once per frame
	void Update () {
//		if( Input.GetButtonDown("Fire1") && Alive ) {
//			
//			Ray mouseRay = Camera.main.ScreenPointToRay( Input.mousePosition );
//			RaycastHit hit = new RaycastHit();
//			Physics.Raycast(mouseRay, out hit);
//			
//			if( hit.collider.GetInstanceID() == collider.GetInstanceID() ) {
//				Debug.Log("Arrg! You hit me!");
//				TakeDamage(1);
//			}
//		}
	}
	
	public void TakeDamage(int dmg) {
		health -= dmg;
		
		StartCoroutine(DamageFlash());
	}
	
	private IEnumerator DamageFlash() {
		renderer.material.color = flashColor;
		yield return new WaitForSeconds(0.5f);
		renderer.material.color = defaultColor;
		
		if( health <= 0 ) {
			// enemy died
			// do something...
			renderer.enabled = false;
		}
	}

	public bool Alive {
		get {
			return health > 0;
		}
	}

	public int Health {
		get {
			return this.health;
		}
	}
}
