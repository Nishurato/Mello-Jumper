using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
	private Rigidbody2D rb;
	private Animator anim;
	[SerializeField] private AudioSource deathSFX;
	[SerializeField] private AudioSource BGM;
	[SerializeField] private AudioSource BGMGameOver;
	[SerializeField] private Rigidbody2D levelParentRb;
	[SerializeField] private GameObject gameHUD;

	[SerializeField] private GameObject GameOver;
	[SerializeField] private GameObject BGLoading;

	private bool isDead = false;

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		if (this.gameObject.transform.position.y < -10f && !isDead)
		{
			rb.bodyType = RigidbodyType2D.Static;
			deathSFX.Play();
			BGM.Stop();
			anim.SetBool("Dead", true);
			gameHUD.SetActive(false);
			isDead = true;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.GetContact(0).collider.tag == "Trap")
		{
			rb.bodyType = RigidbodyType2D.Static;
			deathSFX.Play();
			BGM.Stop();
			anim.SetBool("Dead", true);
			gameHUD.SetActive(false);
			isDead = true;
		}
	}

	private IEnumerator gameOverScreen()
	{
		BGLoading.SetActive(true);
		yield return new WaitForSeconds(0.75f);
		BGMGameOver.Play();
		GameOver.SetActive(true);
		BGLoading.SetActive(false);
	}
}
