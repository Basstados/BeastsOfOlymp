using UnityEngine;
using System.Collections;

public class ShurikenAutodeactivate : MonoBehaviour {

	public ParticleSystem listenParticleSystem;

	void Update() 
	{
		if (!listenParticleSystem.IsAlive())
			this.gameObject.SetActive (false);
	}
}
