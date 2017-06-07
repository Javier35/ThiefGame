using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidersManager : MonoBehaviour {

	// Use this for initialization
	public void DisableAllColliders(){
		foreach (Collider2D tempcollider in gameObject.GetComponents<Collider2D> ()) {
			tempcollider.enabled = false;
		}
	}

	public void EnableAllColliders(){
		foreach (Collider2D tempcollider in gameObject.GetComponents<Collider2D> ()) {
			tempcollider.enabled = true;
		}
	}
}
