﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Rigidbody2D rb;
	private Collider2D coll;
    private Animator anim;
	//private Animator anim;

	public bool canRewind;
	public bool rewinding;
	public GhostController ghost;
	public LinkController link;
	public ParticleSystem rewindParticles;

	//[HideInInspector]
	public bool grounded;
	public LayerMask collidableMask;
	private float raycastOffset = 0.02f;

	public float speed;
	[HideInInspector]
	public float directionX;

	public float jumpVelocity;
	public float fallingCoefficient;
	public float smallJumpCoefficient;
	public float airControlCoefficient;

	public Coroutine actualLinkReappearingCoroutine;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
		//anim = GetComponent<Animator>();
	}

	private void FixedUpdate()
	{
		if (!GameManager.instance.isPaused && !rewinding)
			UpdatePositionsBuffer();
	}

	private void Update()
	{
		if (!GameManager.instance.isPaused)
		{
			if (!rewinding)
			{
				CheckGround();
				Move();
				Jump();

				CheckRewind();
				CheckReset();

				//UpdateAnimatorParameters();
			}
			else
			{
				Rewind();
			}
		}
	}

	void CheckGround()
	{
		Vector2 bottomLeftRayPos = new Vector2(transform.position.x - coll.bounds.extents.x + raycastOffset, transform.position.y - coll.bounds.extents.y + raycastOffset);
		Vector2 bottomRightRayPos = new Vector2(transform.position.x + coll.bounds.extents.x - raycastOffset, transform.position.y - coll.bounds.extents.y + raycastOffset);

		if (Physics2D.Raycast(bottomLeftRayPos, -Vector2.up, 2 * raycastOffset, collidableMask) ||
			Physics2D.Raycast(bottomRightRayPos, -Vector2.up, 2 * raycastOffset, collidableMask))
			grounded = true;
		else
			grounded = false;
        anim.SetBool("Grounded", grounded);
    }

	void Move()
	{
        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.localScale = new Vector3(2, 2, 1);
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            transform.localScale = new Vector3(-2, 2, 1);
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            anim.SetTrigger("Walk");
        }
        else
        {
            anim.ResetTrigger("Walk");
            anim.SetTrigger("Idle");

        }
        directionX = Input.GetAxis("Horizontal");
		float velocityX = directionX * speed;
		if (!grounded)
			velocityX *= airControlCoefficient;

		rb.velocity = new Vector2(velocityX, rb.velocity.y);
        //anim.SetFloat("Speed", Mathf.Abs(velocityX) / speed);
    }

	void Jump()
	{
        if (Input.GetButtonDown("Jump") && grounded)
        {
            rb.velocity += Vector2.up * jumpVelocity;
            anim.SetTrigger("Jump");
        }
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallingCoefficient - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (smallJumpCoefficient - 1) * Time.deltaTime;
        }
            
	}

	void UpdatePositionsBuffer()
	{
		GameManager.instance.positionsBuffer.Add(transform.position);
		if(GameManager.instance.positionsBuffer.Count > GameManager.instance.positionsNb)
		{
			if(canRewind == false)
			{
				if(actualLinkReappearingCoroutine == null)
				{
					actualLinkReappearingCoroutine = StartCoroutine(LinkReappearingCoroutine());
				}
			}
			GameManager.instance.positionsBuffer.RemoveAt(0);
		}
	}

	void CheckRewind()
	{
		if (Input.GetKeyDown(KeyCode.E) && canRewind)
		{
			canRewind = false;
			coll.isTrigger = true;
			rb.gravityScale = 0.0f;
			GameManager.instance.positionsBuffer.Reverse();
			rewinding = true;
			rewindParticles.Play();
		}
	}

	void Rewind()
	{
		if (GameManager.instance.positionsBuffer.Count > 3)
		{
			transform.position = GameManager.instance.positionsBuffer[0];
			GameManager.instance.positionsBuffer.RemoveAt(2);
			GameManager.instance.positionsBuffer.RemoveAt(1);
			GameManager.instance.positionsBuffer.RemoveAt(0);
		}
		else
		{
			GameManager.instance.positionsBuffer.Reverse();
			transform.position = GameManager.instance.positionsBuffer[0];
			GameManager.instance.positionsBuffer.Clear();
			ghost.GetComponent<Materialize>().active = false;
			ghost.GetComponent<Materialize>().value = 0.3f;
			ghost.GetComponent<MeshRenderer>().enabled = false;
			link.gameObject.SetActive(false);
			coll.isTrigger = false;
			rb.gravityScale = 1.0f;
			rewinding = false;
			rewindParticles.Stop();
		}
	}

	public void CheckReset()
	{
		if(Input.GetKeyDown(KeyCode.R))
		{
			GameManager.instance.StartCoroutine(GameManager.instance.DeathCoroutine());
		}
	}

	IEnumerator LinkReappearingCoroutine()
	{
		link.appearing = true;
		link.gameObject.SetActive(true);
		link.StartCoroutine(link.AppearingCouroutine());
		yield return new WaitUntil(() => link.appearing == false);
		ghost.GetComponent<MeshRenderer>().enabled = true;
		ghost.GetComponent<Materialize>().active = true;
		canRewind = true;
		actualLinkReappearingCoroutine = null;
	}
}
