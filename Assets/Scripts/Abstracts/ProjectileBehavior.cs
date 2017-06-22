using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileBehavior : MonoBehaviour {

	//direction in 1 means right, -1 means left
	protected float direction = 1;
	[SerializeField] protected float speed = 1.0f;
	[SerializeField] protected float lifetime = 1.5f;

	protected Rigidbody2D rbody;

	void Awake(){
		rbody = GetComponent<Rigidbody2D> ();
	}

	virtual protected void DeactivateSelf(){
		this.gameObject.SetActive (false);
	}

	public void setDirection(float dir){

		Invoke ("DeactivateSelf", lifetime);
		if (direction != dir) {
			Flip ();
			direction = dir;
		}
	}

	void Flip(){
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
