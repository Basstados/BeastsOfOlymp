using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public struct Attack {

	public string name;
	public int damage;
	public int range;

	public Attack( string attackName ) {
		name = attackName;
		damage = 0;
		range = 0;

		// get Attack information from xml file
		TextAsset textAsset = (TextAsset)Resources.Load("attacks", typeof(TextAsset));
		XmlDocument xmldoc = new XmlDocument ();
		xmldoc.LoadXml ( textAsset.text );
		// get node containing the attacks
		XmlNode rootNode = xmldoc.LastChild;
		// find node where name == attackName
		foreach(XmlNode node in rootNode) {
			if( node.InnerText == attackName ) {
				// node found
				// read values for Attack instance
				damage = XmlConvert.ToInt32(node.Attributes.GetNamedItem("damage").Value );	
				range = XmlConvert.ToInt32(node.Attributes.GetNamedItem("range").Value );
			} else {
				Debug.LogError("Attack not found! AttackName: " + attackName);
			}
		}
	}
}
