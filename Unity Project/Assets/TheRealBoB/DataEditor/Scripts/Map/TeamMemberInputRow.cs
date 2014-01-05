using UnityEngine;
using System.Collections;

public class TeamMemberInputRow : MonoBehaviour {

	public UIInput nameInput;
	public UIInput xInput;
	public UIInput yInput;

	char[] trimChar = new char[]{'|'};

	public string name {
		get {
			return nameInput.value;
		}
		set {
			nameInput.value = value;
		}
	}

	public int x {
		get {
			return int.Parse(xInput.value.TrimEnd(trimChar));
		}
		set {
			xInput.value = value.ToString();
		}
	}

	public int y {
		get {
			return int.Parse(yInput.value.TrimEnd(trimChar));
		}
		set {
			yInput.value = value.ToString();
		}
	}
}
