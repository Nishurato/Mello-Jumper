using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    private int currentWaypointIndex = 0;

	[SerializeField] private float speed = 2f;

	private bool needToReverse = false;
	private bool isTouched = false;

	private Renderer bgRenderer;
	private Renderer cloudsRenderer;

	[SerializeField] private float speedBG = 0.1f;

	private Rigidbody2D rb;

	private void Start()
	{
		bgRenderer = GameObject.Find("Background").GetComponent<Renderer>();
		cloudsRenderer = GameObject.Find("Clouds").GetComponent<Renderer>();
		rb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
    {
        if (!isDead())
        {
			if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < .1f)
			{
				currentWaypointIndex++;

				if (isTouched)
				{
					if (currentWaypointIndex == 1)
					{
						needToReverse = false;
					}
					else if (currentWaypointIndex == 2)
					{
						needToReverse = true;
					}
				}

				if (currentWaypointIndex >= waypoints.Length)
				{
					currentWaypointIndex = 0;
				}
			}

			if (isTouched)
			{
				if (!needToReverse)
				{
					bgRenderer.material.mainTextureOffset += new Vector2(speedBG * Time.deltaTime, 0);
					cloudsRenderer.material.mainTextureOffset += new Vector2(speedBG * Time.deltaTime, 0);
				}
				else
				{
					bgRenderer.material.mainTextureOffset -= new Vector2(speedBG * Time.deltaTime, 0);
					cloudsRenderer.material.mainTextureOffset -= new Vector2(speedBG * Time.deltaTime, 0);
				}
			}
			transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * speed);
		}
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name == "Player")
		{
			isTouched = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.name == "Player")
		{
			isTouched = false;
		}
	}

	private bool isDead()
	{
		return rb.bodyType == RigidbodyType2D.Static;
	}
}
