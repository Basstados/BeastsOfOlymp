using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(BUnit))]
public class UnitGridPlacer : MonoBehaviour {

	private Vector3 position;
	private Quaternion rotation;
	private Vector3 scale;
	
	/// <summary>
	/// On awake this instance, destroy it if we are in playmode.
	/// </summary>
	void Awake()
	{
		if (Application.isPlaying)
			Destroy(this);
		
		position = this.transform.localPosition;
		rotation = this.transform.localRotation;
		scale = this.transform.localScale;
	}
	
	/// <summary>
	/// Check every frame if gameobject has moved; if so snap it to the grid.
	/// </summary>
	void Update () {
		// Only execute in editor
		if (!Application.isEditor)
		{
			Destroy(this);
			return;
		}
		
		if(this.transform.position != position) {
			position = new Vector3(Mathf.Floor(this.transform.localPosition.x),
			                       0,
			                       Mathf.Floor(this.transform.localPosition.z));
			this.transform.localPosition = position;
		}
		if(this.transform.localRotation != rotation)
			//rotation = Quaternion.Euler(0, this.transform.localRotation.eulerAngles.y, 0);
			this.transform.localRotation = rotation;
		if(this.transform.localScale != scale)
			this.transform.localScale = scale;
	}
}
