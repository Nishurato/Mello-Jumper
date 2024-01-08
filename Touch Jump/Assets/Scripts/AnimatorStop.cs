using UnityEngine;

public class AnimatorStop : MonoBehaviour
{
    private Animator animator;
	private Rigidbody2D rb;

	// Start is called before the first frame update
	void Start()
    {
        animator = GetComponent<Animator>();
        rb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead())
        {
            animator.enabled = false;
        }
    }
	private bool isDead()
	{
		return rb.bodyType == RigidbodyType2D.Static;
	}
}
