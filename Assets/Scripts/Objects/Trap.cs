using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Player"))
		{
			if ( !GameManager.instance.player.rewinding && GameManager.instance.actualDeathCoroutine == null)
			{
				GameManager.instance.actualDeathCoroutine = GameManager.instance.StartCoroutine(GameManager.instance.DeathCoroutine());
			}
		}
	}
}
