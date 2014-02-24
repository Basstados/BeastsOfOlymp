using UnityEngine;
using System.Collections.Generic;

public class FireworkLauncher : MonoBehaviour {

	public List<GameObject> fireworks;
	public Vector3 minArea;
	public Vector3 maxArea;
	public float minDelay;
	public float maxDelay;

	float nextLaunchTime;
	float timeSinceLaunch = 0;

	void Start()
	{
		nextLaunchTime = Random.Range(minDelay, maxDelay);
	}
	
	// Update is called once per frame
	void Update () 
	{
		timeSinceLaunch += Time.deltaTime;
		if(timeSinceLaunch > nextLaunchTime) {
			timeSinceLaunch = 0;
			nextLaunchTime = Random.Range(minDelay, maxDelay);
			LaunchFirework();
		}
	}

	private void LaunchFirework()
	{
		// get random rocket
		GameObject rocket = fireworks[Mathf.FloorToInt(Random.Range(0,fireworks.Count * 1000)/1000f)];
		// get random start position
		float x = Mathf.Lerp(minArea.x, maxArea.x, Random.Range(0,1000)/1000f);
		float y = Mathf.Lerp(minArea.y, maxArea.y, Random.Range(0,1000)/1000f);
		float z = Mathf.Lerp(minArea.z, maxArea.z, Random.Range(0,1000)/1000f);
		// launch rocket
		Instantiate(rocket, new Vector3(x,y,z), rocket.transform.rotation);
	}
}
