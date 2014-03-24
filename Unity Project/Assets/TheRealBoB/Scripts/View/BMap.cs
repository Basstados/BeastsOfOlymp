using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BMap : MonoBehaviour {

	[SerializeField] GameObject bMapTilePrefab;
	[SerializeField] int gridSizeX = 10;
	[SerializeField] int gridSizeY = 10;
	[SerializeField] BMapTile[] bMapTiles;

	/// <summary>
	/// Direct access to the bMapTiles of this bMap.
	/// </summary>
	/// <param name="i">The x index.</param>
	/// <param name="j">The y index.</param>
	public BMapTile this[int i,int j]
	{
		get { return bMapTiles[i + j * gridSizeX]; }
		set { bMapTiles[i + j * gridSizeX] = value; }
	}

	public int lengthX { get{return gridSizeX; }}
	public int lengthY { get{return gridSizeY; }}

	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Remove old map and instantiate new mapTiles.
	/// </summary>
	public void InstantiateMap()
	{
		if(!Application.isEditor) return;
		// destroy old map
//		if(bMapTiles != null)
//			for (int i = 0; i < bMapTiles.Length; i++) {
//				for (int j = 0; j < bMapTiles[i].Length; j++) {
//				if(bMapTiles[i + j * gridSizeX].gameObject != null)
//						DestroyImmediate(bMapTiles[i][j].gameObject);
//				}
//			}
		List<GameObject> children = new List<GameObject>();
		for (int i = 0; i < transform.childCount; i++) {
			children.Add(transform.GetChild(i).gameObject);
		}
		foreach(GameObject go in children)
			DestroyImmediate(go);

		bMapTiles = new BMapTile[gridSizeX * gridSizeY];
		for (int i = 0; i < gridSizeX; i++) {
			for (int j = 0; j < gridSizeY; j++) {
				Vector3 pos = this.transform.TransformPoint(new Vector3(i, 0, j));
				GameObject go = Instantiate(bMapTilePrefab, pos, bMapTilePrefab.transform.rotation) as GameObject;
				// parenting
				go.transform.parent = this.transform;
				// proper nameing
				go.name = bMapTilePrefab.name + "[" + i + "," + j + "]";
				// set references
				bMapTiles[i + j * gridSizeX] = go.GetComponent<BMapTile>();
				bMapTiles[i + j * gridSizeX].mapTile = new MapTile(i,j);
			}
		}
	}
}
