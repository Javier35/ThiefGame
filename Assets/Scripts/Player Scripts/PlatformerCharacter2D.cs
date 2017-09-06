using System;
using UnityEngine;
using System.Collections;

public class PlatformerCharacter2D : MonoBehaviour
{
	[SerializeField] private float m_MaxSpeed = 3.9f;                    // The fastest the player can travel in the x axis.
	private float originalMaxSpeed;

    public float m_JumpForce = 95f;                  // Amount of force added when the player jumps.
	//private float originalJumpForce;

	[SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
	[SerializeField] private float m_KnockbackHeight = 300f;

//    private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    const float k_GroundedRadius = .15f; // Radius of the overlap circle to determine if grounded
	[HideInInspector] public bool m_Grounded;            // Whether or not the player is grounded.

    private Transform m_CeilingCheck;   // A position marking where to check for ceilings
	private Transform m_EdgeCheck;   // A position marking where to check for ceilings

    const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
	[HideInInspector]public Animator animator;            // Reference to the player's animator component.
    private Rigidbody2D m_Rigidbody2D;
	[HideInInspector] public bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private bool m_Damaged = false;
//	private bool jumpLock = false;
	private GroundChecker groundchecker;
	// private SpecialTerrainChecker terrainChecker;
	[SerializeField]float dashMove = 1.7f;

    private void Awake()
    {
        // Setting up references.
//        m_GroundCheck = transform.Find("GroundCheck");
        m_CeilingCheck = transform.Find("CeilingCheck");
        animator = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
		originalMaxSpeed = m_MaxSpeed;
		groundchecker = GetComponentInChildren<GroundChecker>();
		// terrainChecker = GetComponentInChildren<SpecialTerrainChecker>();
    }
		
    private void LateUpdate()
    {
		m_Grounded = groundchecker.grounded;
		animator.SetFloat ("Yspeed", m_Rigidbody2D.velocity.y);
		animator.SetBool("InGround", m_Grounded);
		// animator.SetBool ("OnEdge", groundchecker.teetering);
    }


	public void Move(float move, bool dash){

        if (animator.GetBool("InGround") && move != 0)
            animator.SetBool("Run", true);

        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {
			MovementBehavior (move, dash);
        }
    }

	bool lastDash = false;
	void MovementBehavior(float move, bool dash){

		if (lastDash == true && dash == false) {
			var underCeilng = CheckInTransformArea (m_CeilingCheck, .1f, m_WhatIsGround);

			if (underCeilng) {
				dash = true;
				animator.SetBool ("Dash", true);
			}
		}

		if (dash) {
			if(move != 0){
				move = move > 0 ? dashMove : -dashMove;
			}else{
				move = m_FacingRight ? dashMove : -dashMove;
			}
		}

		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Damage") || animator.GetCurrentAnimatorStateInfo (0).IsName ("Death")) {
			move = 0;
		}

		lastDash = dash;

		SetPlayerVelocityX (move);
		FlipToFaceVelocity(move);
		// KnockBackWhileDamaged ();
	}

	private void FlipToFaceVelocity(float move){
		if ((move > 0 && !m_FacingRight) || (move < 0 && m_FacingRight))
		{
			// ... flip the player.
			Flip();
		}
	}

	private void SetPlayerVelocityX(float move){
		// Move the character
		m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);
		m_MaxSpeed = originalMaxSpeed;
	}

	private void KnockBackWhileDamaged(){
		//move back while damaged
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Damage")) { 
			if (m_FacingRight) {
				m_Rigidbody2D.velocity = new Vector2 (-originalMaxSpeed, m_Rigidbody2D.velocity.y);
			} else {
				m_Rigidbody2D.velocity = new Vector2(originalMaxSpeed, m_Rigidbody2D.velocity.y);
			}
		}
	}

	private void KnockUpForce(){
		if(m_Damaged){
			m_Damaged = false;
			m_Rigidbody2D.AddForce (new Vector2 (0f, m_KnockbackHeight));
		}
	}

	float initialJumpForce = 120;
	float additiveJumpForce = 70;
	float jumpTimer = 0;
	float totalJumpTimer = 0;
	
	public void JumpBehavior(bool jump, bool afterJump){

		if( (totalJumpTimer > 0 && afterJump) ||
		(totalJumpTimer > 0 && totalJumpTimer < 0.1 && !afterJump)){
			totalJumpTimer += Time.deltaTime;
			jumpTimer += Time.deltaTime;
		}else{
			totalJumpTimer = 0;
			jumpTimer = 0f;
		}

		if(jumpTimer > 0.05f){
			DoJump(additiveJumpForce);
			jumpTimer = 0f;
		}

		if(totalJumpTimer >= 0.15){
			totalJumpTimer = 0;
			jumpTimer = 0;
		}
		
		if (m_Grounded && jump && animator.GetBool ("InGround")
			&& !animator.GetCurrentAnimatorStateInfo(0).IsName("Damage")
			&& !animator.GetCurrentAnimatorStateInfo(0).IsName("Death")) {

				DoJump (additiveJumpForce);
				totalJumpTimer += Time.deltaTime;
				jumpTimer += Time.deltaTime;

		} else if (!m_Grounded && !jump && !animator.GetBool ("InGround")
			&& !animator.GetCurrentAnimatorStateInfo(0).IsName("Damage")
			&& !animator.GetCurrentAnimatorStateInfo(0).IsName("Death")
			&& animator.GetFloat("Yspeed") < 0) {

				DoFall ();
		}

	}

	float maxJumpSpeed = 2.7f;
	public void DoJump(float jumpForce){
		// Add a vertical force to the player.
//		jumpLock = true;
//		Invoke ("UnlockJumping", 0.1f);

		m_Grounded = false;
		animator.SetBool ("InGround", false);
		animator.SetBool ("Jump", true);
		m_Rigidbody2D.AddForce (new Vector2 (0f, jumpForce));

		if(m_Rigidbody2D.velocity.y > maxJumpSpeed)
			m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, maxJumpSpeed);
	}

	public void DoFall(){
		m_Grounded = false;
		animator.SetBool ("InGround", false);
		animator.SetBool ("Fall", true);
	}

    private void Flip()
    {
        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
		m_FacingRight = !m_FacingRight;
    }

	public void WasDamaged(){
		m_Damaged = true;
	}

//	private void UnlockJumping(){
//		jumpLock = false;
//	}

	public float GetMaxSpeed(){
		return m_MaxSpeed;
	}

	public float GetOriginalMaxSpeed(){
		return originalMaxSpeed;
	}

	public void SetMaxSpeed(float newSpeed){
		m_MaxSpeed = newSpeed;
	}

	public void RestoreMaxSpeed(){
		m_MaxSpeed = originalMaxSpeed;
	}

	protected bool CheckInTransformArea (Transform transform, float areaSize, LayerMask layers){

		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, areaSize, layers);
		if (colliders.Length > 0)
			return true;
		return false;
	}
}

