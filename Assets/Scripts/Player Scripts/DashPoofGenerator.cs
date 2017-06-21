using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPoofGenerator : MonoBehaviour {

	[SerializeField] GameObject poofObject;
	private bool liftDashDust = true;
	Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponentInParent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (liftDashDust && anim.GetCurrentAnimatorStateInfo(0).IsName("Dash")) {
			liftDashDust = false;
			Invoke ("EnableDashDust", 0.07f);
			Instantiate (poofObject, this.transform.position, this.transform.rotation);
		}
	}

	private void EnableDashDust(){
		liftDashDust = true;
	}
}
