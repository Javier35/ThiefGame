using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopMessageBehavior : MonoBehaviour {

	void Start () {
		Invoke ("DestroySelf", 1.2f);
	}
	
	void Update() {
		transform.Translate(Vector3.up * Time.deltaTime, Space.World);
	}

	void DestroySelf(){
		this.gameObject.SetActive (false);
	}
}
