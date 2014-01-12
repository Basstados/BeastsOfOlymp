using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class ShurikenAutodeactivate : MonoBehaviour {

	void Update() 
	{
		if (!particleSystem.IsAlive())
			this.gameObject.SetActive (false);
	}
}
