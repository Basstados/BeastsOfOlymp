using UnityEditor;
using UnityEngine;
using System.Collections;

public class BoBAssetsMenu {

	[MenuItem("BoB-Assets/Create/Attack")]
	static void CreateAttack()
	{
//		ScriptableObject asset = ScriptableObject.CreateInstance(typeof(Attack));
//		AssetDatabase.CreateAsset(asset, "Assets/Database/Attacks/" + ((Attack) asset).attackName +".asset");
//		EditorUtility.FocusProjectWindow();
//		Selection.activeObject = asset;
	}

	[MenuItem("BoB-Assets/Create/Type")]
	static void CreateType()
	{

	}

	[MenuItem("BoB-Assets/Create/Unit")]
	static void CreateUnit()
	{

	}

}
