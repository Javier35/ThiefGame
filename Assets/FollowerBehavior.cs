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

		Invoke ("EnableFollow", 0.48f);
	}

	void Update () {

		if (followEnabled) {


			Vector2 difference = transform.position - positionsList[0];
			var distanceInX = Mathf.Abs(difference.x);

			transform.position = Vector3.Lerp (transform.position, positionsList[0], followSpeed);
			anim.Play (animationNamesList[0]);

			if (facingRightList[0] != facingRight)
				Flip ();

			positionsList.RemoveAt (0);
			animationNamesList.RemoveAt (0);
			facingRightList.RemoveAt (0);
		}

		positionsList.Add (targetTransform.position);
		facingRightList.Add (targetVariables.m_FacingRight);
		targetAirborneList.Add (targetVariables.m_Grounded);

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

	private void Flip()
	{
		Vector3 theScale = this.transform.localScale;
		theScale.x *= -1;
		this.transform.localScale = theScale;
		facingRight = !facingRight;
	}
}
