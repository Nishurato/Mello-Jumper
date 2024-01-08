using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using TMPro;

public class LanguageSystem : MonoBehaviour
{
    private bool active = false;
    [SerializeField] private List<TMP_Text> texts;
    [SerializeField] private TMP_FontAsset originalFont;
    [SerializeField] private TMP_FontAsset arabicFont;
    [SerializeField] private TMP_FontAsset asianFont;

	[SerializeField] private GameObject BGLoading;

	private void Start()
	{
        int ID = PlayerPrefs.GetInt("LocaleKey", 3);
        ChangeLocale(ID);
	}

	public void ChangeLocale(int localeID)
    {
        if (active)
        {
            return;
        }
        StartCoroutine(SetLocale(localeID));
    }

	private void FontChange(int _localeID)
	{
		if (_localeID == 0)
		{
			
			for (int i = 0; i < texts.Count; i++)
			{
				texts[i].font = arabicFont;
			}
		}
		else if (_localeID == 1 || _localeID == 2 || _localeID == 7 || _localeID == 8)
		{
			for (int i = 0; i < texts.Count; i++)
			{
				texts[i].font = asianFont;
			}
		}
		else
		{
			for (int i = 0; i < texts.Count; i++)
			{
				texts[i].font = originalFont;
			}
		}
	}

	private IEnumerator SetLocale(int _localeID)
    {
		BGLoading.SetActive(true);
		active = true;
        yield return LocalizationSettings.InitializationOperation;
		FontChange(_localeID);
		LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];
		PlayerPrefs.SetInt("LocaleKey", _localeID);
		active = false;
		yield return new WaitForSeconds(0.1f);
		BGLoading.SetActive(false);
	}
}
