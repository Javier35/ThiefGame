using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpDownPlatform : SpecialTerrain {

	private Collider2D thisCollider;
	public bool passable = false;

	void Start () {
		thisCollider = GetComponent<BoxCollider2D> ();
	}

	public override void StandEvent (GameObject gObject){}

	public override void JumpEvent (GameObject gObject){

		if(Input.GetKey(KeyCode.DownArrow)){
			var characterComponent = gObject.GetComponent<PlatformerCharacter2D> ();

			var childHitboxContainer = gObject.transform.FindChild ("PhysicalColliders");

			if (childHitboxContainer != null) {
				var otherCollider = childHitboxContainer.GetComponent<BoxCollider2D> ();
				Physics2D.IgnoreCollision (otherCollider, thisCollider);
				characterComponent.DoFall ();
				passable = true;
				StartCoroutine (BecomeSolid(otherCollider));
			} else {
				Debug.Log ("no physical colliders in child gameobject");
			}

		}
	}

	private IEnumerator BecomeSolid(Collider2D col){
		yield return new WaitForSeconds (1f);
		Physics2D.IgnoreCollision (col, thisCollider, false);
		passable = false;
	}
}