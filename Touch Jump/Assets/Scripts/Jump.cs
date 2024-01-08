using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Jump : MonoBehaviour
{
	//Player components
	private Rigidbody2D rb;
	private BoxCollider2D coll;
	private Animator anim;
	[SerializeField] private AudioSource jumpSFX;
	[SerializeField] private AudioSource landSFX;
	[SerializeField] private LayerMask jumpableGround;
	[SerializeField] private Slider slider;

	//Player counts
	[SerializeField] private float jumpForce = 12f;
	[SerializeField] private float jumpDistance = 10f;
	private float timer = 0f;
	private float timerAnimtation = 0f;
	private float charge = 0.3f;
	private float chargeReverse = 1f;
	private bool chargeIsFull = false;
	private bool playerIsJumped = false;
	private bool reversed = false;
	private bool landSFXIsPlayed = false;
	private bool gameIsStarted = false;

	//Menu components
	[SerializeField] private GameObject hint;
	[SerializeField] private List<Animator> mainMenuAnimator;
	[SerializeField] private List<Animator> gameHUDAnimator;
	[SerializeField] private GameObject mainMenu;
	[SerializeField] private GameObject buttonListener;

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		coll = GetComponent<BoxCollider2D>();
		anim = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		//Player script
		slider.value = charge;

		if (SystemInfo.deviceType == DeviceType.Handheld)
		{
			mobileControl();
		}

		else if (SystemInfo.deviceType == DeviceType.Desktop)
		{
			pcControl();
		}

		if (gameIsStarted)
		{
			AnimationStart();
		}

		anim.SetFloat("rigidbodyVelocity", rb.velocity.y);

		PlayerIsGrounded();
	}

	private void PlayerIsGrounded()
	{
		if (isGrounded())
		{
			stopJumpPhase();
		}

		else
		{
			if (timer >= .1f)
			{
				playerIsJumped = true;
			}
			else
			{
				timer += Time.deltaTime;
			}
		}
	}

	private void pcControl()
	{
		if (!gameIsStarted)
		{
			if (Input.GetButtonDown("Fire1") && !EventSystem.current.IsPointerOverGameObject())
			{
				gameIsStarted = true;
			}
		}

		else
		{
			if (Input.GetButton("Fire1") && isGrounded())
			{
				firstPhaseJump();
			}

			else if (Input.GetButtonUp("Fire1") && isGrounded())
			{
				secondPhaseJump();
			}
		}
	}

	private void mobileControl()
	{
		if (Input.touchCount > 0)
		{
			if (!gameIsStarted)
			{
				if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) && Input.GetTouch(0).phase == TouchPhase.Began)
				{
					gameIsStarted = true;
				}
			}
			else
			{
				if ((Input.GetTouch(0).phase == TouchPhase.Stationary || Input.GetTouch(0).phase == TouchPhase.Moved) && isGrounded())
				{
					firstPhaseJump();
				}

				else if (Input.GetTouch(0).phase == TouchPhase.Ended && isGrounded())
				{
					secondPhaseJump();
				}
			}
		}
	}

	private void stopJumpPhase()
	{
		float timerIn = 0f;
		if (playerIsJumped)
		{
			anim.SetBool("playerIsLanded", true);
			anim.SetBool("playerIsJumped", false);
			rb.velocity = new Vector2(0, 0);

			if (timerIn < 1.5f)
			{
				timerIn += Time.deltaTime;
				if (anim.GetCurrentAnimatorStateInfo(0).IsName("mjsprite_jump_contact"))
				{
					reversed = true;
					anim.SetBool("FullyCharged", true);
				}
				else if (anim.GetCurrentAnimatorStateInfo(0).IsName("player_idle"))
				{
					reversed = false;
					anim.SetBool("FullyCharged", false);
				}
			}

			if (!landSFXIsPlayed)
			{
				landSFX.Play();
				landSFXIsPlayed = true;
			}
			timer = 0;
		}
		else
		{
			landSFXIsPlayed = false;
		}
	}

	private void secondPhaseJump()
	{
		chargeReverse -= charge;
		playerIsJumped = false;
		rb.velocity = new Vector2((jumpDistance * chargeReverse) + (chargeReverse * 10), jumpForce * charge);
		anim.SetBool("playerIsJumped", true);
		anim.SetBool("playerIsLanded", false);
		anim.SetBool("playerIsCharging", false);
		jumpSFX.Play();
		charge = 0.3f;
		chargeReverse = 1;
	}

	private void firstPhaseJump()
	{
		anim.SetBool("playerIsCharging", true);
		if (charge <= 0.9f && chargeIsFull == false)
		{
			charge += Time.deltaTime;
			if (charge >= 0.9f)
			{
				chargeIsFull = true;
				if (reversed)
				{
					anim.SetBool("FullyCharged", false);
				}
				else
				{
					anim.SetBool("FullyCharged", true);
				}
			}
		}

		else if (charge >= 0.3f && chargeIsFull == true)
		{
			charge -= Time.deltaTime;
			if (charge <= 0.3f)
			{
				chargeIsFull = false;
				if (reversed)
				{
					anim.SetBool("FullyCharged", true);
				}
				else
				{
					anim.SetBool("FullyCharged", false);
				}
			}
		}
	}

	private void AnimationStart()
	{
		hint.SetActive(false);
		for (int i = 0; i < mainMenuAnimator.Count; i++)
		{
			mainMenuAnimator[i].SetBool("isTouched", true);
		}

		for (int i = 0; i < gameHUDAnimator.Count; i++)
		{
			gameHUDAnimator[i].SetBool("isTouched", true);
		}

		if (timerAnimtation >= 2f)
		{
			mainMenu.SetActive(false);
		}
		else
		{
			timerAnimtation += Time.deltaTime;
			Debug.Log(timerAnimtation);
		}
	}

	private bool isGrounded()
	{
		return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
	}
}