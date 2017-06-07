using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToCamera : MonoBehaviour {
	
	void LateUpdate(){
		var cameraPos = Camera.main.transform.position;
		this.transform.position = new Vector3 (cameraPos.x, cameraPos.y, this.transform.position.z);
	}
}
