using UnityEngine;
using System.Collections;

public class BTopping : MonoBehaviour {
	
	public AudioClip destroySound;
	//public ParticleSystem myparticle;
	public GameObject particleTest;

	// Use this for initialization
	void Start () {
	
	}

    public void DestroyTopping()
    {
		audio.PlayOneShot(destroySound, 1F);

		if(particleTest != null){
		particleTest.particleSystem.transform.parent=null;
		particleTest.particleSystem.Play();
		Destroy(particleTest.particleSystem.gameObject, 0.5F);
		}

		/*remove yourself from the game*/
		Destroy(this.gameObject, audio.clip.length);

    }
}
