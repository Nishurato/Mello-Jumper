using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonListenerScript : MonoBehaviour
{
	[SerializeField] private AudioSource music;
	[SerializeField] private AudioSource musicGameOver;
	[SerializeField] private List<AudioSource> sounds;

	[SerializeField] private GameObject infoMenu;
	[SerializeField] private GameObject settingsMenu;
	[SerializeField] private GameObject langMenu;
	[SerializeField] private GameObject shopMenu;

	[SerializeField] private Toggle musicToggle;
	[SerializeField] private Toggle soundToggle;

	[SerializeField] private GameObject BGLoading;

	[SerializeField] private List<Sprite> sprites;

	private void Start()
	{
		bool musicMuted = (PlayerPrefs.GetInt("MusicIsMuted") == 0) ? true : false;
		bool soundMuted = (PlayerPrefs.GetInt("SoundIsMuted") == 0) ? true : false;
		musicToggle.isOn = musicMuted;
		soundToggle.isOn = soundMuted;
	}

	public void restartLevel()
	{
		StartCoroutine(loadingRestartScreen());
	}

	public void musicMute(bool muted)
	{
		if (muted)
		{
			music.volume = 0.0f;
			music.Stop();
			musicGameOver.volume = 0.0f;
		}
		else
		{
			music.volume = 0.5f;
			music.Play();
			musicGameOver.volume = 0.5f;
		}
		PlayerPrefs.SetInt("MusicIsMuted", muted ? 0 : 1);
	}
	public void soundMute(bool muted)
	{
		if (muted)
		{
			for (int i = 0; i < sounds.Count; i++)
			{
				sounds[i].volume = 0.0f;
			}
		}
		else
		{
			for (int i = 0; i < sounds.Count; i++)
			{
				sounds[i].volume = 1f;
			}
		}
		PlayerPrefs.SetInt("SoundIsMuted", muted ? 0 : 1);
		Debug.Log(PlayerPrefs.GetInt("SoundIsMuted"));
	}

	public void infoMenuOpen()
	{
		StartCoroutine(loadingScreen());
		infoMenu.SetActive(true);
	}

	public void infoMenuClose()
	{
		StartCoroutine(loadingScreen());
		infoMenu.SetActive(false);
	}

	public void settingsMenuOpen()
	{
		StartCoroutine(loadingScreen());
		settingsMenu.SetActive(true);
	}

	public void settingsMenuClose()
	{
		StartCoroutine(loadingScreen());
		settingsMenu.SetActive(false);
	}

	public void langMenuOpen()
	{
		StartCoroutine(loadingScreen());
		langMenu.SetActive(true);
	}

	public void langMenuClose()
	{
		StartCoroutine(loadingScreen());
		langMenu.SetActive(false);
	}

	public void shopMenuOpen()
	{
		StartCoroutine(loadingScreen());
		shopMenu.SetActive(true);
	}

	public void shopMenuClose()
	{
		StartCoroutine(loadingScreen());
		shopMenu.SetActive(false);
	}

	private IEnumerator loadingRestartScreen()
	{
		BGLoading.SetActive(true);
		yield return new WaitForSeconds(0.25f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	private IEnumerator loadingScreen()
	{
		BGLoading.SetActive(true);
		yield return new WaitForSeconds(0.1f);
		BGLoading.SetActive(false);
	}

	public void XLink()
	{
		Application.OpenURL("https://twitter.com/Nishurato_EN");
	}

	public void YoutubeLink()
	{
		Application.OpenURL("https://www.youtube.com/c/Nishurato");
	}

	public void VKLink()
	{
		Application.OpenURL("https://vk.com/nishurato_gaming");
	}
}
