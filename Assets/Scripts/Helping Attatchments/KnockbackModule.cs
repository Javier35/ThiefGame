using UnityEngine;
using System.Collections;

public class KnockbackModule : MonoBehaviour {

	//private Rigidbody2D rbody;
	[SerializeField]private float upForce = 70;
	[SerializeField]private float sideForce = 50;

	private Rigidbody2D rbody;

	// Use this for initialization
	void Start () {
		rbody = GetComponent<Rigidbody2D> ();
	}

	public void Knockback(float knockDir){
		rbody.AddForce (new Vector2 (sideForce * knockDir, upForce));
	}
}
