using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_drillbird : Movable {
	

	//0 = inactive
	//1 = Flying into position
	//2 = drill forward
	float currentBehaviorState = -1;
	
	float flightHeight = 0;
	float xOffset = 1.6f;
	int moveDir;
	float flySpeed = 4.2f;
	float drillSpeed = 3.2f;

	Camera mainCamera;
	Animator anim;

	void Start()
	{
		mainCamera = Camera.main;
		anim = GetComponent<Animator>();
		
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
		moveDir = faceLeft ?  -1 :  1;
		
		if(currentBehaviorState == 1){
			MoveToPos();
		}else if(currentBehaviorState == 2 && anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack")){
			
			var dir = Mathf.Sign(xOffset);
			rbody.velocity = new Vector2( drillSpeed * moveDir, rbody.velocity.y);
			//other trigger box
		}
		
	}

	void OnBecameVisible()
	{
		if(currentBehaviorState == -1){
			currentBehaviorState = 1;
		}
	}

	Vector3 setFinalPosition(){
		var finalPosition = new Vector3 (mainCamera.transform.position.x + (xOffset * -moveDir), flightHeight, this.transform.localPosition.z);
		return finalPosition;
	}

	private void MoveToPos() {

		var origin = this.transform.position;
		var end = setFinalPosition();

		if(Vector3.Distance(origin, end) > .1f) {
			Vector3 direction = (end - origin).normalized;
			rbody.MovePosition(transform.position + direction * (flySpeed * Time.fixedDeltaTime));
		}else{
			//currentBehaviorState = 0;
			Invoke("setDrillBehavior", 0.9f);
		}
	}

	void setDrillBehavior(){
		currentBehaviorState = 2;
		this.transform.parent = null;
		anim.SetBool("Attack", true);
	}
	
}
