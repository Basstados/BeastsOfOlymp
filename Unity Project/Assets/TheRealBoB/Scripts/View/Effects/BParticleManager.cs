using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BParticleManager : MonoBehaviour {
	
	public GameObject[] particles;
	public int maxParticleOfOneKinde;
	Dictionary<string, Queue<GameObject>> particleSystems = new Dictionary<string, Queue<GameObject>>();

	#region singelton
	static BParticleManager instance;
	void Awake() 
	{
		foreach (GameObject go in particles) {
			Queue<GameObject> particleQueue = new Queue<GameObject>();
			particleQueue.Enqueue(go);
			// create copies of the particle game object and add them to an queue
			for (int i = 0; i < maxParticleOfOneKinde-1; i++) {
				GameObject handle = (GameObject) Instantiate(go);
				handle.transform.parent = this.transform;
				particleQueue.Enqueue(handle);
			}
			particleSystems.Add(go.name, particleQueue);
		}
		instance = this;
	}
	#endregion

	public static void PlayEffect(string particleName, Vector3 pos)
	{
		GameObject go = instance.particleSystems[particleName].Dequeue();
		// place and activate particle game object
		go.transform.position = pos;
		go.SetActive(true);
		instance.particleSystems[particleName].Enqueue(go);
	}
}
