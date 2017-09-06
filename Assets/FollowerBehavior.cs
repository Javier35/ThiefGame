using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerBehavior : MonoBehaviour {


	//Target components
	List<Vector3> positionsList = new List<Vector3>();
	List<string> animationNamesList = new List<string>();


	public GameObject targetObject;
	private Transform targetTransform;
	private Animator targetAnimator;


	//My components
	AnimationNameTranslator hashTranslator;
	public float nonFollowDistance = 1f;
	public float followSpeed = 2.4f;
	bool followEnabled = false;
	private Animator anim;


	void Start () {
		targetTransform = targetObject.GetComponent<Transform> ();
		targetAnimator = targetObject.GetComponent<Animator> ();

		hashTranslator = GetComponent<AnimationNameTranslator> ();
		anim = GetComponent<Animator> ();

		Invoke ("EnableFollow", 0.48f);
	}

	void Update () {

		if (followEnabled) {
			transform.position = Vector3.Lerp (transform.position, positionsList[0], followSpeed);
			positionsList.RemoveAt (0);

			anim.Play (animationNamesList[0]);
			animationNamesList.RemoveAt (0);

		}
		positionsList.Add (targetTransform.position);


		var currentAnimHash = targetAnimator.GetCurrentAnimatorStateInfo (0).shortNameHash;
		var currentAnimName = hashTranslator.getNameByHash (currentAnimHash);
		animationNamesList.Add (currentAnimName);
	}

	void EnableFollow(){
		followEnabled = true;
	}
	void DisableFollow(){
		followEnabled = false;
	}
}
