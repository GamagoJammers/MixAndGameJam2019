using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	public int sceneToLoad;
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			StartCoroutine(LoadSceneCoroutine());
		}
	}

	public IEnumerator LoadSceneCoroutine()
	{
		GameManager.instance.StartCoroutine(GameManager.instance.FadeCoroutine("FadeOut"));
		yield return new WaitUntil(() => GameManager.instance.isPaused == false);

		SceneManager.LoadScene(sceneToLoad);
	}
}
