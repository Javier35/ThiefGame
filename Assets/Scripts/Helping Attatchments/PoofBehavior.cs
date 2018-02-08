using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoofBehavior : MonoBehaviour {

	public void DestroySelf(){
		Destroy (this.gameObject);
	}

	public void DeactivateSelf(){
		this.gameObject.SetActive(false);
	}
}
