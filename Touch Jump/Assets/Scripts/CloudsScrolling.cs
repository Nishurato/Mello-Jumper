using UnityEngine;

public class CloudsScrolling : MonoBehaviour
{
	[SerializeField] private Renderer bgRenderer;
	[SerializeField] private float speed = 0.1f;

	[SerializeField] private Rigidbody2D rb;

	// Start is called before the first frame update
	void Start()
    {
		bgRenderer.material.mainTextureOffset = new Vector2(0, 0);
	}

    // Update is called once per frame
    void Update()
    {
		if (!isDead())
		{
			bgRenderer.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0);
		}
	}

	private bool isDead()
	{
		return rb.bodyType == RigidbodyType2D.Static;
	}
}
