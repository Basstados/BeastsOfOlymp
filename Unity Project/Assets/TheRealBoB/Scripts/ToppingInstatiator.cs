using UnityEngine;
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
                bMapTile.UpdateTopping();
                Debug.Log(prefabPath + bMapTile.mapTile.topping.prefabName);
				bMapTile.SpawnTopping();
			}

			lastTopping = bMapTile.topping;
		}

	}
}
