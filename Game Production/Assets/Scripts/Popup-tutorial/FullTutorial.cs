﻿using UnityEngine;
using System.Collections;

public class FullTutorial : MonoBehaviour {

	public static int progress = 0;
	private Rect popUpPosStart = new Rect((Screen.width/2) - 330, 175, 700, 350);
	private Rect nextButtonPosStart = new Rect((Screen.width/2) - 275, 455, 130, 35);
	public GUISkin mySkin;

	private int xOffset = 0;
	private bool doOnce = false;

	public static bool movedAUnit = false;
	public static bool unitAttack = false;
	public static bool disableEndTurn = true;
	public static bool firstCapture = false;
	public static bool TutorialActive = true;


	void disableAllForTut()
	{
		GameObject[] tutUnits = GameObject.FindGameObjectsWithTag ("Unit");
     	GameObject[] tutTiles = GameObject.FindGameObjectsWithTag ("Tile");
		foreach(GameObject t in tutTiles)
		{
			if(t.GetComponent<TileStandard>() !=null)
			{
				t.GetComponent<TileStandard>().enabled = false;
			}

			
			if(t.GetComponent<UnitBuilding>() != null) 
			{
				t.GetComponent<UnitBuilding>().enabled = false;
			}
		}

		foreach(GameObject u in tutUnits)
		{
			if(u.GetComponent<UnitBase>() !=null)
			{
				u.GetComponent<UnitBase>().enabled = false;
			}
		}
	}
	void enableAllForTut()
	{
		GameObject[] tutUnits = GameObject.FindGameObjectsWithTag ("Unit");
		GameObject[] tutTiles = GameObject.FindGameObjectsWithTag ("Tile");
		foreach(GameObject t in tutTiles)
		{
			if(t.GetComponent<TileStandard>() !=null)
			{
				t.GetComponent<TileStandard>().enabled = true;
			}
			
			
			if(t.GetComponent<UnitBuilding>() != null) 
			{
				t.GetComponent<UnitBuilding>().enabled = true;
			}
		}
		
		foreach(GameObject u in tutUnits)
		{
			if(u.GetComponent<UnitBase>() !=null)
			{
				u.GetComponent<UnitBase>().enabled = true;
			}
		}
	}

	void disableBarracks() {
		GameObject[] tutTiles = GameObject.FindGameObjectsWithTag ("Tile");
		foreach(GameObject t in tutTiles)
		{
			if(t.GetComponent<UnitBuilding>() != null) 
			{
				t.GetComponent<UnitBuilding>().enabled = false;
			}
		}
	}

	void enableBarracks() {
		GameObject[] tutTiles = GameObject.FindGameObjectsWithTag ("Tile");
		foreach(GameObject t in tutTiles)
		{
			if(t.GetComponent<UnitBuilding>() != null) 
			{
				t.GetComponent<UnitBuilding>().enabled = true;
			}
		}
	}

	void enableTilesForTut()
	{
	    GameObject[] tutTiles = GameObject.FindGameObjectsWithTag ("Tile");
		foreach(GameObject t in tutTiles)
		{
			if(t.GetComponent<TileStandard>() !=null)
			{
				t.GetComponent<TileStandard>().enabled = true;
			}
	
		}
	}
	
	void enableUnitsForTut()
	{
		GameObject[] tutUnits = GameObject.FindGameObjectsWithTag ("Unit");
		foreach(GameObject u in tutUnits)
		{
			if(u.GetComponent<UnitBase>() !=null)
			{
				u.GetComponent<UnitBase>().enabled = true;
			}
		}
	}

	void enableUnitsForCapture()
	{
		GameObject[] tutUnits = GameObject.FindGameObjectsWithTag ("Unit");
		GameObject[] tutTiles = GameObject.FindGameObjectsWithTag ("Tile");

		foreach(GameObject u in tutUnits){
			if(u.GetComponent<UnitBase>() != null && u.GetComponent<UnitBase>().currentSpace.controller != u.GetComponent<UnitBase>().controller){
				u.GetComponent<UnitBase>().enabled = true;
			}
		}

		foreach(GameObject t in tutTiles) {
			if(t.GetComponent<TileStandard>() != null && t.GetComponent<TileStandard>().unitOnTile != null && t.GetComponent<TileStandard>().controller != t.GetComponent<TileStandard>().unitOnTile.controller){
				t.GetComponent<TileStandard>().enabled = true;
			}
		}
	}

	void enableSpecificTiles() 
	{
		GameObject[] tutTiles = GameObject.FindGameObjectsWithTag ("Tile");
		foreach(GameObject t in tutTiles)
		{
			if(t.GetComponent<EnableSelected>() != null)
			{
				t.GetComponent<TileStandard>().enabled = true;
			}
		}
	}
	
	void enableMeleeUnits()
	{
		GameObject[] meleeUnits = GameObject.FindGameObjectsWithTag ("Unit");
		GameObject[] tutTiles = GameObject.FindGameObjectsWithTag ("Tile");

		foreach(GameObject m in meleeUnits){
			if(m.GetComponent<MeleeUnit>() != null)
			{
				m.GetComponent<UnitBase>().enabled = true;
			}
		}

		foreach(GameObject t in tutTiles) {
			if(t.GetComponent<TileStandard>() != null && t.GetComponent<TileStandard>().unitOnTile != null && t.GetComponent<TileStandard>().unitOnTile.unitClass.Equals("Melee"))
			{
				t.GetComponent<TileStandard>().enabled = true;
			}
		}
	}

	
	void OnGUI(){

		Rect popUpPos = new Rect (popUpPosStart.position.x - xOffset, popUpPosStart.position.y, popUpPosStart.width, popUpPosStart.height);
		Rect nextButtonPos = new Rect (nextButtonPosStart.position.x - xOffset, nextButtonPosStart.position.y, nextButtonPosStart.width, nextButtonPosStart.height);
		Rect SecondButtonPos = new Rect (nextButtonPos.position.x + 150, nextButtonPos.position.y, nextButtonPos.width, nextButtonPos.height);
		
		GUI.skin.box.wordWrap = true;

		if (progress == 0) {
			StartCoroutine(delay());

			GUI.Box(new Rect(popUpPos), "", mySkin.GetStyle("Box"));
			mySkin.GetStyle("Label").fontSize = 52;
			GUI.Label(popUpPos, "\n\nWelcome to Daren's Siege!", mySkin.GetStyle("Label"));
			if(GUI.Button( new Rect(nextButtonPos), "Next")) {
				progress++;
			}
		}
		if (progress == 1) {
			mySkin.GetStyle ("Label").fontSize = 32;
			GUI.Box (new Rect (popUpPos), "\n\n\n\n Would you Like to play through the tutorial?", mySkin.GetStyle ("Box"));
			GUI.Label (popUpPos, "\nTutorial", mySkin.GetStyle ("Label"));
			if (GUI.Button (new Rect (nextButtonPos), "Yes")) {
					progress++;
			}
			if (GUI.Button (new Rect (SecondButtonPos), "No")) {
					progress = 20;
			}
		}


		if (progress == 2) {
			if(!doOnce) {
				doOnce = true;
				for(int i = 0; i < 10; i++) {
					StartCoroutine(moveToSide());
				}
			}
			enableMeleeUnits();
			mySkin.GetStyle("Label").fontSize = 32;
			GUI.Box(new Rect(popUpPos), "\n\n\n\n Units are your main source to victory.\n\n Select one of the four RED melee \nunits in the forward row. \n", mySkin.GetStyle("Box"));
			GUI.Label(popUpPos, "\nUNITS", mySkin.GetStyle("Label"));
			if(NewGameController.selectedUnit != null) {
				progress++;
			}
		}
		
		if (progress == 3) {
			enableSpecificTiles();
			GUI.Box(popUpPos, "\n\n\n The light blue highlighted spaces show \nwhere your unit can move.\n\nNow move the selected unit\n next to the wounded Neutral melee unit", mySkin.GetStyle("Box"));
			GUI.Label(popUpPos, "\nMOVEMENT", mySkin.GetStyle("Label"));
			if(movedAUnit) {
				progress++;
			}
		}

		if (progress == 4) {
			GUI.Box(new Rect(popUpPos), "\n\n\n The orange highlighted spaces\n indicate your attack range.\n\nHover over the enemy unit to view its health. \n\nNow attack the enemy Neutral unit.", mySkin.GetStyle("Box"));
			GUI.Label(popUpPos, "\nCOMBAT", mySkin.GetStyle("Label"));
			if(unitAttack) {
				progress++;
			}
		}
		
		if (progress == 5) {
			unitAttack= false;
			TutorialActive = true;
			GUI.Box(new Rect(popUpPos), "\n\n\n Good, the enemy unit lost \nhealth equal to your attack power. \n\nAttack power, along with other useful information \nis displayed in the selected unit's stat block.", mySkin.GetStyle("Box"));
			GUI.Label(popUpPos, "\nCOMBAT", mySkin.GetStyle("Label"));
			if(GUI.Button( new Rect(nextButtonPos), "Next")) {
				progress++;
			}
		}
		
		if (progress == 6) {
			enableMeleeUnits();
			enableSpecificTiles();
			GUI.Box(new Rect(popUpPos), "\n\n\nScroll over the wounded Neutral unit \nto see its remaining HP. \n\nAnother good attack should end this foe.\nUse another melee unit to defeat the Neutral unit.", mySkin.GetStyle("Box"));
			GUI.Label(popUpPos, "\nCOMBAT", mySkin.GetStyle("Label"));
			if(unitAttack) {
				progress++;
			}
		}
		
		if (progress == 7) {
			disableAllForTut();
			GUI.Box(new Rect(popUpPos), "\n\n\nVery good Commander. \nKeep this up and we shall win for sure.\n\nWhen you are done with your turn press\n the end turn button located in the bottom left corner.", mySkin.GetStyle("Box"));
			GUI.Label(popUpPos, "\nCOMBAT", mySkin.GetStyle("Label"));
			if(GUI.Button( new Rect(nextButtonPos), "Close")) {
				progress++;
				disableEndTurn = false;
				enableAllForTut();
			}
		}

		if(progress == 8) {
			disableBarracks();
		}

		if( progress == 10) {
			disableEndTurn = true;
			disableAllForTut();
			GUI.Box(popUpPos, "\n\n\nAt the start of each turn you will lose food equal to your current upkeep located in the bottom left of the screen.\n\n Your upkeep is determined by the units you control. \nThe more units, the higher the upkeep.", mySkin.GetStyle("Box"));
			GUI.Label(popUpPos, "\nUPKEEP", mySkin.GetStyle("Label"));
			if(GUI.Button( new Rect(nextButtonPos), "Next")) {
				progress++;
			}
		}

		if( progress == 11) {
			GUI.Box(popUpPos, "\n\n\nIf there is too little food, your units will begin to starve, \ntaking a point of damage at the start of each turn. \n\nThis will continue until there is enough food \nto feed all of your units.", mySkin.GetStyle("Box"));
			GUI.Label(popUpPos, "\nUPKEEP", mySkin.GetStyle("Label"));
			if(GUI.Button( new Rect(nextButtonPos), "Next")) {
				progress++;
			}
		}

		if( progress == 12) {
			GUI.Box(popUpPos, "\n\n\nTile dominance is shown by the color of ring around it.\n\nRed for the RED army. \nBlue for the BLUE army. \nGrey for unclaimed territory.", mySkin.GetStyle("Box"));
			GUI.Label(popUpPos, "\nDOMINANCE", mySkin.GetStyle("Label"));
			if(GUI.Button( new Rect(nextButtonPos), "Next")) {
				progress++;
			}
		}

		if( progress == 13) {
			enableTilesForTut();
			NewGameController.deselectAllUnits();
			GUI.Box(popUpPos, "\n\n\nTiles under your influence will\n provide you with resources each turn.\n\n Different tiles provide different resources.\n Scroll over different tiles to\n see what resources they provide.", mySkin.GetStyle("Box"));
			GUI.Label(popUpPos, "\nDOMINANCE", mySkin.GetStyle("Label"));
			if(GUI.Button( new Rect(nextButtonPos), "Next")) {
				disableAllForTut();
				progress++;
			}
		}

		if( progress == 14) {
				Debug.Log(progress);
			
			disableAllForTut();
			enableUnitsForCapture();
			GUI.Box(popUpPos, "\n\n\nSome terrains, such as walls, \nalso provide defensive bonuses. \n\nNow select a unit on unclaimed territory\n and press the middle mouse button down\n to claim the tile for your army.", mySkin.GetStyle("Box"));
			GUI.Label(popUpPos, "\nDOMINANCE", mySkin.GetStyle("Label"));
			if(firstCapture) {
				progress++;
				Debug.Log(progress);
			}
		}

		if( progress == 15) {
			GUI.Box(popUpPos, "\n\n\nYou have now claimed your first tile. \n\nNotice how the ring around the\n tile has changed to RED.\n\n Go ahead and use the rest of your units.", mySkin.GetStyle("Box"));
			GUI.Label(popUpPos, "\nDOMINANCE", mySkin.GetStyle("Label"));
			if(GUI.Button( new Rect(nextButtonPos), "Close")) {
				progress++;
			}
		}

		if(progress == 16) {
			enableTilesForTut();
			enableUnitsForTut();
			disableBarracks();
			disableEndTurn = false;
		}

		if( progress == 18) {
			disableEndTurn = true;
			disableAllForTut();
			enableBarracks();
			GUI.Box(popUpPos, "\n\n\nIf you ever find yourself lacking units, \nyou can always train units from your barracks.\n\nNow select the barracks and train a unit", mySkin.GetStyle("Box"));
			GUI.Label(popUpPos, "\nREINFORCEMENTS", mySkin.GetStyle("Label"));
			if(NewGameController.currentPlayer.numberOfUnits == 10) {
				progress++;
			}
		}

		if(progress == 19) {
			GUI.Box(popUpPos, "\n\n\n\nYou have now learned all the basics.\n\nGood luck in defeating the enemy!", mySkin.GetStyle("Box"));
			if(GUI.Button( new Rect(nextButtonPos), "Close")) {
				progress++;
			}
		}

		if(progress == 20) {
			enableTilesForTut();
			enableUnitsForTut();
			disableEndTurn = false;
			TutorialActive = false;
			foreach(MeleeUnit a in FindObjectsOfType(typeof(MeleeUnit)))
			{
				if(a.GetComponent<EnableSelected>() != null){
					Destroy(a.gameObject);

				}
			}
		}

	}

	private IEnumerator moveToSide() {
		for(int i = 0; i < 60; i++) {

			yield return new WaitForSeconds(.00001f);
			xOffset++;
		}
	}

	private IEnumerator delay(){
		yield return new WaitForSeconds (.25f);
		disableAllForTut ();

	}
}
