using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
	private void Update()
	{
		if(!GameManager.instance.isPaused)
		{
			if (GameManager.instance.positionsBuffer.Count == GameManager.instance.positionsNb && !GameManager.instance.player.rewinding)
			{
				transform.position = GameManager.instance.positionsBuffer[0];
			}
		}
	}
}
