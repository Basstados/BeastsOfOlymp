using UnityEngine;
using System.Collections;

public class BTopping : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public void DestroyTopping()
    {
        // remove yourself from the game
        Destroy(this.gameObject);
    }
}
