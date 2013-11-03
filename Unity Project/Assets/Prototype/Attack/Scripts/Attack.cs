using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

public class Attack {
	
	int damage;
	Range range;
	
	GameObject source;
	
	public Attack( int dmg, Range rng, GameObject src ) {
		damage = dmg;
		range = rng;
		source = src;
	}
	
	public Attack( string attackName, BattlefieldQuad[,] battleMatrix, GameObject src ) {
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
				range = new Range(battleMatrix ,XmlConvert.ToInt32(node.Attributes.GetNamedItem("range").Value ));
				Debug.Log("Damage: " + damage + " Range: " + range.intValue);
			}
		}
		
		source = src;
	}
	
	public bool IsInRange( int[] target ) {
		int[] pos = new int[]{(int) source.transform.position.x, (int)source.transform.position.z};
		range.UpdateCalculations( pos );
		return range.IsInRange( target );
	}
	
	public void Execute( GameObject target ) {
		// check if is in range
		int[] targetPos = new int[]{(int) target.transform.position.x, (int) target.transform.position.z};
		if( range.IsInRange( targetPos ) ) {
			// target is in range! yeah! lets burn it!
			target.SendMessage("TakeDamage", damage);
		} else {
			Debug.Log("Out of range!");	
		}
	}

	public Range Range {
		get {
			return this.range;
		}
	}
}

