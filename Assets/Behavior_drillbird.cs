using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_drillbird : Movable {
	
	bool startBehavior = false;
	float startSpeed = 2;
	float flightDistanceY;
	Vector3 finalPosition;
	Camera mainCamera;

	void Start()
	{
		mainCamera = Camera.main;
		
		RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector2.down, 100f, 1 << LayerMask.NameToLayer("Platform"));
		if (hit.collider != null) {
            flightDistanceY = hit.distance * 0.6f;
        }else{
			flightDistanceY =  2f * mainCamera.orthographicSize * 0.7f;
		}
	}

	float duration;

	void Update()
	{
		if(checkIfActive() && !startBehavior){
			startBehavior = true;
			this.transform.parent = mainCamera.transform;
			finalPosition = new Vector3 (this.transform.localPosition.x - 1.4f, this.transform.localPosition.y - flightDistanceY, this.transform.localPosition.z);
		}

		if(startBehavior){
			MoveTowardsTargetInLocalSpace(finalPosition);
		}
	}

	private void MoveTowardsTargetInLocalSpace(Vector3 targetPosition) {

		Vector3 currentPosition = this.transform.localPosition;
		
		if(Vector3.Distance(currentPosition, targetPosition) > .1f) { 
			Vector3 directionOfTravel = targetPosition - currentPosition;
			directionOfTravel.Normalize();

			this.transform.Translate(
				(directionOfTravel.x * startSpeed * Time.deltaTime),
				(directionOfTravel.y * startSpeed * Time.deltaTime),
				(directionOfTravel.z * startSpeed * Time.deltaTime),
				Space.Self);
		}
	}
	
}
