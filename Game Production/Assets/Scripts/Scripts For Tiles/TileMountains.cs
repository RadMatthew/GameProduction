﻿using UnityEngine;
using System.Collections;

public class TileMountains : TileStandard {

	override public void init()
	{
		this.originalMoveCost = 3;
		this.defensiveValue = 1;
		this.TerrainName = "Mountain";
		this.ResourceType = "Ore";
		this.ResourceValue = Random.Range (0, 5);
	}
	//Should units be able to shoot through mountain tiles?

	override protected void tempModsUpdate()
	{
			//Debug.Log("is called");
			this.unitOnTile.transform.position = new Vector3(this.unitOnTile.transform.position.x, 3.0f, this.unitOnTile.transform.position.z);
				
	}
	
	override public void SelectTarget(TileStandard target)
	{
		NewGameController.selectedBuilding.SelectTarget (target);
	}

}
