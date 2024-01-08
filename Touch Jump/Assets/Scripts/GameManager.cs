using UnityEngine;

public class GameManager : MonoBehaviour
{

    void Start()
    {
		Application.targetFrameRate = Screen.currentResolution.refreshRate;
	}

	void Update()
	{
		if (Input.GetKeyDown("r"))
		{
			ScreenCapture.CaptureScreenshot("MJ_screenshot.png");
		}
	}
}
