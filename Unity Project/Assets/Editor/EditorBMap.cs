using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(BMap))]
public class EditorBMap : Editor {

	 public override void OnInspectorGUI ()
	{
		BMap myTarget = (BMap) target;

		DrawDefaultInspector();
		if(GUILayout.Button("Inistantiate Map"))
			myTarget.InstantiateMap();
		if(GUILayout.Button("Spawn obstacles"))
			myTarget.SpawnObstacles();
		if(GUILayout.Button("Update map"))
			myTarget.UpdateMap();
	}
}
