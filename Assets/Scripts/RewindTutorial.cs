using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindTutorial : MonoBehaviour
{
	public GameObject tutorialSprite;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Player"))
		{
			StartCoroutine(RewindTutorialCoroutine());
		}
	}

	IEnumerator RewindTutorialCoroutine()
	{
		Physics2D.gravity = new Vector2(0.0f, 0.0f);
		GameManager.instance.player.rb.velocity = new Vector2(0.0f, GameManager.instance.player.rb.velocity.y/2);
		GameManager.instance.player.anim.speed = 0;
		GameManager.instance.playerStopped = true;

		yield return new WaitForSeconds(1.5f);

		GameManager.instance.bufferUnlocked = false;
		GameManager.instance.linkUnlocked = true;
		GameManager.instance.player.link.appearing = true;

		yield return new WaitUntil(() => GameManager.instance.player.link.appearing == false);
		yield return new WaitForSeconds(0.75f);

		tutorialSprite.SetActive(true);
		GameManager.instance.rewindUnlocked = true;

		yield return new WaitUntil(() => GameManager.instance.player.rewinding == true);

		GameManager.instance.bufferUnlocked = true;
		Physics2D.gravity = new Vector2(0.0f, -25.0f);
		GameManager.instance.player.anim.speed = 1;
		GameManager.instance.playerStopped = false;
		Destroy(gameObject);
	}
}
