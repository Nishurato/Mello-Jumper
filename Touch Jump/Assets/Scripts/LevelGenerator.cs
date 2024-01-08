using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
	[SerializeField] private List<GameObject> levels;
	[SerializeField] private List<GameObject> coinLevels;
	[SerializeField] private GameObject level0;
	[SerializeField] private GameObject levelParent;
	[SerializeField] private Transform playerTransform;

	private float level0X = 28f;
	[SerializeField] private int countLevelFragments = 0;
	private int levelSelect = 0;
	private List<int> levelFragments = new List<int>();

	[SerializeField] private Rigidbody2D rb;

	// Start is called before the first frame update
	void Start()
    {
		levelSelect = Random.Range(0, 9);
		levelFragments.Add(levelSelect);
		Instantiate(level0, new Vector3(Mathf.FloorToInt(-26f + levels[levelSelect].transform.Find("EndLevel").position.x), 4.92f), Quaternion.identity, levelParent.transform);
		Instantiate(levels[levelSelect], new Vector3(Mathf.FloorToInt(level0.transform.Find("EndLevel").position.x), 4.92f), Quaternion.identity, levelParent.transform);
		//Instantiate(coinLevels[levelSelect], new Vector3(Mathf.FloorToInt(level0.transform.Find("EndLevel").position.x), 4.92f), Quaternion.identity);
		levelSelect = Random.Range(0, 9);
		levelFragments.Add(levelSelect);
		Instantiate(levels[levelSelect], new Vector3(Mathf.FloorToInt(27f + level0.transform.Find("EndLevel").position.x), 4.92f), Quaternion.identity, levelParent.transform);
		//Instantiate(coinLevels[levelSelect], new Vector3(Mathf.FloorToInt(27f + level0.transform.Find("EndLevel").position.x), 4.92f), Quaternion.identity);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name == "Player" && gameObject.tag == "Untagged")
		{
			countLevelFragments++;

			//Easy
			if (countLevelFragments > 1 && countLevelFragments < 10)
			{
				levelSelect = Random.Range(0, 9);
				for (int i = 0; i < levelFragments.Count; i++)
				{
					if (levelFragments[i] == levelSelect)
					{
						levelSelect = Random.Range(0, 9);
					}
				}
				levelFragments.Add(levelSelect);
				Instantiate(levels[levelSelect], new Vector3(Mathf.FloorToInt(levels[levelSelect].transform.Find("EndLevel").position.x + level0X + playerTransform.position.x), 
					4.92f), Quaternion.identity, levelParent.transform);
				//Instantiate(coinLevels[levelSelect], new Vector3(Mathf.FloorToInt(levels[levelSelect].transform.Find("EndLevel").position.x + level0X + playerTransform.position.x),
					//4.92f), Quaternion.identity);
			}

			//Normal
			else if (countLevelFragments >= 10 && countLevelFragments < 25)
			{
				levelSelect = Random.Range(10, 24);
				for (int i = 0; i < levelFragments.Count; i++)
				{
					if (levelFragments[i] == levelSelect)
					{
						levelSelect = Random.Range(10, 24);
					}
				}
				levelFragments.Add(levelSelect);
				Instantiate(levels[levelSelect], new Vector3(Mathf.FloorToInt(levels[levelSelect].transform.Find("EndLevel").position.x + level0X + playerTransform.position.x),
					4.92f), Quaternion.identity, levelParent.transform);
				//Instantiate(coinLevels[levelSelect], new Vector3(Mathf.FloorToInt(levels[levelSelect].transform.Find("EndLevel").position.x + level0X + playerTransform.position.x),
					//4.92f), Quaternion.identity);
			}

			//Hard
			else if (countLevelFragments >= 25)
			{
				levelSelect = Random.Range(25, levels.Count - 1);
				for (int i = 0; i < levelFragments.Count; i++)
				{
					if (levelFragments[i] == levelSelect)
					{
						levelSelect = Random.Range(10, 24);
					}
				}
				levelFragments.Add(levelSelect);
				if (levelFragments.Count == 40)
				{
					for (int i = 0; i < 15; i++)
					{
						levelFragments.Remove(levels.Count - i - 1);
					}
				}
				Instantiate(levels[levelSelect], new Vector3(Mathf.FloorToInt(levels[levelSelect].transform.Find("EndLevel").position.x + level0X + playerTransform.position.x),
					4.92f), Quaternion.identity, levelParent.transform);
				//Instantiate(coinLevels[levelSelect], new Vector3(Mathf.FloorToInt(levels[levelSelect].transform.Find("EndLevel").position.x + level0X + playerTransform.position.x),
					//4.92f), Quaternion.identity);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.name == "Player" && gameObject.tag == "Untagged")
		{
			if (!isDead())
			{
				Destroy(levelParent.transform.GetChild(0).gameObject);
				//Destroy(GameObject.Find(levelParent.transform.GetChild(0).gameObject.name));
			}
		}		
	}

	private bool isDead()
	{
		return rb.bodyType == RigidbodyType2D.Static;
	}
}
