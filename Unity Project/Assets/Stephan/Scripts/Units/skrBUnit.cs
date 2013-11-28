using UnityEngine;
using System.Collections;

public class skrBUnit : MonoBehaviour {

	public skrUnit unit;

	public void MoveUnit (Vector3 startPosition, Vector3 endPosition) 
	{
		StartCoroutine(MoveRoutine (startPosition, endPosition));
	}

	IEnumerator MoveRoutine (Vector3 startPosition, Vector3 endPosition) 
	{
		transform.position = startPosition;

		while(transform.position != endPosition) {
			Vector3 translation = endPosition - startPosition;
			translation = translation.normalized * unit.speed * Time.deltaTime;
			if (Vector3.Distance(transform.position, endPosition) >= translation.magnitude)
				transform.Translate( translation );
			else
				transform.position = endPosition;

			yield return 0; // continue next frame
		}
	}
}
