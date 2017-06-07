using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundChecker : MonoBehaviour {

	private BoxCollider2D groundBox;
	[HideInInspector] public bool grounded = true;
	[HideInInspector] public bool teetering = false;

	private RaycastHit2D rayHit1, rayHit2, rayHit3, rayHit4; //ground hit flags, are true when ray is touching ground
	private Vector3 rayStart1, rayStart2, rayStart3, rayStart4; //starting points for the 4 rays that check if character is grounded
	private float rayLength = 0.1f;

	private PlatformerCharacter2D characterReference;

	void Awake(){
		characterReference = GetComponentInParent<PlatformerCharacter2D> ();
		groundBox = GetComponent<BoxCollider2D> ();
	}

	void Update(){
		PlaceVectorStartPoints ();
		RaycastGroundVectors ();
		InterpreteRayHits ();
	}
		

	private void PlaceVectorStartPoints(){ // main function that raycasts the 4 rays and updates the flags if they touch ground

		var centerpointX = groundBox.bounds.center.x;
		rayStart1 = new Vector3 (groundBox.bounds.min.x, groundBox.bounds.min.y + 0.05f, this.transform.position.z);
		rayStart2 = new Vector3 (centerpointX - groundBox.bounds.extents.x/4, groundBox.bounds.min.y + 0.05f, this.transform.position.z);
		rayStart3 = new Vector3 (centerpointX + groundBox.bounds.extents.x/4, groundBox.bounds.min.y + 0.05f, this.transform.position.z);
		rayStart4 = new Vector3 (groundBox.bounds.max.x, groundBox.bounds.min.y + 0.05f, this.transform.position.z);
	}


	private void RaycastGroundVectors(){

		rayHit1 = Physics2D.Raycast (rayStart1, Vector3.down, rayLength, 1 << LayerMask.NameToLayer("Platform"));
		rayHit2 = Physics2D.Raycast (rayStart2, Vector3.down, rayLength, 1 << LayerMask.NameToLayer("Platform"));
		rayHit3 = Physics2D.Raycast (rayStart3, Vector3.down, rayLength, 1 << LayerMask.NameToLayer("Platform"));
		rayHit4 = Physics2D.Raycast (rayStart4, Vector3.down, rayLength, 1 << LayerMask.NameToLayer("Platform"));
	}


	private void InterpreteRayHits(){
		
		if(
				(rayHit1 && !CheckIfPlatformIsPassable(rayHit1))|| 
				(rayHit2 && !CheckIfPlatformIsPassable(rayHit2))|| 
				(rayHit3 && !CheckIfPlatformIsPassable(rayHit3))|| 
				(rayHit4 && !CheckIfPlatformIsPassable(rayHit4))
			)
				grounded = true;
		else
				grounded = false;

		teetering = false;
		if (characterReference.m_FacingRight) {
			if (rayHit1 && !rayHit2)
					teetering = true;
		} else {
			if (rayHit4 && !rayHit3)
					teetering = true;
		}
		if(grounded)
			if (characterReference.m_FacingRight) {
				if (rayHit1 && !rayHit2 )
						teetering = true;
			} else {
				if (rayHit4 && !rayHit3)
						teetering = true;
			}
	}

	private bool CheckIfPlatformIsPassable(RaycastHit2D rHit){
		if(rHit.collider != null){
			var jumpdownPlatformObject = rHit.collider.gameObject.GetComponent<JumpDownPlatform> ();
			if(jumpdownPlatformObject != null){
				return jumpdownPlatformObject.passable;
			}
		}
		return false;
	}

	void OnCollisionStay2D(Collision2D col){
		
		if (col.gameObject.tag == "Platform") {

			ContactPoint2D contact = col.contacts [0];
			if (Vector3.Dot (contact.normal, Vector3.up) > 0.5) {

				//collision was from below
				if(!grounded)
					grounded = true;
			}
		}
	}

}
