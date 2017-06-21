using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadzoneFollow : PlayerFollow {

	public GameObject currentLevel;
	public float yMovementThreshold = 1.4f;
	float characterDeltaY;
	private Vector3 minBounds;
	private Vector3 maxBounds;

	void Start(){
		SetCameraBounds ();
		moveToPlayer ();
	}

	void Update() {

		if (allowFollow) {
			deadzoneFollow();
		}
			
	}

	private void deadzoneFollow(){
		
		characterDeltaY = character.transform.position.y - transform.position.y;
		var clampedX = Mathf.Clamp(character.transform.position.x, minBounds.x, maxBounds.x);

		if (characterDeltaY > yMovementThreshold) {
			transform.position = new Vector3 (clampedX, character.transform.position.y - yMovementThreshold, transform.position.z);
		} else if (characterDeltaY < -yMovementThreshold) {
			transform.position = new Vector3 (clampedX, character.transform.position.y + yMovementThreshold, transform.position.z);
		} else {
			transform.position = new Vector3 (clampedX, transform.position.y, transform.position.z);
		}
	}

	override public void moveToPlayer(){
		transform.position = new Vector3 (character.transform.position.x, character.transform.position.y + yMovementThreshold, transform.position.z);
	}

	private void SetCameraBounds(){
		Bounds bounds = new Bounds(currentLevel.transform.position, Vector3.zero);

		foreach(Renderer renderer in currentLevel.GetComponentsInChildren<Renderer>())
		{
			bounds.Encapsulate(renderer.bounds);
		}

		minBounds = GetVertexWorldPosition (bounds.min, currentLevel.transform);
		maxBounds = GetVertexWorldPosition (bounds.max, currentLevel.transform);

		float height = 2f * Camera.main.orthographicSize;
		float width = height * Camera.main.aspect;

		minBounds = new Vector3 (minBounds.x + width/2, minBounds.y + height/2);
		maxBounds = new Vector3 (maxBounds.x - width/2, maxBounds.y);
	}

	public Vector3 GetVertexWorldPosition(Vector3 vertex, Transform owner)
	{
		return owner.localToWorldMatrix.MultiplyPoint3x4(vertex);
	}
}﻿ 