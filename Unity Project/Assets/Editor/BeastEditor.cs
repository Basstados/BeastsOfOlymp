using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Xml;

[CustomEditor(typeof(Beast))]
public class BeastEditor : Editor {

	Beast beast;
	string[] beastOptions;
	int beastChoice = 0;

	void Awake() {
		// Get target
		beast = (Beast) target;

		GetBeastOptionsFromXML();
	}

	public override void OnInspectorGUI() {
		// Draw default instpector as usually for now
		beastChoice = EditorGUILayout.Popup("Beast Type",  beastChoice, beastOptions );
	}

	private void GetBeastOptionsFromXML() {
		// get viable beasts from xml file
		TextAsset textAsset = (TextAsset)Resources.Load("BeastList", typeof(TextAsset));
		XmlDocument xmldoc = new XmlDocument ();
		xmldoc.LoadXml ( textAsset.text );
		// get node containing the attacks
		XmlNode rootNode = xmldoc.LastChild;

		// read out all beasts names
		beastOptions = new string[rootNode.ChildNodes.Count];
		int i=0;
		foreach(XmlNode node in rootNode) {
			beastOptions[i] = node["name"].InnerText;
			i++;
		}
	}
}

