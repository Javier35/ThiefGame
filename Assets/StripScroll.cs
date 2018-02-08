using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StripScroll : MonoBehaviour {


	public float parallaxSpeed = 1;

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
	
	// Update is called once per frame
	void LateUpdate () {
		float deltaX = cameraTransform.position.x - lastCameraX;
		transform.position += Vector3.right * (deltaX * parallaxSpeed);
		lastCameraX = cameraTransform.position.x;

		if(cameraTransform.position.x < (layers[leftIndex].transform.position.x + viewZone))
			ScrollLeft();
		if(cameraTransform.position.x > (layers[rightIndex].transform.position.x - viewZone))
			ScrollRight();
	}

	void ScrollLeft(){
		// int lastRight = rightIndex;
		layers[rightIndex].position = Vector3.right * (layers[leftIndex].position.x - backgroundSize);
		leftIndex = rightIndex;
		rightIndex--;
		if(rightIndex < 0)
			rightIndex = layers.Length - 1;
	}

	void ScrollRight(){
		// int lastRight = leftIndex;
		layers[leftIndex].position = Vector3.right * (layers[leftIndex].position.x + backgroundSize);
		rightIndex = leftIndex;
		leftIndex++;
		if(leftIndex == layers.Length)
			leftIndex = 0;
	}
}
