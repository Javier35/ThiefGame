using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveDamage : HitHandler {

	DamageManager damageManager;

	void Start(){
		damageManager = GetComponentInParent<DamageManager> ();
	}

	override public void HitEvent(int damage){
		damageManager.ReceiveDamage (damage);
	}
}
