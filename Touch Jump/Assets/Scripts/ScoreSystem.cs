using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Localization;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text scoreGameOver;
    [SerializeField] private TMP_Text highScore;
	[SerializeField] private TMP_Text highScoreGameOver;
    [SerializeField] private TMP_Text highScoreStart;
    [SerializeField] private int highScoreNumber;
	[SerializeField] private LocalizedString localStringHighScore;
	private bool needToUpdate = true;

	[SerializeField] private Button hiScoreButton;
	private bool isConnectedToGP = false;

	[SerializeField] private Rigidbody2D rbPlayer;
	[SerializeField] private Transform playerTransform;

	void Start()
    {
		highScoreNumber = PlayerPrefs.GetInt("HighScore");
		localStringHighScore.Arguments[0] = highScoreNumber;
		localStringHighScore.RefreshString();
	}

	void Update()
	{
		score.text = ((int)playerTransform.position.x).ToString();
		

		if ((int)this.gameObject.transform.position.x > PlayerPrefs.GetInt("HighScore"))
		{
			highScoreNumber = (int)this.gameObject.transform.position.x;
			localStringHighScore.Arguments[0] = highScoreNumber;
			localStringHighScore.RefreshString();

			PlayerPrefs.SetInt("HighScore", highScoreNumber);
			if (isConnectedToGP)
			{
				Social.ReportScore(PlayerPrefs.GetInt("HighScore", highScoreNumber), GPGSIds.leaderboard_high_score, (bool success) =>
				{
					if (success)
					{
						Debug.Log("Published");
					}
					else
					{
						Debug.Log("Not published");
					}
				});
			}
		}

		if (isDead())
		{
			//leaderboardFunction();
			scoreGameOver.text = score.text;
			highScoreGameOver.text = highScore.text;
		}
	}

	private void OnEnable()
	{
		localStringHighScore.Arguments = new object[] { highScoreNumber };
		localStringHighScore.StringChanged += UpdateText;
	}

	private void OnDisable()
	{
		localStringHighScore.StringChanged -= UpdateText;
	}

	private void UpdateText(string value)
	{
		highScore.text = value;
		highScoreGameOver.text = highScore.text;
		highScoreStart.text = highScore.text;
	}

	private bool isDead()
	{
		return rbPlayer.bodyType == RigidbodyType2D.Static;
	}
}
