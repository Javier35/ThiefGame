using UnityEngine;
using System.Collections;

public class CollisionManager : MonoBehaviour {

	private Collider2D thisCollider;
	[SerializeField]private string[] ignoreCollisionsWith = {"Enemy", "Collectible", "Interactable"};
	// Use this for initialization
	void Start () {
		thisCollider = gameObject.GetComponent<BoxCollider2D> ();
	}

	void OnCollisionEnter2D (Collision2D col){
		//if col's tag is in ignoreCollisionsWith, then ignore the collisions
		if (System.Array.IndexOf(ignoreCollisionsWith, col.gameObject.tag) != -1) {
			
			Physics2D.IgnoreCollision (col.collider, thisCollider);
		}
	}
}
