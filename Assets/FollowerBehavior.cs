using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerBehavior : MonoBehaviour {


	//Target components
	List<Vector3> positionsList = new List<Vector3>();
	List<string> animationNamesList = new List<string>();
	List<bool> facingRightList = new List<bool>();
	List<bool> targetAirborneList = new List<bool>();

	public GameObject targetObject;
	private PlatformerCharacter2D targetVariables;
	private Transform targetTransform;
	private Animator targetAnimator;


	//My components
	AnimationNameTranslator hashTranslator;
	public float nonFollowDistance = 1f;
	public float followSpeed = 2.4f;
	bool followEnabled = false;
	bool facingRight = true;
	private Animator anim;


	void Start () {
		targetTransform = targetObject.GetComponent<Transform> ();
		targetAnimator = targetObject.GetComponent<Animator> ();
		targetVariables = targetObject.GetComponent<PlatformerCharacter2D> ();

		hashTranslator = GetComponent<AnimationNameTranslator> ();
		anim = GetComponent<Animator> ();

		Invoke ("EnableFollow", 0.43f);
	}

	void Update () {


		positionsList.Add (targetTransform.position);
		facingRightList.Add (targetVariables.facingRight);
		targetAirborneList.Add (targetVariables.grounded);

		var currentAnimHash = targetAnimator.GetCurrentAnimatorStateInfo (0).shortNameHash;
		var currentAnimName = hashTranslator.getNameByHash (currentAnimHash);
		animationNamesList.Add (currentAnimName);

		if (followEnabled) {

//			var distanceInX = Vector3.Distance(transform.position, new Vector3(targetTransform.position.x, transform.position.y, transform.position.z));
//
//			if (targetVariables.grounded && distanceInX <= nonFollowDistance) {
//				anim.Play ("Idle");
//			} else {
//
//				var positionToBe = positionsList [0];
//
//				var distanceToBeX = Vector3.Distance(targetTransform.position, new Vector3(positionToBe.x, targetTransform.position.y, targetTransform.position.z));
//				if(distanceToBeX < nonFollowDistance){
//					var behindPlayerPosition = positionToBe.x + (-targetVariables.faceDir * nonFollowDistance); 
//					positionToBe = new Vector3 (behindPlayerPosition, positionToBe.y, positionToBe.z);
//				}
//
//				transform.position = Vector3.Lerp (transform.position, positionToBe, followSpeed);
//				anim.Play (animationNamesList [0]);
//			}

			transform.position = Vector3.Lerp (transform.position, positionsList [0], followSpeed);
			anim.Play (animationNamesList [0]);

			if (facingRightList[0] != facingRight)
				Flip ();

			positionsList.RemoveAt (0);
			animationNamesList.RemoveAt (0);
			facingRightList.RemoveAt (0);
		}


	}

	void EnableFollow(){
		followEnabled = true;
	}
	void DisableFollow(){
		followEnabled = false;
	}

	private void Flip()
	{
		Vector3 theScale = this.transform.localScale;
		theScale.x *= -1;
		this.transform.localScale = theScale;
		facingRight = !facingRight;
	}
}
