using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour {

	public float paralaxSpeed = 1;

	private float backgroundSize;
	private Transform cameraTransform;
	private Transform[] layers;
	private float viewZone = 10;
	private int leftIndex;
	private int rightIndex;

	private float lastCameraX;

	private void Start(){
		backgroundSize = GetComponentInChildren<SpriteRenderer> ().bounds.size.x;
		cameraTransform = Camera.main.transform;
		lastCameraX = cameraTransform.position.x;
		layers = new Transform[transform.childCount];
		for (int i = 0; i < transform.childCount; i++)
			layers [i] = transform.GetChild (i);

		leftIndex = 0;
		rightIndex = layers.Length - 1;
	}

	private void LateUpdate(){
		
		float deltaX = cameraTransform.position.x - lastCameraX;
		transform.position = new Vector3 (transform.position.x + (deltaX * paralaxSpeed), transform.position.y, transform.position.z);
		lastCameraX = cameraTransform.position.x;

		if(cameraTransform.position.x < (layers[leftIndex].transform.position.x + viewZone))
			ScrollLeft();

		if(cameraTransform.position.x > (layers[rightIndex].transform.position.x - viewZone))
			ScrollRight();
	}

	private void ScrollLeft(){
		layers [rightIndex].position = new Vector3 (layers [leftIndex].position.x - backgroundSize, transform.position.y, transform.position.z);
		leftIndex = rightIndex;
		rightIndex--;
		if (rightIndex < 0)
			rightIndex = layers.Length - 1;
	}

	private void ScrollRight(){
		layers [leftIndex].position = new Vector3 (layers [rightIndex].position.x + backgroundSize, transform.position.y, transform.position.z);
		rightIndex = leftIndex;
		leftIndex++;
		if (leftIndex == layers.Length)
			leftIndex = 0;
	}
}
