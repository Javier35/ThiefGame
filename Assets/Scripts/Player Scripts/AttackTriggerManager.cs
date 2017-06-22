using UnityEngine;
using System.Collections;

public class AttackTriggerManager : MonoBehaviour {

	private BoxCollider2D attackBox;
	private ArrayList enemyRefs = new ArrayList();

	void Awake(){
		attackBox = gameObject.GetComponent<BoxCollider2D> ();
		attackBox.enabled = false;
	}

	void OnTriggerEnter2D(Collider2D col){
		

		var hitHandler = col.gameObject.GetComponent<HitHandler> ();
		if (hitHandler != null) {
			hitHandler.HitEvent (gameObject.GetComponentInParent<PlayerDamageManager> ().damage);
			CameraShake.Shake (0.1f, 0.015f);

			if (col.gameObject.tag == "Enemy") {
				enemyRefs.Add (col.gameObject.transform.parent.gameObject);
			}
		}

	}
		
}
