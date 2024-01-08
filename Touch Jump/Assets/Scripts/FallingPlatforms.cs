using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatforms : MonoBehaviour
{
	private Rigidbody2D rb;
	private Rigidbody2D levelsRigidbody;

	private bool isUnfrezzed = false;

	private void Start()
	{
		levelsRigidbody = GameObject.Find("Levels").GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		if (isUnfrezzed)
		{
			rb.velocity = new Vector2(levelsRigidbody.velocity.x, rb.velocity.y);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name == "Player")
		{
			if (!isUnfrezzed)
			{
				this.gameObject.AddComponent<Rigidbody2D>();
				rb = this.gameObject.GetComponent<Rigidbody2D>();
				rb.constraints = RigidbodyConstraints2D.FreezeRotation;
				isUnfrezzed = true;
			}
		}
	}
}
