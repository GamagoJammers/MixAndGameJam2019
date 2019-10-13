using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
	public GameObject menu;
	public GameObject resumeButton;
	public EventSystem eS;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (GameManager.instance.isPaused)
			{
				Resume();
			}
			else
			{
				Time.timeScale = 0.0f;
				GameManager.instance.isPaused = true;
				menu.gameObject.SetActive(true);
				eS.SetSelectedGameObject(resumeButton);
			}
		}
	}

	public void Resume()
	{
		Time.timeScale = 1.0f;
		GameManager.instance.isPaused = false;
		menu.gameObject.SetActive(false);
	}

	public void Quit()
	{
		Application.Quit();
	}
}
