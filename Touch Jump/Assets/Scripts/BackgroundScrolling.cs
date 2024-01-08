using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundScrolling : MonoBehaviour
{
    [SerializeField] private Renderer bgRenderer;

	[SerializeField] private Rigidbody2D rb;
	[SerializeField] private Transform playerTransform;
	[SerializeField] private BoxCollider2D coll;
	[SerializeField] private LayerMask jumpableGround;

	[SerializeField] private float speed = 0.1f;
	private float timer = 0f;
	private float charge = 0.3f;
	private float chargeReverse = 1f;
	private bool chargeIsFull = false;
	private bool playerIsJumped = false;
	private bool backgroundNow = false;
	private bool gameIsStarted = false;
	[SerializeField] private float jumpDistance = 10f;

	// Start is called before the first frame update
	void Start()
    {
		bgRenderer.material.mainTextureOffset = new Vector2(0, 0);
	}

    // Update is called once per frame
    void Update()
	{
		transform.position = new Vector2(17.1f + playerTransform.position.x, transform.position.y);

		if (SystemInfo.deviceType == DeviceType.Handheld)
		{
			mobileControl();
		}

		else if (SystemInfo.deviceType == DeviceType.Desktop)
		{
			pcControl();
		}

		if (backgroundNow && !IsDead())
		{
			bgRenderer.material.mainTextureOffset += new Vector2(((speed * Time.deltaTime) * chargeReverse) * ((jumpDistance * chargeReverse) + (chargeReverse * 10)), 0);
		}

		if (IsGrounded())
		{
			if (playerIsJumped)
			{
				chargeReverse = 1f;
				charge = 0.3f;
				bgRenderer.material.mainTextureOffset += new Vector2(0, 0);
				backgroundNow = false;
				timer = 0f;
			}
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

	private void mobileControl()
	{
		if (Input.touchCount > 0)
		{
			if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) && Input.GetTouch(0).phase == TouchPhase.Began)
			{
				gameIsStarted = true;
			}
			else
			{
				if ((Input.GetTouch(0).phase == TouchPhase.Stationary || Input.GetTouch(0).phase == TouchPhase.Moved) && IsGrounded() && !IsDead())
				{
					FirstJumpPhase();
				}

				else if (Input.GetTouch(0).phase == TouchPhase.Ended && IsGrounded() && !IsDead())
				{
					SecondJumpPhase();
				}
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
			if (Input.GetButton("Fire1") && IsGrounded())
			{
				FirstJumpPhase();
			}

			else if (Input.GetButtonUp("Fire1") && IsGrounded())
			{
				SecondJumpPhase();
			}
		}
	}

	private void FirstJumpPhase()
	{
		if (charge <= 0.9f && chargeIsFull == false)
		{
			charge += Time.deltaTime;
			if (charge >= 0.9f)
			{

				chargeIsFull = true;
			}
		}

		else if (charge >= 0.3f && chargeIsFull == true)
		{
			charge -= Time.deltaTime;
			if (charge <= 0.3f)
			{
				chargeIsFull = false;
			}
		}
	}

	private void SecondJumpPhase()
	{
		playerIsJumped = false;
		chargeReverse -= charge;
		backgroundNow = true;
	}

	private bool IsGrounded()
	{
		return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
	}

	private bool IsDead()
	{
		return rb.bodyType == RigidbodyType2D.Static;
	}
}
