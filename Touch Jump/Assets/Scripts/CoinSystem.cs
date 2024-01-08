using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinSystem : MonoBehaviour
{
	[SerializeField] private int coins = 0;
    [SerializeField] private AudioSource coinSFX;
	[SerializeField] private GameObject levelParent;
	[SerializeField] private List<TMP_Text> coinsCountText;

	[SerializeField] private List<Button> paletes;
	private List<bool> paleteIsBuyed = new List<bool>();

	[SerializeField] private Image playerPreview;
	[SerializeField] private List<Material> playerColorMaterials;

    // Start is called before the first frame update
    void Start()
    {
        coins = PlayerPrefs.GetInt("Coins");
		for (int i = 0; i < coinsCountText.Count; i++)
		{
			coinsCountText[i].text = coins.ToString();
		}

		paleteIsBuyed.Add(true);

		for (int i = 1; i < paletes.Count; i++)
		{
			paleteIsBuyed.Add(false);
		}
		playerPreview.material = playerColorMaterials[1];
	}

	// Update is called once per frame
	void Update()
    {

    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Coin" && 
			collision.gameObject.name != levelParent.transform.GetChild(1).gameObject.name)
        {
				coins++;
				for (int i = 0; i < coinsCountText.Count; i++)
				{
					coinsCountText[i].text = coins.ToString();
				}
				PlayerPrefs.SetInt("Coins", coins);
				coinSFX.Play();
				Destroy(collision.gameObject);
		}
	}
}
