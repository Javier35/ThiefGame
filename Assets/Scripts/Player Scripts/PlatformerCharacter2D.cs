using System;
using UnityEngine;
using System.Collections;

public class PlatformerCharacter2D : MonoBehaviour
{
    
	[HideInInspector] public bool grounded;
	[HideInInspector]public Animator animator;
	[HideInInspector] public bool facingRight = true;
	[HideInInspector] public float faceDir = 1;
	[HideInInspector] public Rigidbody2D rbody;

	[SerializeField] private bool airControl = true;
    [SerializeField] private LayerMask m_WhatIsGround;
	[SerializeField] private float m_KnockbackHeight = 300f;
	[SerializeField]float dashMove = 1.7f;
	[SerializeField] private float maxSpeed = 3.9f;

	private bool damaged = false;
	GroundChecker groundchecker;
	Transform m_CeilingCheck;
	Transform m_EdgeCheck;
	float originalMaxSpeed;
	const float k_GroundedRadius = .15f;
	public float m_JumpForce = 10f;

    private void Awake()
    {
        // Setting up references.
//        m_GroundCheck = transform.Find("GroundCheck");
        m_CeilingCheck = transform.Find("CeilingCheck");
        animator = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
		originalMaxSpeed = maxSpeed;
		groundchecker = GetComponentInChildren<GroundChecker>();
		// terrainChecker = GetComponentInChildren<SpecialTerrainChecker>();
    }
		
    void Update()
    {
		grounded = groundchecker.grounded;
		animator.SetFloat ("Yspeed", rbody.velocity.y);
		animator.SetBool("InGround", grounded);
		// animator.SetBool ("OnEdge", groundchecker.teetering);
    }


	public void Move(float move, bool dash){

        if (animator.GetBool("InGround") && move != 0)
            animator.SetBool("Run", true);

        //only control the player if grounded or airControl is turned on
        if (grounded || airControl)
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
				move = facingRight ? dashMove : -dashMove;
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
		if ((move > 0 && !facingRight) || (move < 0 && facingRight))
		{
			// ... flip the player.
			Flip();
		}
	}

	private void SetPlayerVelocityX(float move){
		// Move the character
		rbody.velocity = new Vector2(move * maxSpeed, rbody.velocity.y);
		maxSpeed = originalMaxSpeed;
	}

	private void KnockBackWhileDamaged(){
		//move back while damaged
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Damage")) { 
			if (facingRight) {
				rbody.velocity = new Vector2 (-originalMaxSpeed, rbody.velocity.y);
			} else {
				rbody.velocity = new Vector2(originalMaxSpeed, rbody.velocity.y);
			}
		}
	}

	private void KnockUpForce(){
		if(damaged){
			damaged = false;
			rbody.AddForce (new Vector2 (0f, m_KnockbackHeight));
		}
	}
	
	bool beganJumping = false;

	public void GravityJump(bool jump, bool jumpHold){
		if(jump){
			if (grounded && animator.GetBool ("InGround")
			&& !animator.GetCurrentAnimatorStateInfo(0).IsName("Damage")
			&& !animator.GetCurrentAnimatorStateInfo(0).IsName("Death")) {
				rbody.velocity = new Vector2(rbody.velocity.x, 0);
				DoJump (m_JumpForce);
				beganJumping = true;
			}
		} else if(!jumpHold && beganJumping && animator.GetFloat("Yspeed") >= 0){
			rbody.velocity = new Vector2(rbody.velocity.x, 0);
			beganJumping = false;
		}


		if (!grounded && !animator.GetBool ("InGround")
			&& !animator.GetCurrentAnimatorStateInfo(0).IsName("Damage")
			&& !animator.GetCurrentAnimatorStateInfo(0).IsName("Death")
			&& animator.GetFloat("Yspeed") < 0) {

				DoFall ();
		}
	}


	float maxJumpSpeed = 3.0f;
	public void DoJump(float jumpForce){

		grounded = false;
		animator.SetBool ("InGround", false);
		animator.SetBool ("Jump", true);
		rbody.AddForce (new Vector2 (0f, jumpForce), ForceMode2D.Impulse);

		// if(rbody.velocity.y > maxJumpSpeed)
		// 	rbody.velocity = new Vector2(rbody.velocity.x, maxJumpSpeed);
	}

	public void DoFall(){
		grounded = false;
		animator.SetBool ("InGround", false);
		animator.SetBool ("Fall", true);
	}

    private void Flip()
    {
		faceDir = -faceDir;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
		facingRight = !facingRight;
    }

	public void WasDamaged(){
		damaged = true;
	}

	public float GetMaxSpeed(){
		return maxSpeed;
	}

	public float GetOriginalMaxSpeed(){
		return originalMaxSpeed;
	}

	public void SetMaxSpeed(float newSpeed){
		maxSpeed = newSpeed;
	}

	public void RestoreMaxSpeed(){
		maxSpeed = originalMaxSpeed;
	}

	protected bool CheckInTransformArea (Transform transform, float areaSize, LayerMask layers){

		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, areaSize, layers);
		if (colliders.Length > 0)
			return true;
		return false;
	}
}

