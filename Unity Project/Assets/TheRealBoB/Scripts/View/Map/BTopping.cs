using UnityEngine;
using System.Collections;

public class BTopping : MonoBehaviour {

	//public GameObject SoundOilVase;
	public AudioClip destroySound;
	//public ParticleSystem myparticle;
	public GameObject particleTest;
	//public GameObject ownPosition;

	//public objectPos = object.transform.position;
	// Use this for initialization
	void Start () {
		//myparticle = GetComponent<ParticleSystem>();
		//particleTest = GetComponent<ParticleSystem>();


	}

    public void DestroyTopping()
    {
		audio.PlayOneShot(destroySound, 1F);

		if(particleTest != null){
		particleTest.particleSystem.transform.parent=null;
		particleTest.particleSystem.Play();
		Destroy(particleTest.particleSystem.gameObject, 0.5F);
		}
		


		/*
		Vector3 position2 = new Vector3 (ownPosition.position);


		Vector3 positionOwn = ownPosition.transform.position;
		gameObject.particleSystem.Play();

		particleSystem.transform.parent=null; // detach particle system

		particleTest.transform.position = positionOwn;
		particleTest.particleSystem.Play();

		Destroy(particleSystem.gameObject, 3);
        remove yourself from the game*/

		Destroy(this.gameObject, audio.clip.length);

    }
}
