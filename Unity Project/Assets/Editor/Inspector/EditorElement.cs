using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(Element))]
public class EditorElement : Editor {

	public override void OnInspectorGUI ()
	{
		GUIStyle xButton = new GUIStyle(GUI.skin.button);
		xButton.fixedWidth = 25f;

		Element myTarget = (Element) target;
		string oldName = myTarget.elementName; // save old name to rename asset later
		myTarget.elementName 	= EditorGUILayout.TextField("Name",myTarget.elementName);
		EditorGUILayout.Separator();

		// list all weaknesses and buttons to edit them
		EditorGUILayout.LabelField("Weaknesses");
		if(myTarget.weaknesses != null) {
			for (int i = 0; i < myTarget.weaknesses.Length; i++) {
				EditorGUILayout.BeginHorizontal();
				myTarget.weaknesses[i] = (Element) EditorGUILayout.ObjectField(myTarget.weaknesses[i], typeof(Element));
				if(GUILayout.Button("x", xButton))
					RemoveFromArray<Element>(ref myTarget.weaknesses, i);
				EditorGUILayout.EndHorizontal();
			}
		}
		if(GUILayout.Button("Add weakness"))
			IncreaseArraySize<Element>(ref myTarget.weaknesses);

		// list all strength an buttons to edit them
		EditorGUILayout.LabelField("Strength");
		if(myTarget.strengths != null) {
			for (int i = 0; i < myTarget.strengths.Length; i++) {
				EditorGUILayout.BeginHorizontal();
				myTarget.strengths[i] = (Element) EditorGUILayout.ObjectField(myTarget.strengths[i], typeof(Element));
				if(GUILayout.Button("x", xButton))
					RemoveFromArray<Element>(ref myTarget.strengths, i);
				EditorGUILayout.EndHorizontal();
			}
		}
		if(GUILayout.Button("Add strength"))
			IncreaseArraySize<Element>(ref myTarget.strengths);

		// you need this to save changes in the custom inspector permanent in the .asset file
		EditorUtility.SetDirty(target);

		// keep elementName and asset name equal
		AssetDatabase.RenameAsset("Assets/Database/Elements/" + oldName + ".asset", myTarget.elementName);
	}

	/// <summary>
	/// Increases the size of the array by adding an empty entry at the end.
	/// </summary>
	/// <param name="array">Array.</param>
	/// <typeparam name="T">The type of the array.</typeparam>
	private void IncreaseArraySize<T>(ref T[] array)
	{
		T[] newArray = new T[1];
		if(array != null) {
			newArray = new T[array.Length + 1];
			for (int i = 0; i < array.Length; i++) {
				newArray[i] = array[i];
			}
		} 
		array = newArray;
	}

	/// <summary>
	/// Removes entry with given index from array.
	/// </summary>
	/// <param name="array">Array.</param>
	/// <param name="index">Index of the element to remove.</param>
	/// <typeparam name="T">The type of the array.</typeparam>
	private void RemoveFromArray<T>(ref T[] array, int index)
	{
		List<T> list = new List<T>(array);
		list.RemoveAt(index);
		array = list.ToArray();
	}
}
