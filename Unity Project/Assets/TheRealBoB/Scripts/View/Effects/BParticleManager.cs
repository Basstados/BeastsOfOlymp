using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BParticleManager : MonoBehaviour {
	
	public GameObject[] particles;
	Dictionary<string, GameObject> particleSystems = new Dictionary<string, GameObject>();

	#region singelton
	static BParticleManager instance;
	void Awake() 
	{
		foreach (GameObject go in particles) {
			particleSystems.Add(go.name, go);
		}
		instance = this;
	}
	#endregion

	public static void PlayEffect(string particleName, Vector3 pos)
	{
		instance.particleSystems[particleName].transform.position = pos;
		instance.particleSystems[particleName].SetActive(true);
	}
}
