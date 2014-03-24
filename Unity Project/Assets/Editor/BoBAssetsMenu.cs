using UnityEditor;
using UnityEngine;
using System.Collections;

public class BoBAssetsMenu {

	[MenuItem("BoB-Assets/Create/Attack")]
	static void CreateAttack()
	{
		ScriptableObject asset = ScriptableObject.CreateInstance(typeof(Attack));
		AssetDatabase.CreateAsset(asset, "Assets/Database/Attacks/" + ((Attack) asset).attackName +".asset");
		EditorUtility.FocusProjectWindow();
		Selection.activeObject = asset;
	}

	[MenuItem("BoB-Assets/Create/Element")]
	static void CreateType()
	{
		ScriptableObject asset = ScriptableObject.CreateInstance(typeof(Element));
		AssetDatabase.CreateAsset(asset, "Assets/Database/Elements/" + ((Element) asset).elementName +".asset");
		EditorUtility.FocusProjectWindow();
		Selection.activeObject = asset;
	}

	[MenuItem("BoB-Assets/Create/Unit")]
	static void CreateUnit()
	{
		ScriptableObject asset = ScriptableObject.CreateInstance(typeof(UnitData));
		AssetDatabase.CreateAsset(asset, "Assets/Database/Units/" + ((UnitData) asset).unitName + ".asset");
		EditorUtility.FocusProjectWindow();
		Selection.activeObject = asset;
	}

}
