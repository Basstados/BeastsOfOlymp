using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Attack {
	
	int damage;
	Range range;
	
	GameObject source;
	
	public Attack( int dmg, Range rng, GameObject src ) {
		damage = dmg;
		range = rng;
		source = src;
	}
	
	public bool IsInRange( int[] target ) {
		int[] pos = new int[]{(int) source.transform.position.x, (int)source.transform.position.z};
		range.UpdateCalculations( pos );
		return range.IsInRange( target );
	}
	
	public void Execute( GameObject target ) {
		// check if is in range
		int[] targetPos = new int[]{(int) target.transform.position.x, (int) target.transform.position.z};
		if( range.IsInRange( targetPos ) ) {
			// target is in range! yeah! lets burn it!
			target.SendMessage("TakeDamage", damage);
		} else {
			Debug.Log("Out of range!");	
		}
	}
}

