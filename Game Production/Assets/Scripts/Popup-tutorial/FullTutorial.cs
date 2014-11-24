﻿using UnityEngine;
using System.Collections;

public class FullTutorial : MonoBehaviour {

	private int progress = 0;
	private Rect popUpPos = new Rect((Screen.width/2) - 350, 200, 700, 350);
	private Rect nextButtonPos = new Rect((Screen.width/2) + 175, 500, 125, 35);
	public GUISkin mySkin;

	private int xOffset = 0;
	private bool doOnce = false;


	public static bool movedAUnit = false;
	public static bool disableEndTurn = true;

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
		}

		foreach(GameObject u in tutUnits)
		{
			if(u.GetComponent<UnitBase>() !=null)
			{
				u.GetComponent<UnitBase>().enabled = false;
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

	
	void OnGUI(){

		GUI.skin.box.wordWrap = true;
		if (progress == 0) {
			disableAllForTut();
			GUI.Box(new Rect(popUpPos), "\nWelcome to Daren's Siege! \n The main objective is to eliminate all of your opponent's units.", mySkin.GetStyle("Box"));
			if(GUI.Button( new Rect(nextButtonPos), "Next")) {
				progress++;
			}
		}
		
		if (progress == 1) {
			GUI.Box(new Rect(popUpPos), "\nUNITS \n Units are your main source to victory. They will allow you to overcome your opponent's defenses. \n\n They each have an upkeep cost, reducing your total food at the end of each turn. \n", mySkin.GetStyle("Box"));
			if(GUI.Button( new Rect(nextButtonPos), "Next")) {
				progress++;
			}
		}
		
		if (progress == 2) {
			enableUnitsForTut();
			GUI.Box(popUpPos, "\nUNITS \n Now select a unit by clicking a RED Piece.", mySkin.GetStyle("Box"));
			if(NewGameController.selectedUnit != null) {
				progress++;
			}
		}
		
		if (progress == 3) {
			GUI.Box(new Rect(popUpPos), "\nUNITS \n Above you will find your unit's stats. \n\n HP (Current / Max) \n  Attack Range \n Movement \n Attack Power \n Upkeep Cost", mySkin.GetStyle("Box"));
			if(GUI.Button( new Rect(nextButtonPos), "Next")) {
				progress++;
			}
		}
		
		if (progress == 4) {
			GUI.Box(new Rect(popUpPos), "\nUNITS \n In the bottom right you will find the actions each unit can take. \n\n Capture \n Move \n Attack", mySkin.GetStyle("Box"));
			if(GUI.Button( new Rect(nextButtonPos), "Next")) {
				progress++;
			}
		}
		
		if (progress == 5) {
			GUI.Box(new Rect(popUpPos), "\nUNITS \n\n Capture: \n Uses both your movement and attack action to claim the tile the unit is on. Captured tiles are indicated by their corresponding ring color.\n You can find the amount of resources you currently have in the bottom left corner.\n\n Move: \n Allows the selected unit to move to a selected tile within its move range. (Indicated by GREEN highlights). ", mySkin.GetStyle("Box"));
			if(GUI.Button( new Rect(nextButtonPos), "Next")) {
				progress++;
			}
		}
		
		if (progress == 6) {
			GUI.Box(new Rect(popUpPos), "\nUNITS \n\n Attack: \nAllows the selected unit to damage an opposing unit within its attack range (Indicated by ORANGE highlights). \n\n Clicking the same unit will alternate between move and attack.", mySkin.GetStyle("Box"));
			if(GUI.Button( new Rect(nextButtonPos), "Next")) {
				progress++;
			}
		}

		if( progress == 7) {
			enableTilesForTut();
			if(!doOnce) {
				doOnce = true;
				for(int i = 0; i < 5; i++) {
					StartCoroutine(moveToSide());
				}
			}
			GUI.Box(new Rect(popUpPos.position.x - xOffset, popUpPos.position.y, popUpPos.width, popUpPos.height), "\nUNITS \n\n Now, select a desired RED unit, and move it to any location marked with a GREEN highlight", mySkin.GetStyle("Box"));
			if(movedAUnit) {
				progress++;
			}
		}

		if( progress == 8) {
			disableAllForTut();
			enableTilesForTut();
			NewGameController.deselectAllUnits();
			GUI.Box(popUpPos, "\nTERRAIN \n There are several terrains you will encounter during Daren's Siege. \n\n Mountains \n Forests \n Plains \n Mana Wells \n Hills \n\n In the top left you will find the amount of resource each terrain provides \n(While hovering over a terrain)", mySkin.GetStyle("Box"));
			if(GUI.Button( new Rect(nextButtonPos), "Next")) {
				progress++;
			}
		}

		if( progress == 9) {
			GUI.Box(popUpPos, "\nTERRAIN \n\n MOUNTAINS: \n Impedes movement\n Provides Ore to the player who captures at the end of each turn.\n Increases attack range of the RANGED unit on the mountain. \n\n FORESTS: \n Impedes movement \n Provides lumber to the player who captures at the end of each turn \n Reduces attack range of the RANGED unit in the forest", mySkin.GetStyle("Box"));
			if(GUI.Button( new Rect(nextButtonPos), "Next")) {
				progress++;
			}
		}

		if( progress == 10) {
			GUI.Box(popUpPos, "\nTERRAIN \n\n PLAINS: \n Normal Movement\n Provides food to the player who captures at the end of each turn.\n\n HILLS: \n Impedes movement \n Provides food to the player who captures at the end of each turn \n Increases attack power of melee units on the hill.", mySkin.GetStyle("Box"));
			if(GUI.Button( new Rect(nextButtonPos), "Next")) {
				progress++;
			}
		}

		if( progress == 11) {
			GUI.Box(popUpPos, "\nTERRAIN\n\n MANA WELLS: \n Normal Movement\n No resources are provided for being captured \n Increases attack damage of mage units on the mana well \n Decreases attack range of mage units on the mana well", mySkin.GetStyle("Box"));
			if(GUI.Button( new Rect(nextButtonPos), "Next")) {
				progress++;
			}
		}

		if( progress == 12) {
			disableAllForTut();
			GUI.Box(popUpPos, "\nBUILDING UNITS \n\n By clicking on the building you control, you will be able to spend resources to purchase more units.", mySkin.GetStyle("Box"));
			if(GUI.Button( new Rect(nextButtonPos), "Next")) {
				progress++;
			}
		}

		if( progress == 13) {
			enableTilesForTut();
			enableUnitsForTut();
			disableEndTurn = false;
			GUI.Box(popUpPos, "\nWhen you are done with your turn press the end turn button located in the bottom left corner. \n\n\n BEST OF LUCK!", mySkin.GetStyle("Box"));
			if(GUI.Button( new Rect(nextButtonPos), "Close")) {
				progress++;
			}
		}
	}

	private IEnumerator moveToSide() {
		for(int i = 0; i < 120; i++) {

			yield return new WaitForSeconds(.00001f);
			xOffset++;
		}
	}
}