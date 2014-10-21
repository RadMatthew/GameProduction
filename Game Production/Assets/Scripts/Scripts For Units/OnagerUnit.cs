using UnityEngine;
using System.Collections;

public class OnagerUnit : UnitBase {
	
	override public void init() {
		this.HPmax = 15;
		this.HPcurr = this.HPmax;
		this.minAttackRange = 3;
		this.maxAttackRange = 6;
		this.movement = 3;
		this.attackPow = 6;
		this.foodCost = 0;
		this.lumberCost = 10;
		this.unitType = "Siege";
		Debug.Log ("Onager Unit");
	}
}
