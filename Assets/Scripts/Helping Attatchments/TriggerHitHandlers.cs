using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHitHandlers : MonoBehaviour {

	private List<int> objectsHit = new List<int>();
	[SerializeField] int damageStrength = 1;

	void OnTriggerEnter2D(Collider2D col){

		var hitHandler = col.gameObject.GetComponent<HitHandler> ();
		if (hitHandler != null) {

			int hittedObjectId = col.gameObject.GetInstanceID ();

			if (!objectsHit.Contains (hittedObjectId)) {
				hitHandler.HitEvent (damageStrength);
				objectsHit.Add (hittedObjectId);
			}
		}
	}

	public void ClearHittedList (){
		objectsHit.Clear ();
	}
}