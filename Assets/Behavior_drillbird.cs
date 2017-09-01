using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_drillbird : Movable {
	
	float flightHeight = 0;
	float xOffset = 1.6f;
	int moveDir;
	float flySpeed = 3.2f;
	float drillSpeed = 3.2f;
	float drillDistance;

	Camera mainCamera;
	GameObject playerRef;
	Animator anim;
	BoxCollider2D[] boxColliders;
	PolygonCollider2D polygonCollider;
	Vector3 finalPos;

	void Start()
	{
		playerRef = GameObject.Find ("Cricket");
		mainCamera = Camera.main;
		anim = GetComponent<Animator>();

		var collidersChild = this.transform.Find ("TriggerColliders");
		boxColliders = collidersChild.GetComponents<BoxCollider2D>();
		polygonCollider = collidersChild.GetComponent<PolygonCollider2D> ();

		float flightDistanceY;
		RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector2.down, 100f, 1 << LayerMask.NameToLayer("Platform"));
		if (hit.collider != null) {
            flightDistanceY = hit.distance - 0.85f;
        }else{
			flightDistanceY =  2f * mainCamera.orthographicSize - 0.85f;
		}
		flightHeight = this.transform.position.y - flightDistanceY;
	}

	void FixedUpdate()
	{
		if(anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")){
			FacePlayer ();
		}

		moveDir = faceLeft ?  -1 :  1;

		if(currentBehaviorState == 1){
			finalPos = new Vector3 (playerRef.transform.position.x + (xOffset * -moveDir), flightHeight, this.transform.localPosition.z);
			MoveToPos(finalPos);
		}else if(currentBehaviorState == 2){
			var dir = Mathf.Sign(xOffset);
			rbody.velocity = new Vector2( drillSpeed * moveDir, rbody.velocity.y);


			if((this.transform.position.x < playerRef.transform.position.x - 1 && moveDir == -1) ||
			(this.transform.position.x > playerRef.transform.position.x + 1 && moveDir == 1)){

				setIdleBehavior ();
			}
		}
		
	}

	void OnBecameVisible()
	{
		if(currentBehaviorState == -1){
			currentBehaviorState = 1;
		}

		if (polygonCollider.enabled) {
			setIdleHitboxes ();
		}
	}

	private void MoveToPos(Vector3 finalPos) {

		var origin = this.transform.position;

		if(Vector3.Distance(origin, finalPos) > .1f) {
			Vector3 direction = (finalPos - origin).normalized;
			rbody.velocity = direction * flySpeed;
		}else{
			rbody.velocity = new Vector2(0, 0);
			currentBehaviorState = 0;
			Invoke ("setDrillBehavior", 0.9f);
		}
	}

	void setDrillBehavior(){
		currentBehaviorState = 2;
		setAttackHitboxes ();
		anim.SetBool("Attack", true);
	}

	void setIdleBehavior(){
		rbody.velocity = new Vector2(0, 0);
		currentBehaviorState = 1;
		setIdleHitboxes ();
		anim.SetBool("Attack", false);
	}

	void setAttackHitboxes(){
		foreach(BoxCollider2D bc in boxColliders) bc.enabled = false;
		polygonCollider.enabled = true;
	}

	void setIdleHitboxes(){
		foreach(BoxCollider2D bc in boxColliders) bc.enabled = true;
		polygonCollider.enabled = false;
	}

	void FacePlayer(){
		if (playerRef.transform.position.x < this.transform.position.x && !faceLeft) {
			Flip ();
		}else if(playerRef.transform.position.x > this.transform.position.x && faceLeft){
			Flip ();
		}
	}
	
}
