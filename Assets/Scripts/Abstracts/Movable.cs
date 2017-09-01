using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movable : MonoBehaviour {

	protected SpriteRenderer renderer;
	protected Rigidbody2D rbody;
	protected Animator animator;
	protected GameObject player;
	protected Vector3 spawnPosition;
	protected bool visible = false;
	protected bool near = false;
	protected float currentBehaviorState = -1;

	public string activationCondition = "visible";

	[SerializeField] public bool faceLeft = true;
	[HideInInspector]public bool originalFaceLeft;

	void Awake () {

		renderer = GetComponent<SpriteRenderer>();
		rbody = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();
		spawnPosition = gameObject.transform.position;

		if (!faceLeft) {
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}
		originalFaceLeft = faceLeft;
	}

	public void ResetPosition(){
		
		if(faceLeft != originalFaceLeft){
			Flip ();
			faceLeft = originalFaceLeft;
		}
		gameObject.transform.position = spawnPosition;
		currentBehaviorState = -1;
		visible = false;
	}

	public void Flip () {

		faceLeft = !faceLeft;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void OnBecameVisible() {
		visible = true;
	}

	void OnBecomeInvisible() {
		visible = false;
	}

	public void SetIsNear(bool isNear){
		near = isNear;
	}

	protected bool checkIfActive(){
		if (
			(activationCondition == "visible" && renderer.isVisible) ||
			(activationCondition == "proximity" && near) ||
			activationCondition == "always") {
			return true;
		}
		return false;
	}
}
