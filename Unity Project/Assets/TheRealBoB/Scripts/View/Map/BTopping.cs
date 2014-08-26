using UnityEngine;
using System.Collections;

public class BTopping : MonoBehaviour {

	//public GameObject SoundOilVase;
	public AudioClip destroySound;
	// Use this for initialization
	void Start () {

	}

    public void DestroyTopping()
    {
		audio.PlayOneShot(destroySound, 1F);
        // remove yourself from the game
		Destroy(this.gameObject, audio.clip.length);

    }
}
