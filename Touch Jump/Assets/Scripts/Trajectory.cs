using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Trajectory : MonoBehaviour
{
	[Header("Formula variables")]
	[SerializeField] private Vector2 velocity;
	[SerializeField] private float yLimit;
	private float g;

	[Header("Linecast variables")]
	[Range(2, 30)]
	[SerializeField] private int linecastResolution;
	[SerializeField] private LayerMask canHit;

	[SerializeField] private GameObject point;
	private List<GameObject> points = new List<GameObject>();
	private List<GameObject> pointsParrent = new List<GameObject>();
	[Range(2, 30)]
	[SerializeField] private int dotsCount;

	private float charge = 0.3f;
	private float chargeReverse = 1f;
	private float cRTemp = 1f;
	private bool chargeIsFull = false;
	private bool gameIsStarted = false;

	[SerializeField] private BoxCollider2D coll;
	[SerializeField] private LayerMask jumpableGround;

	void Start()
	{
		g = Mathf.Abs(Physics2D.gravity.y);
		for (int i = 0; i < dotsCount; i++)
		{
			points.Add(new GameObject("Point" + i));
			Instantiate(point, new Vector2(0f,0f), Quaternion.identity, points[i].transform);
			points[i].transform.position = new Vector2(0f, -10f);	
		}
	}

	void Update()
	{
		if (SystemInfo.deviceType == DeviceType.Handheld)
		{
			mobileControl();
		}
		else if (SystemInfo.deviceType == DeviceType.Desktop)
		{
			pcControl();
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

		else {
			if (Input.GetButton("Fire1") && isGrounded())
			{
				firstJumpPhase();
			}

			else if (Input.GetButtonUp("Fire1") && isGrounded())
			{
				secondJumpPhase();
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
					firstJumpPhase();
				}

				else if (Input.GetTouch(0).phase == TouchPhase.Ended && isGrounded())
				{
					secondJumpPhase();
				}
			}
		}
	}

	private void secondJumpPhase()
	{
		if (gameIsStarted && isGrounded())
		{
			for (int i = 0; i < dotsCount; i++)
			{
				points[i].transform.position = new Vector2(0f, -10f);
			}
			chargeReverse = 1f;
			charge = 0.3f;
		}
	}

	private void firstJumpPhase()
	{
		RenderArc();
		if (charge <= 0.9f && chargeIsFull == false)
		{
			charge += Time.deltaTime;
			chargeReverse = cRTemp - charge;
			if (charge >= 0.9f)
			{
				chargeIsFull = true;
			}
		}
		else if (charge >= 0.3f && chargeIsFull == true)
		{
			charge -= Time.deltaTime;
			chargeReverse = cRTemp - charge;
			if (charge <= 0.3f)
			{
				chargeIsFull = false;
			}
		}
	}

	private void RenderArc()
	{
		var lowestTimeValue = MaxTimeX() / linecastResolution;

		for (int i = 1; i < dotsCount; i++)
		{
			var t = lowestTimeValue * i;
			points[i].transform.position = CalculateLinePoint(t);
		}
		
	}

	private Vector2 HitPosition()
	{
		var lowestTimeValue = MaxTimeY() / linecastResolution;

		for (int i = 0; i < linecastResolution + 1; i++)
		{
			var t = lowestTimeValue * i;
			var t2 = lowestTimeValue * (i + 1);

			var hit = Physics2D.Linecast(CalculateLinePoint(t), CalculateLinePoint(t2), canHit);

			if (hit)
				return hit.point;
		}

		return CalculateLinePoint(MaxTimeY());
	}

	private Vector2 CalculateLinePoint(float time)
	{
		float x = ((velocity.x * chargeReverse) + (chargeReverse * 10)) * time;
		float y = ((velocity.y * charge) * time) - (g * Mathf.Pow(time, 2) / 2);
		return new Vector2(x + transform.position.x, y + transform.position.y);
	}

	private float MaxTimeY()
	{
		float v = velocity.y;
		float v2 = v * v;

		float t = (v + Mathf.Sqrt(v2 + 2 * g * (transform.position.y - yLimit))) / g;
		return t;
	}

	private float MaxTimeX()
	{
		float x = velocity.x;
		if (x == 0)
		{
			velocity.x = 000.1f;
			x = velocity.x;
		}

		float t = (HitPosition().x - transform.position.x) / x;
		return t;
	}

	private bool isGrounded()
	{
		return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
	}
}
