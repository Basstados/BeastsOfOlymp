using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(BMap))]
public class EditorBMap : Editor {

	public override void OnInspectorGUI ()
	{
		BMap myTarget = (BMap) target;

		DrawDefaultInspector();
		if(GUILayout.Button("Instantiate Map")) {
			myTarget.InstantiateMap();
			Debug.Log("Instantiate map successful!");
		}
		if(GUILayout.Button("Spawn obstacles")) {
			myTarget.SpawnObstacles();
			Debug.Log("Spawning obstacles completed!");
		}
		if(GUILayout.Button("Update map")) {
			myTarget.UpdateMap();
			Debug.Log("Update map penalties completed!");
		}
	}
}
