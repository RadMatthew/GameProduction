﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitBase : MonoBehaviour {

	public Player controller;
	public TileStandard currentSpace;
	public Material[] unitColors;
	public Material[] spaceHighlights;

	public bool isSelected = false;
	protected bool hasMoved = false;
	protected bool hasActioned = false;
	public bool isDone = false;

	//Units Stats
	public int movement = 2;
	public int minAttackRange = 1;
	public int maxAttackRange = 1;
	public int attackPow = 1;
	public int HPmax = 1;
	public int HPcurr = 1;
	public int foodCost = 0;
	public int lumberCost = 0;
	public string unitType = "";

	public Vector3 posOffset;

	//Button Stuff;
	protected float BUTTON_X_POS = Screen.width - (Screen.width / 8);
	protected float BUTTON_WIDTH = Screen.width/9;
	protected float BUTTON_HEIGHT = Screen.height/20;
	protected float BUTTON_SPACING = Screen.height/100 + Screen.height/20;

	//Unit Hp Stuff;
	private float HP_X_POS = Screen.width * 0.45f;
	private float HP_Y_POS = Screen.height * 0.3f;
	private float HP_WIDTH = Screen.width * 0.1f;
	private float HP_HEIGHT = Screen.height * 0.05f;
	private bool entered = false;

	//Unit Stat Stuff
	private float STAT_BOX_X_POS = Screen.width*0.4f;
	private float STAT_BOX_Y_POS = Screen.height*.05f;
	private float STAT_BOX_WIDTH = Screen.width * 0.3f;
	private float STAT_BOX_HEIGHT = Screen.height * 0.1f;
	
	//use this to modify unit stats
	public virtual void init() {

	}

	// Use this for initialization
	void Start () {
		posOffset = new Vector3 (0 , .5f, 0);
		init ();

		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		foreach(GameObject i in players) {
			if(i.GetComponent<Player>() != null && getMaterialName() == i.GetComponent<Player>().getPlayerColor()) {
				this.controller = i.GetComponent<Player>();
			}
		}

		this.currentSpace = getClosestTile ();
		this.currentSpace.unitOnTile = this;
		this.transform.position = this.currentSpace.transform.position + this.posOffset;
	}

	//Called when mouse is over unit
	void OnMouseEnter()
	{
		entered = true;
	}

	//Called when mouse leaves unit
	void OnMouseExit()
	{
		entered = false;
	}
	
	//Called when unit is pressed
	void OnMouseUpAsButton() {
		if (NewGameController.currentPlayer == this.controller) {
			this.selected ();
		} else if (NewGameController.selectedUnit != null && this.currentSpace.canAttackUnitOnThis) {
			NewGameController.selectedUnit.attackUnit(this);
		}

		//if (NewGameController.currentPlayer == this.controller) {
		//	this.selected ();
		//}
		//else if (NewGameController.selectedUnit != null && this.currentSpace.canAttackUnitOnThis && !this.hasActioned) {
		//	NewGameController.selectedUnit.attackUnit(this);
		//}
		//else {
		//	NewGameController.deselectAllUnits();
		//}
	}

	// Update is called once per frame
	void Update () {
		if (this.hasMoved && this.hasActioned) {
			this.isDone = true;
		}

		if(this.isDone) {
			this.renderer.material = this.unitColors[this.controller.playerID + (this.unitColors.Length / 2)];
			this.transform.FindChild("unit").renderer.material = this.unitColors[this.controller.playerID + (this.unitColors.Length / 2)];
		}

		if (this.HPcurr <= 0) {
			Destroy(this.gameObject);
		}
	}

	void OnGUI() {
		BUTTON_X_POS = Screen.width - (Screen.width / 8);
		BUTTON_WIDTH = Screen.width/9;
		BUTTON_HEIGHT = Screen.height/20;
		BUTTON_SPACING = Screen.height/100 + Screen.height/20;
		
		HP_X_POS = Screen.width * 0.45f;
		HP_Y_POS = Screen.height * 0.3f;
		HP_WIDTH = Screen.width * 0.1f;
		HP_HEIGHT = Screen.height * 0.05f;

		STAT_BOX_X_POS = Screen.width * 0.4f;
		STAT_BOX_Y_POS = Screen.height * 0.05f;
		STAT_BOX_WIDTH = Screen.width * 0.225f;
		STAT_BOX_HEIGHT = Screen.height * 0.1f;
		
		GUI.skin.box.alignment = TextAnchor.UpperCenter;
		GUI.color = new Vector4(0.23f, 0.75f, 0.54f, 1);
		if (isSelected) {
			GUI.Box (new Rect (STAT_BOX_X_POS, STAT_BOX_Y_POS*0f, STAT_BOX_WIDTH, STAT_BOX_HEIGHT), 
			         "Unit Stats:");
			GUI.Box(new Rect (STAT_BOX_X_POS, STAT_BOX_Y_POS, STAT_BOX_WIDTH, STAT_BOX_HEIGHT),
			        "HP:" + this.HPcurr + "/"+this.HPmax +
			        " AttackRange: " + this.minAttackRange + "-"+ this.maxAttackRange);
			GUI.Box(new Rect (STAT_BOX_X_POS, STAT_BOX_Y_POS*2f, STAT_BOX_WIDTH, STAT_BOX_HEIGHT),
			        "Movement: " + this.movement +
			        " Attack Power: " + this.attackPow);
			GUI.Box(new Rect (STAT_BOX_X_POS, STAT_BOX_Y_POS*3f, STAT_BOX_WIDTH, STAT_BOX_HEIGHT),
			        "Unit Type: " + this.unitType);
		}


		if (entered) {
			GUI.color = (this.controller == NewGameController.currentPlayer) ? Color.green : Color.red;
			GUI.Box (new Rect (HP_X_POS, HP_Y_POS, HP_WIDTH, HP_HEIGHT),
			         "HP:" + this.HPcurr + "/" + this.HPmax);
		}

		if (isSelected) {
			Rect attackButton = new Rect (BUTTON_X_POS, Screen.height - BUTTON_SPACING, BUTTON_WIDTH, BUTTON_HEIGHT);
			Rect moveButton = new Rect (BUTTON_X_POS, attackButton.position.y - BUTTON_SPACING, BUTTON_WIDTH, BUTTON_HEIGHT);

			GUI.color = (!this.hasActioned) ? Color.white : Color.gray;
			if (GUI.Button (attackButton, "Attack")) {
					showAttack();
			}

			GUI.color = (!this.hasMoved) ? Color.white : Color.gray;
			if (GUI.Button (moveButton, "Move")) {
					showMovement();
			}
		}
	}

	public void attackUnit (UnitBase target) {
		target.HPcurr -= this.attackPow;
		this.hasActioned = true;
		deselect ();
	}

	public void moveUnit(TileStandard moveLocation) {
		this.currentSpace.unitOnTile = null;
		this.currentSpace = moveLocation;
		this.currentSpace.unitOnTile = this;
		this.transform.position = this.currentSpace.transform.position + this.posOffset;
		this.hasMoved = true;
		deselect ();
	}

	public void resolveTurn() {
		this.hasMoved = false;
		this.hasActioned = false;
		this.isDone = false;
		this.renderer.material = this.unitColors [this.controller.playerID];
		this.transform.FindChild("unit").renderer.material = this.unitColors[this.controller.playerID];
		deselect ();
	}

	public void selected () {
		NewGameController.deselectAllUnits ();
		NewGameController.selectedUnit = this;
		highlightCurrentSpace (spaceHighlights[1]);
		isSelected = true;

		if (!hasActioned) {
			showAttack();
		}

		if (!hasMoved) {
			showMovement();
		}
	}

	public void deselect () {
		if (NewGameController.selectedUnit == this) {
			NewGameController.selectedUnit = null;
		}
		NewGameController.clearHighlights ();
		isSelected = false;
	}

	public string getMaterialName() {
		string matName = renderer.material.name;
		matName = matName.Substring (0, matName.IndexOf (" ("));

		return matName;
	}

	public void giveControl(Material mat) {
		this.renderer.material = mat;
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		foreach(GameObject i in players) {
			if(i.GetComponent<Player>() != null && getMaterialName() == i.GetComponent<Player>().getPlayerColor()) {
				this.controller = i.GetComponent<Player>();
			}
		}
	}

	private void highlightCurrentSpace(Material highlight) {
		MeshRenderer currentSpaceTile = currentSpace.transform.FindChild("Terrain").GetComponentInChildren<MeshRenderer> ();
		currentSpaceTile.material = highlight;
	}

	protected void showAttack() {
		if (!hasActioned) {
			NewGameController.clearHighlights();
			showAttackHelper(this.maxAttackRange, this.currentSpace, this.spaceHighlights[3]);
			showAttackHelper(this.minAttackRange - 1, this.currentSpace, this.spaceHighlights[0]);
			highlightCurrentSpace(this.spaceHighlights[1]);
		}
	}
	
	private void showAttackHelper(int attackRange, TileStandard tile, Material mat) {

		if (attackRange > 0) {
			Collider[] hitCollider = Physics.OverlapSphere(tile.transform.position, 2);
			List<TileStandard> tiles = new List<TileStandard>();
			
			foreach(Collider i in hitCollider) {
				if(i.GetComponent<TileStandard>() != null){
					tiles.Add(i.GetComponent<TileStandard>());
				}
			}
			
			foreach(TileStandard i in tiles) {
				if((i.unitOnTile != null && i.unitOnTile.controller != this.controller) || i.unitOnTile == null) {
					i.transform.FindChild("Terrain").GetComponentInChildren<MeshRenderer>().material = mat;
					if(mat == this.spaceHighlights[3]) {
						i.canAttackUnitOnThis = true;
					} 
					else {
						i.canAttackUnitOnThis = false;
					}
				}
			}

			foreach(TileStandard i in tiles) {
				showAttackHelper(attackRange - 1, i, mat);
			}

		}


		//if (attackRange > 0) {
		//	Collider[] hitColliders = Physics.OverlapSphere (tile.transform.position, 2);
		//	List<TileStandard> tiles = new List<TileStandard>();
		//	
		//	foreach(Collider i in hitColliders) {
		//		if(i.GetComponent<TileStandard>() != null) {
		//			tiles.Add (i.GetComponent<TileStandard>());
		//		}
		//	}
		//	
		//	for(int i = 0; i < tiles.Count; i++) {
		//		if(attackRange - 1 >= 0) {
		//			if((tiles[i].unitOnTile != null && tiles[i].unitOnTile.controller != this.controller) || tiles[i].unitOnTile == null) {
		//
		//				tiles[i].canAttackUnitOnThis = true;
		//				tiles[i].transform.FindChild("Terrain").GetComponentInChildren<MeshRenderer>().material = mat;
		//
		//
		//			}
		//			showAttackHelper(attackRange - 1, tiles[i], mat);
		//		}
		//	}
		//}
	}

	protected void showMovement() {
		if (!hasMoved) {
			NewGameController.clearHighlights();
			showMovementRangeHelper(this.movement, this.currentSpace);
			highlightCurrentSpace(this.spaceHighlights[1]);
		}
	}
	
	private void showMovementRangeHelper(int moveRange, TileStandard tile) {
		if (moveRange > 0) {
			Collider[] hitCollider = Physics.OverlapSphere(tile.transform.position, 2);
			List<TileStandard> tiles = new List<TileStandard>();

			foreach(Collider i in hitCollider) {
				if(i.GetComponent<TileStandard>() != null) {// && !i.GetComponent<TileStandard>().canMoveTo){
					tiles.Add(i.GetComponent<TileStandard>());
				}
			}

			foreach(TileStandard i in tiles) {
				if(!i.unitOnTile) {
					i.transform.FindChild("Terrain").GetComponentInChildren<MeshRenderer>().material = this.spaceHighlights[2];
					i.canMoveTo = true;
				}
			}

			foreach(TileStandard i in tiles) {
				showMovementRangeHelper(moveRange - i.moveCost, i);
			}
		}
	}
	
	private TileStandard getClosestTile() {
		GameObject[] tiles = GameObject.FindGameObjectsWithTag ("Tile");
		GameObject closest = null;
		foreach(GameObject i in tiles) {
			if(!closest) {
				closest = i;
			}
			
			if(Vector3.Distance(this.transform.position, i.transform.position) <= Vector3.Distance(this.transform.position, closest.transform.position)) {
				closest = i;
			}
		}
		
		return closest.GetComponent<TileStandard>();
	}
}
