using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof (PlatformerCharacter2D))]
public class PlayerInputController : MonoBehaviour
{
	[SerializeField] private LayerMask whatIsGround;
	private PlatformerCharacter2D m_Character;
	private bool jump;
	private bool jumpHold;
	private bool m_Dash;
	private bool crouch;

	private bool dashEnabled = true;
	private bool pushDashFlag = true;
	float dashTimer = 0;

	Animator armAnimator;
	float inputAxis;
	Transform ceilingCheck;

	public FollowerBehavior currentFollower;

	private void Awake()
	{
		ceilingCheck = transform.Find("CeilingCheck");
		m_Character = GetComponent<PlatformerCharacter2D>();
		armAnimator = transform.Find ("Arm").GetComponent<Animator>();
	}


	private void Update()
	{
		InterpreteKeys ();

		if(Input.GetAxisRaw ("Vertical") == -1)
			crouch = true;
		else
			crouch = false;

		SetDashingValue ();
		SetAttackAnimation ();
		inputAxis = Input.GetAxisRaw ("Horizontal");
	}

	void FixedUpdate(){
		m_Character.Move(inputAxis, m_Dash, crouch);
		m_Character.GravityJump(jump, jumpHold);
		jump = false;
	}

	private void InterpreteKeys () {

		if(Input.GetKeyDown (KeyCode.R))
			jump = true;

		if(Input.GetKey (KeyCode.R)){
			jumpHold = true;
		}else{
			jumpHold = false;
		}
			

		if(Input.GetKeyDown(KeyCode.O))
			SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex ) ;

		if(Input.GetKeyDown(KeyCode.W))
			currentFollower.StartAssist(this.transform.position, m_Character.facingRight);

		if (m_Character.animator.GetBool ("InGround") && Input.GetAxisRaw ("Horizontal") != 0)
			m_Character.animator.SetBool ("Run", true);
		else
			m_Character.animator.SetBool ("Run", false);
	}

	float dashCooldown = 0.05f;
	private void SetDashingValue(){

		if (!pushDashFlag && dashEnabled && Input.GetKeyDown (KeyCode.Q))
			pushDashFlag = true;		

		var underCeilng = CheckInTransformArea (ceilingCheck, .4f, whatIsGround);

		if (pushDashFlag && Input.GetKey (KeyCode.Q)) {
			if(	!m_Character.grounded || (dashTimer >= 0.3 && !underCeilng)	){
				StopDashing (dashCooldown);
				return;
			}
			m_Character.animator.SetBool ("Dash", true);
			m_Dash = true;
			dashTimer += Time.deltaTime;

		} else if(pushDashFlag && Input.GetKeyUp (KeyCode.Q) && !underCeilng){
			StopDashing (dashCooldown);
		} else if (!underCeilng) {
			m_Character.animator.SetBool ("Dash", false);
			m_Dash = false;
			dashTimer = 0;
		}
	}

	void SetAttackAnimation(){

		if (Input.GetKeyDown (KeyCode.E) && 
			!m_Character.animator.GetCurrentAnimatorStateInfo (0).IsName ("Damage") &&
			!m_Character.animator.GetCurrentAnimatorStateInfo (0).IsName ("Death")) {

			armAnimator.SetTrigger ("Swing");
		}

		if (m_Character.animator.GetCurrentAnimatorStateInfo (0).IsName ("Damage") &&
			m_Character.animator.GetCurrentAnimatorStateInfo (0).IsName ("Death")) {

			armAnimator.CrossFade ("Invisible", 0);
			armAnimator.ResetTrigger ("Swing");
		}

		if (armAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Swing")) {
			m_Character.animator.SetLayerWeight (1, 1);
		} else {
			m_Character.animator.SetLayerWeight (1, 0);
		}
	}

	void StopDashing(float cooldown){
		dashTimer = 0;
		m_Character.animator.SetBool ("Dash", false);
		m_Dash = false;
		dashEnabled = false;
		pushDashFlag = false;
		Invoke ("ToggleDashEnabled", cooldown);
	}

	void ToggleDashEnabled(){
		dashEnabled = !dashEnabled;
	}

	protected bool CheckInTransformArea (Transform transform, float areaSize, LayerMask layers){
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, areaSize, layers);
		if (colliders.Length > 0)
			return true;
		return false;
	}
		
}