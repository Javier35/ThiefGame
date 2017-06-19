using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadzoneFollow : MonoBehaviour {

	[SerializeField] GameObject character;
	Rigidbody2D rbody;
	private Vector3 moveTemp;

	public float xMovementThreshold = 0.4f;
	public float yMovementThreshold = 1.4f;

	float characterDeltaX;
	float characterDeltaY;
	float difX = 0.0f;
	float difY = 0.0f;

	float lastVelX = 0;
	float lastVelY = 0;

	bool allowFollow = true;

	void Awake(){
		rbody = character.GetComponent<Rigidbody2D> ();
		moveOntoPlayer ();
	}

	void Update() {

		if (allowFollow) {
			deadzoneFollow();
		}
			
	}

	private void deadzoneFollow(){
		
		characterDeltaX = character.transform.position.x - transform.position.x;
		characterDeltaY = character.transform.position.y - transform.position.y;

//		if (Mathf.Abs (characterDeltaX) > xMovementThreshold) {
//			var velX = rbody.velocity.x;
//			if (velX == 0) {
//				difX = characterDeltaX - xMovementThreshold;
//				velX = lastVelX;
//			} else {
//				difX = characterDeltaX + (Mathf.Sign (velX) * -1 * xMovementThreshold);
//			}
//
//			var targerPos = new Vector3 (
//				transform.position.x + difX,
//				transform.position.y,
//				transform.position.z
//			);
//
//			lastVelX = velX;
//			transform.position = Vector3.MoveTowards (transform.position, targerPos, Mathf.Abs(velX) * Time.deltaTime);
//		}

		transform.position = new Vector3 (character.transform.position.x, transform.position.y, transform.position.z);

		if (Mathf.Abs (characterDeltaY) > yMovementThreshold - 0.4f) {
			var velY = rbody.velocity.y;
			if (velY == 0) {
				difY = characterDeltaY - yMovementThreshold - 0.4f; 
				velY = lastVelY;
			} else {
				difY = characterDeltaY + (Mathf.Sign (velY) * -1 * (yMovementThreshold - 0.4f));
			}

			var targerPos = new Vector3 (
				transform.position.x,
				transform.position.y + (difY),
				transform.position.z
			);

			lastVelY = velY;
			transform.position = Vector3.MoveTowards (transform.position, targerPos, (Mathf.Abs(velY)) * Time.deltaTime);
		}
	}

	public void StopFollowing(){
		allowFollow = false;
	}
	public void StartFollowing(){
		allowFollow = true;
		moveOntoPlayer ();
	}

	private void moveOntoPlayer(){
		transform.position = new Vector3 (character.transform.position.x, character.transform.position.y + yMovementThreshold, transform.position.z);
	}
}﻿ 