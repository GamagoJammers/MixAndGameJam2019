using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager instance { get; private set; }

	//[HideInInspector]
	public bool isPaused;

	public PlayerController player;

	public int positionsNb;
	[HideInInspector]
	public List<Vector3> positionsBuffer;

	public Animator UIAnim;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(this.gameObject);
		}

		positionsBuffer = new List<Vector3>();
		player.ghost.GetComponent<MeshRenderer>().enabled = false;
		player.link.gameObject.SetActive(false);
		StartCoroutine(FadeCoroutine("FadeIn"));
	}

	public IEnumerator FadeCoroutine(string fadeName)
	{
		GameManager.instance.isPaused = true;
		UIAnim.SetTrigger(fadeName);
		yield return new WaitForSeconds(UIAnim.GetCurrentAnimatorStateInfo(0).length);
		GameManager.instance.isPaused = false;
	}

	public IEnumerator DeathCoroutine()
	{
		//death anim/particules ?

		StartCoroutine(FadeCoroutine("FadeOut"));
		yield return new WaitUntil(() => isPaused == false);

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
