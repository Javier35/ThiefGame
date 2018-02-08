using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof (PlatformerCharacter2D))]
public class PlayerInputController : MonoBehaviour
{
	private PlatformerCharacter2D m_Character;
	private bool jump;
	private bool jumpHold;
	private bool m_Dash;

	private bool dashEnabled = true;
	private bool pushDashFlag = true;
	float dashTimer = 0;

	Animator armAnimator;
	float inputAxis;

	public FollowerBehavior currentFollower;

	private void Awake()
	{
		m_Character = GetComponent<PlatformerCharacter2D>();
		armAnimator = transform.Find ("Arm").GetComponent<Animator>();
	}


	private void Update()
	{
		InterpreteKeys ();
		SetDashingValue ();
		SetAttackAnimation ();
		inputAxis = Input.GetAxisRaw ("Horizontal");
	}

	void FixedUpdate(){
		m_Character.Move(inputAxis, m_Dash);
		m_Character.GravityJump(jump, jumpHold);
		jump = false;
		jumpHold = false;
	}

	private void InterpreteKeys () {

		if(Input.GetKeyDown (KeyCode.R))
			jump = true;
		else if(Input.GetKey (KeyCode.R))
			jumpHold = true;

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

		if (!pushDashFlag && dashEnabled && Input.GetKeyDown (KeyCode.Q)) {
			pushDashFlag = true;		
		}

		if (pushDashFlag && Input.GetKey (KeyCode.Q)) {

			if(	!m_Character.grounded ||
				// (m_Character.facingRight && Input.GetAxisRaw ("Horizontal") == -1) ||
				// (!m_Character.facingRight && Input.GetAxisRaw ("Horizontal") == 1) ||
				(dashTimer >= 0.3 )	){

				StopDashing (dashCooldown);
				return;
			}

			m_Character.animator.SetBool ("Dash", true);
			m_Dash = true;
			dashTimer += Time.deltaTime;

		} else if(pushDashFlag && Input.GetKeyUp (KeyCode.Q)){

			StopDashing (dashCooldown);
			
		} else {
			
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
		
}