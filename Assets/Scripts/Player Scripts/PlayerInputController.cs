using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof (PlatformerCharacter2D))]
public class PlayerInputController : MonoBehaviour
{
	private PlatformerCharacter2D m_Character;
	private bool m_Jump;
	private bool m_Dash;

	private bool dashEnabled = true;
	private bool pushDashFlag = true;
	float dashTimer = 0;

	private Rigidbody2D rbody;

	Animator armAnimator;

	bool crouch;
	float inputAxis;

	private void Awake()
	{
		m_Character = GetComponent<PlatformerCharacter2D>();
	}


	private void Update()
	{
		armAnimator = transform.Find ("Arm").GetComponent<Animator>();

		InterpreteKeys ();
		SetDashingValue ();
		SetAttackAnimation ();

		inputAxis = Input.GetAxisRaw ("Horizontal");

		m_Character.Move(inputAxis, crouch, m_Jump, m_Dash);
		m_Jump = false;
	}

	private void InterpreteKeys () {


		if (!m_Jump)
		{
			if (Input.GetKeyDown (KeyCode.Z) || Input.GetKeyDown (KeyCode.Space))
				m_Jump = true;
			else
				m_Jump = false;
		}

		if(Input.GetKeyDown(KeyCode.R))
			SceneManager.LoadScene("AlphaLayout");

		if (m_Character.animator.GetBool ("InGround") && Input.GetAxisRaw ("Horizontal") != 0)
			m_Character.animator.SetBool ("Run", true);
		else
			m_Character.animator.SetBool ("Run", false);
	}

	private void SetDashingValue(){

		if (!pushDashFlag && dashEnabled && Input.GetKeyDown (KeyCode.C)) {
			pushDashFlag = true;		
		}

		if (pushDashFlag && Input.GetKey (KeyCode.C)) {

			if(	!m_Character.m_Grounded ||
			(m_Character.m_FacingRight && Input.GetAxisRaw ("Horizontal") == -1) ||
			(!m_Character.m_FacingRight && Input.GetAxisRaw ("Horizontal") == 1) ||
			(dashTimer >= 0.3 )	){

				StopDashing (0.15f);
				return;
			}

			m_Character.animator.SetBool ("Dash", true);
			m_Dash = true;
			dashTimer += Time.deltaTime;

		} else if(pushDashFlag && Input.GetKeyUp (KeyCode.C)){

			StopDashing (0.10f);
			return;

		} else {
			
			m_Character.animator.SetBool ("Dash", false);
			m_Dash = false;
			dashTimer = 0;
		}
	}

	void SetAttackAnimation(){

		if (Input.GetKeyDown (KeyCode.X) && 
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

	void StopDashing(float dashAgainDelay){
		dashTimer = 0;
		m_Character.animator.SetBool ("Dash", false);
		m_Dash = false;
		dashEnabled = false;
		pushDashFlag = false;
		Invoke ("ToggleDashEnabled", dashAgainDelay);
	}

	void ToggleDashEnabled(){
		dashEnabled = !dashEnabled;
	}
		
}