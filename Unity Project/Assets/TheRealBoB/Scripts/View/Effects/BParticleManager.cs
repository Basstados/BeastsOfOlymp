using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BParticleManager : MonoBehaviour {
	
	public GameObject[] particles;
	public int[] particleCloneCounts;

	Dictionary<string, Queue<GameObject>> particleSystems = new Dictionary<string, Queue<GameObject>>();

	#region singelton
	static BParticleManager instance;
	void Awake() 
	{
		for (int k = 0; k < particles.Length; k++) {
			Queue<GameObject> particleQueue = new Queue<GameObject>();
			particleQueue.Enqueue(particles[k]);
			// create copies of the particle game object and add them to an queue
			for (int i = 0; i < particleCloneCounts[k]-1; i++) {
				GameObject handle = (GameObject) Instantiate(particles[k]);
				handle.transform.parent = this.transform;
				particleQueue.Enqueue(handle);
			}
			particleSystems.Add(particles[k].name, particleQueue);
		}
		instance = this;
	}
	#endregion

	public static void PlayEffect(string particleName, Vector3 pos)
	{
		if(instance.particleSystems.ContainsKey(particleName)) {
			GameObject go = instance.particleSystems[particleName].Dequeue();
			// place and activate particle game object
			go.transform.position = pos;
			go.SetActive(true);
			instance.particleSystems[particleName].Enqueue(go);
		}
	}

	public static void DisbaleEffect(string particleName)
	{
		if(instance.particleSystems.ContainsKey(particleName))
			foreach(GameObject go in instance.particleSystems[particleName]) {
				go.SetActive(false);
			}
	}
}
