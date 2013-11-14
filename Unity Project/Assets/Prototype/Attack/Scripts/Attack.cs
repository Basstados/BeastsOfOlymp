using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

public class Attack {
	
	int damage;
	int range;
	
	GameObject source;
	
	public Attack( int dmg, int rng, GameObject src ) {
		damage = dmg;
		range = rng;
		source = src;
	}
	
	public Attack( string attackName, GameObject src ) {
		// get Attack information from xml file
		TextAsset textAsset = (TextAsset)Resources.Load("attacks", typeof(TextAsset));
		XmlDocument xmldoc = new XmlDocument ();
		xmldoc.LoadXml ( textAsset.text );
		
		Debug.Log( xmldoc.Name + " " + xmldoc.LastChild.Name );
		
		XmlNode rootNode = xmldoc.LastChild;
		foreach(XmlNode node in rootNode) {
			Debug.Log("AttackeName: " + node.InnerText);
			if( node.InnerText == attackName ) {
				damage = XmlConvert.ToInt32(node.Attributes.GetNamedItem("damage").Value );	
				range = XmlConvert.ToInt32(node.Attributes.GetNamedItem("range").Value );
				Debug.Log("Damage: " + damage + " Range: " + range);
			}
		}
		
		source = src;
	}
	
	public bool IsInRange( int[] target ) {
		int[] pos = new int[]{(int) source.transform.position.x, (int)source.transform.position.z};
		//range.UpdateCalculations( pos );
		//return range.IsInRange( target );
		return true;
	}
	
	public void Execute( Monster target ) {
		target.SendMessage("TakeDamage", damage);
	}

	public int Range {
		get {
			return this.range;
		}
	}
}

