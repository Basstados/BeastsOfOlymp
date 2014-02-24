using UnityEngine;
using System.Collections;

public class SFX : MonoBehaviour {

	public AudioSource start;
	public AudioSource[] explosion;
	public AudioSource glitter;

	public float endTime;
	public float explosionDelay;
	public float glitterDelay;

	public bool sndExplosion = false;
	public float sndExploDelay;

	float startTime;
	bool explosionHappend = false;
	bool glitterHappend = false;
	bool sndExplosionHappend = false;

	// Use this for initialization
	void Start () {
		if(start != null)
			start.Play();

		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if((Time.time > startTime + explosionDelay) && !explosionHappend) {
			explosionHappend = true;
			explosion[Random.Range(0,explosion.Length)].Play();
		}

		if(sndExplosion && (Time.time > startTime + sndExploDelay) && !sndExplosionHappend) {
			sndExplosionHappend = true;
			explosion[Random.Range(0,explosion.Length)].Play();
		}

		if((Time.time > startTime + glitterDelay) && !glitterHappend) {
			glitterHappend = true;
			glitter.Play();
		}

		if(Time.time > endTime + startTime) {
			start.Stop();
			foreach(AudioSource source in explosion) {
				source.Stop();
			}
			glitter.Stop();
			this.enabled = false;
		}
	}
}
