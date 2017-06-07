using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DealDamage : MonoBehaviour {

	DamageManager damageManager;

	[SerializeField]private string[] damageableByMe = new string[] {"Player"};
	int damage;

	void Start(){
		damageManager = GetComponentInParent<DamageManager> ();
		damage = damageManager.damage;
	}

	void OnTriggerEnter2D(Collider2D col){
		if (damageableByMe.Contains(col.tag)) {
			var damageManager = col.gameObject.GetComponentInParent<DamageManager> ();
			damageManager.ReceiveDamage (damage);
		}
	}
}
