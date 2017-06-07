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

	bool crouch;
	float inputAxis;

	private void Awake()
	{
		m_Character = GetComponent<PlatformerCharacter2D>();
	}


	private void Update()
	{
		InterpreteKeys ();
		SetDashingValue ();
		inputAxis = Input.GetAxisRaw ("Horizontal");

		m_Character.Move(inputAxis, crouch, m_Jump, m_Dash);
		m_Jump = false;
	}

	private void InterpreteKeys () {


		if (!m_Jump)
		{
			// Read the jump input in Update so button presses aren't missed.
			if (Input.GetKeyDown (KeyCode.Z) || Input.GetKeyDown (KeyCode.Space))
				m_Jump = true;
			else
				m_Jump = false;
		}

		if(Input.GetKeyDown(KeyCode.R))
			//SceneManagement.Scene
			SceneManager.LoadScene("AlphaLayout");

		if (Input.GetKeyDown (KeyCode.X)) {
//			m_Character.animator.SetTrigger ("Attack");
//			m_Character.animator.SetBool ("Run", false);
		}

//		if (Input.GetKeyUp (KeyCode.DownArrow)) {
//			m_Character.animator.SetBool ("Crouch", false);
//		}

		if (m_Character.animator.GetBool ("InGround") && Input.GetAxisRaw ("Horizontal") != 0)
			m_Character.animator.SetBool ("Run", true);
		else
			m_Character.animator.SetBool ("Run", false);

//		//if it is moving forward while jumping, switch the layer to display the forward jumping animation
//		if (Input.GetAxisRaw ("Horizontal") != 0 && !m_Character.animator.GetBool ("InGround")) {
//			m_Character.animator.SetLayerWeight (1, 1);
//		} else {
//			m_Character.animator.SetLayerWeight (1, 0);
//		}
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

				StopDashing (0.3f);
				return;
			}

			m_Character.animator.SetBool ("Dash", true);
			m_Dash = true;
			dashTimer += Time.deltaTime;

		} else if(pushDashFlag && Input.GetKeyUp (KeyCode.C)){

			StopDashing (0.14f);
			return;

		} else {
			
			m_Character.animator.SetBool ("Dash", false);
			m_Dash = false;
			dashTimer = 0;
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