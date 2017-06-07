using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movable : MonoBehaviour {


	protected Rigidbody2D rbody;
	protected Animator animator;
	protected GameObject player;
	protected Vector3 spawnPosition;
	protected bool visible = false;
	protected bool near = false;

	public string activationCondition = "visible";

	[SerializeField] public bool faceLeft = true;
	[HideInInspector]public bool originalFaceLeft;

	void Awake () {

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
		if (activationCondition == "visible" && visible ||
			activationCondition == "proximity" && near ||
			activationCondition == "always") {
			return true;
		}
		return false;
	}
}
