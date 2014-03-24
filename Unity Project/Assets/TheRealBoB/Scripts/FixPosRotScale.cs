using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class FixPosRotScale : MonoBehaviour {

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

		position = this.transform.position;
		rotation = this.transform.rotation;
		scale = this.transform.localScale;
	}

	/// <summary>
	/// Check every frame if gameobject has moved; if so move it back
	/// </summary>
	void Update () {
		// Only execute in editor
		if (!Application.isEditor)
		{
			Destroy(this);
			return;
		}

		if(this.transform.position != position)
			this.transform.position = position;
		if(this.transform.rotation != rotation)
			this.transform.rotation = rotation;
		if(this.transform.localScale != scale)
			this.transform.localScale = scale;
	}
}
