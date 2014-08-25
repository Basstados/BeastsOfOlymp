using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(UIPanel))]
public class BCombatMenu : MonoBehaviour {
	public int manualHeight = 720;
	public float radius = 200f;
	public float angleOffset = 0f;
	public int circleSlots = 5;
	public GameObject circleAnchor;
	public GameObject locked;
	
	public GameObject attackButtonPrefab;
	public UIButton backButton;
	//public UIButton lockedAttack;
	public UIButton endTurnButton;
	public UIButton cancelButton;

	public UISprite ZeusGewitterzorn;
	public UISprite Blitzbolzen;
	
	#region notification variables
	public GameObject gameoverPanel;
	public UILabel gameoverLabel;
	
	public BNotification bNotification;
	
	public AudioSource loseSound;
	public AudioSource winSound;
	#endregion
	
	BUnit bUnit;
	bool followUnit = false;
	
	void Update() {
		if(followUnit && bUnit != null) {
			// move above active unit
			// transplate world postion of unity into screen coordinates
			Vector3 screenPos = Camera.main.WorldToScreenPoint(bUnit.transform.position);
			// tranlate screen coordinates into UI-coordinates
			screenPos.x = screenPos.x / Screen.height * manualHeight;
			screenPos.y = screenPos.y / Screen.height * manualHeight;
			this.transform.localPosition = screenPos;
		}
	}
	
	public void OpenForBUnit(BUnit bUnit) 
	{
		HideAttackinfo ();
		
		this.bUnit = bUnit;
		if(bUnit.unit.AIControled) {
			// if a AI-unit is active, don't show the menu
			circleAnchor.SetActive(false);
			backButton.gameObject.SetActive(false);
			followUnit = false;
		} else {
			// the player controls this unit
			followUnit = true;
			
			if(bUnit.unit.CanAttack) {
				circleAnchor.SetActive(true);
				cancelButton.gameObject.SetActive(true);
			}
			//lockedAttack.gameObject.SetActive(true);
			locked.gameObject.SetActiveRecursively(true);
			endTurnButton.gameObject.SetActive(true);
			backButton.gameObject.SetActive(false);
			
			// clear attack ring ui
			List<GameObject> circleButtons = new List<GameObject>();
			foreach(Transform child in circleAnchor.transform) circleButtons.Add(child.gameObject);
			foreach(GameObject child in circleButtons) Destroy(child);
			circleButtons.Clear();
			
			// init attack buttons
			foreach(Attack atk in bUnit.unit.AttacksArray) {
				GameObject handle = (GameObject) Instantiate(attackButtonPrefab);
				handle.transform.parent = circleAnchor.transform;
				
				handle.GetComponent<BAttackButton>().Init(atk,this);
				circleButtons.Add(handle);
			}
			
			//circleButtons.Add(lockedAttack.gameObject);
			locked.gameObject.SetActiveRecursively(true);
			circleButtons.Add(endTurnButton.gameObject);
			
			
			// arrange buttons
			RepositionButtons(circleButtons);
		}
	}
	
	/// <summary>
	/// Position the list of buttons on a circle.
	/// </summary>
	/// <param name="buttons">The buttons which will placed on a circle.</param>
	void RepositionButtons(List<GameObject> buttons)
	{
		int n = circleSlots; // number of items on the circle
		
		for (int i = 0; i < buttons.Count; i++) {
			float angle = i/((float)n) * 2 * Mathf.PI + Mathf.Deg2Rad * angleOffset;
			buttons[i].transform.localPosition = new Vector3(radius * Mathf.Cos(angle),radius * Mathf.Sin(angle));
			buttons[i].transform.localScale = Vector3.one;
		}
	}
	
	public void ActionCompleted()
	{
		if(bUnit != null)
			OpenForBUnit(bUnit);
	}
	
	public void Hide() 
	{
		StartCoroutine(HideRoutine());
	}

	IEnumerator HideRoutine() {
		// Ugly hack! 
		// Hide UI at the end of the frame, incase it was opend again 
		// by a tap on the units maptile in the same frame.
		yield return new WaitForEndOfFrame();

		circleAnchor.SetActive(false);
		backButton.gameObject.SetActive(false);
		cancelButton.gameObject.SetActive(false);
		locked.gameObject.SetActiveRecursively(false);
		//lockedAttack.gameObject.SetActive(false);
		endTurnButton.gameObject.SetActive(false);
		followUnit = false;
	}
	
	#region button actions
	public void ChooseAttack()
	{
		backButton.gameObject.SetActive(true);
		cancelButton.gameObject.SetActive(false);
		locked.gameObject.SetActiveRecursively(false);
		//lockedAttack.gameObject.SetActive(false);
		endTurnButton.gameObject.SetActive(false);
		circleAnchor.SetActive(true);
	}
	
	public void ActionAttack(Attack attack) 
	{
		circleAnchor.SetActive(false);
		cancelButton.gameObject.SetActive(false);
		backButton.gameObject.SetActive(true);
		//lockedAttack.gameObject.SetActive(false);
		locked.gameObject.SetActiveRecursively(false);
		endTurnButton.gameObject.SetActive(false);
		bUnit.ClearDisplayRange();
		bUnit.DisplayAttackRange(attack);
		DisplayAttackinfo (attack);
	}
	
	public void ActionMove() 
	{
		backButton.gameObject.SetActive(true);
		bUnit.DisplayMovementRange(bUnit.unit.mapTile, bUnit.unit.MovePoints);
	}
	
	public void ActionEndTurn()
	{
		Hide();
		bUnit.EndTurn();
	}
	
	public void Back()
	{
		HideAttackinfo ();
		backButton.gameObject.SetActive(false);
		circleAnchor.SetActive(true);
		//lockedAttack.gameObject.SetActive(true);
		locked.gameObject.SetActiveRecursively(true);
		endTurnButton.gameObject.SetActive(true);
		bUnit.ClearDisplayRange();
		bUnit.DisplayMovementRange(bUnit.unit.mapTile, bUnit.unit.MovePoints);
	}
	#endregion
	
	
	
	#region notifications
	public void DisplayGameover(string text, bool playerWin)
	{
		gameoverPanel.SetActive(true);
		gameoverLabel.text = text;
		if(playerWin) {
			winSound.Play();
		} else {
			loseSound.Play();
		}
	}
	
	public void DisplayAttackinfo(Attack attack)
	{
		/*if(attack.name == "B_Zeus Gewitterzorn"){
			ZeusGewitterzorn.gameObject.SetActive(true);
		}

		if(attack.name == "B_Blitzbolzen"){
			Blitzbolzen.gameObject.SetActive(true);
		}*/
		switch (attack.name) {
				case "B_Zeus Gewitterzorn":
						ZeusGewitterzorn.gameObject.SetActive (true);
						break;
				case "B_Blitzbolzen":
						Blitzbolzen.gameObject.SetActive (true);
						break;

						Debug.Log ("Grafik angezeigt");
				}
	}
	
	public void HideAttackinfo()
	{
		ZeusGewitterzorn.gameObject.SetActive(false);
		Blitzbolzen.gameObject.SetActive(false);
		Debug.Log ("Grafik ausgeblendet");
	}
	
	public void ShowTurnStart(BUnit bUnit)
	{
		StartCoroutine(NotifyRoutine(bUnit));
	}
	
	IEnumerator NotifyRoutine(BUnit bUnit)
	{
		if(bUnit.unit.team == Unit.Team.PLAYER) {
			bNotification.Display("Dein " + bUnit.unit.UnitName + " ist am Zug");
		} else {
			bNotification.Display("Gegnerische " + bUnit.unit.UnitName + " ist am Zug");
		}
		yield return new WaitForSeconds(2f);
		EventProxyManager.FireEvent(this, new EventDoneEvent());
	}
	#endregion
}