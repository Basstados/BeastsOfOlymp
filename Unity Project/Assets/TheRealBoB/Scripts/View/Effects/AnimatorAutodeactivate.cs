using UnityEngine;
using System.Collections;

public class AnimatorAutodeactivate : MonoBehaviour {

	public float duration;

	private float startTime;

	public void OnEnable()
	{
		startTime = Time.time;
	}

	public void Update()
	{
		if(Time.time > startTime + duration)
			this.gameObject.SetActive(false);
	}
}
