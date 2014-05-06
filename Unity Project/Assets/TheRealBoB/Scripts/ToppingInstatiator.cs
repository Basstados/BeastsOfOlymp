using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ToppingInstatiator : MonoBehaviour {

	public string prefabPath;

	BMapTile bMapTile;
	BMapTile.ToppingType lastTopping = BMapTile.ToppingType.NONE;

	// Use this for initialization
	void Awake () {
		if (Application.isPlaying)
			Destroy(this);

		bMapTile = GetComponent<BMapTile>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!Application.isEditor)
		{
			Destroy(this);
			return;
		}

		// if topping has changed, instantiate new prefab
		if(lastTopping != bMapTile.topping) {
			// remove old children objects
			List<GameObject> children = new List<GameObject>();
			for (int i = 0; i < transform.childCount; i++) {
				children.Add(transform.GetChild(i).gameObject);
			}
			foreach(GameObject go in children)
				DestroyImmediate(go);

			// spawn prefab if there is any
			if(bMapTile.topping != BMapTile.ToppingType.NONE) {
				Debug.Log(prefabPath + bMapTile.topping.ToString());
				GameObject prefab = Resources.Load<GameObject>(prefabPath + bMapTile.topping.ToString()); // note: not .prefab!
				GameObject handle = (GameObject) Instantiate(prefab, Vector3.zero, prefab.transform.rotation);
				handle.transform.parent = this.transform;
				handle.transform.localPosition = Vector3.zero;
				
				
			}

			lastTopping = bMapTile.topping;
		}

	}
}
