using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerBehavior : MonoBehaviour {


	//Target components
	List<Vector3> positionsList = new List<Vector3>();
	List<string> animationNamesList = new List<string>();
	List<bool> facingRightList = new List<bool>();
	List<bool> targetAirborneList = new List<bool>();

	[SerializeField] private GameObject targetObject;
	[SerializeField] private GameObject AssistPoof;
	private PlatformerCharacter2D targetVariables;
	private Transform targetTransform;
	private Animator targetAnimator;
	private SpriteRenderer spriteRenderer;

	//My components
	[SerializeField] private LayerMask whatIsGround;
	AnimationNameTranslator hashTranslator;
	public float nonFollowDistance = 1f;
	public float followSpeed = 2.4f;
	bool followEnabled = false;
	public bool facingRight = true;
	private Animator anim;
	Transform groundChecker;


	void Start () {
		targetTransform = targetObject.GetComponent<Transform> ();
		targetAnimator = targetObject.GetComponent<Animator> ();
		targetVariables = targetObject.GetComponent<PlatformerCharacter2D> ();

		spriteRenderer = GetComponent<SpriteRenderer>();
		hashTranslator = GetComponent<AnimationNameTranslator> ();
		anim = GetComponent<Animator> ();
		groundChecker = this.transform.Find ("GroundChecker");

		Invoke ("EnableFollow", 0.43f);
	}

	void Update () {

		addToLists ();

		if (followEnabled) {

			if(anim.GetBool("Assist") || anim.GetCurrentAnimatorStateInfo(0).IsName("Assist")){
				AssistBehavior();
				spriteRenderer.sortingOrder = 1;
			}else{
				spriteRenderer.sortingOrder = -1;
				var posToMoveTo = new Vector3 (positionsList [0].x, positionsList [0].y, positionsList [0].z + 0.1f);
				transform.position = Vector3.Lerp (transform.position, posToMoveTo, followSpeed);
				
				anim.Play (animationNamesList [0]);

				if (facingRightList[0] != facingRight)
					Flip ();
			}

			positionsList.RemoveAt (0);
			animationNamesList.RemoveAt (0);
			facingRightList.RemoveAt (0);
		}
	}

	Vector3 assistPosition;
	bool assistFacingRight;

	public void StartAssist(Vector3 position, bool facingRight){

		if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Assist")){
			anim.SetTrigger("Assist");
			assistPosition = position;
			assistFacingRight = facingRight;

			Instantiate(AssistPoof, positionsList [0], this.transform.rotation);
		}
	}

	void AssistBehavior(){
		transform.position = assistPosition;

		if (assistFacingRight != facingRight)
			Flip ();
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

	void addToLists(){

		var followerGrounded = CheckForFloor (groundChecker, 0.02f, whatIsGround);

		var currentAnimHash = targetAnimator.GetCurrentAnimatorStateInfo (0).shortNameHash;
		var currentAnimName = hashTranslator.getNameByHash (currentAnimHash);

		if (animationNamesList.Count > 0) {
			if (currentAnimName == "Idle" && followerGrounded) {
				animationNamesList.Insert (0, currentAnimName);
				positionsList.Insert (0, positionsList[0]);
				facingRightList.Insert (0, targetVariables.facingRight);
				targetAirborneList.Insert (0, targetVariables.grounded);
			} else {
				animationNamesList.Add (currentAnimName);
				positionsList.Add (targetTransform.position);
				facingRightList.Add (targetVariables.facingRight);
				targetAirborneList.Add (targetVariables.grounded);
			}
		} else {
			animationNamesList.Add (currentAnimName);
			positionsList.Add (targetTransform.position);
			facingRightList.Add (targetVariables.facingRight);
			targetAirborneList.Add (targetVariables.grounded);
		}
			
	}

	bool CheckForFloor (Transform transform, float areaSize, LayerMask layers){

		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, areaSize, layers);
		if (colliders.Length > 0)
			return true;
		return false;
	}
}
