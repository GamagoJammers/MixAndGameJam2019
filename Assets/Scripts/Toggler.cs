using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActivationMode { Collectible, Rewind, TemporaryRewind};

public class Toggler : MonoBehaviour
{
	public ActivationMode mode;

	[Header("For Rewind and TempRewind Mode")]
	public Sprite disabledSprite;
	public Sprite enabledSprite;
	public float toggleTimer;
	private bool canToggle;
	private Coroutine actualTemporaryToggleCoroutine;

	[Header("Affected Objects")]
	public List<Transform> toggleList;

	private void Awake()
	{
		canToggle = true;
	}


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Player"))
		{
			if (mode == ActivationMode.Collectible)
			{
				ToggleObjects();
				Destroy(gameObject);
			}
			if (mode == ActivationMode.Rewind && collision.gameObject.GetComponent<PlayerController>().rewinding && canToggle)
			{
				ToggleObjects();
				StartCoroutine(ToggleCooldownCoroutine());
				SwapSprites();
			}
			if (mode == ActivationMode.TemporaryRewind && collision.gameObject.GetComponent<PlayerController>().rewinding)
			{
				if(canToggle)
				{
					ToggleObjects();
					SwapSprites();
				}
				if(actualTemporaryToggleCoroutine != null)
					StopCoroutine(actualTemporaryToggleCoroutine);
				actualTemporaryToggleCoroutine = StartCoroutine(TemporaryToggleCoroutine());
			}
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (mode == ActivationMode.Rewind && collision.gameObject.GetComponent<PlayerController>().rewinding && canToggle)
		{
			ToggleObjects();
			StartCoroutine(ToggleCooldownCoroutine());
			SwapSprites();
		}
		if (mode == ActivationMode.TemporaryRewind && collision.gameObject.GetComponent<PlayerController>().rewinding)
		{
			if (canToggle)
			{
				ToggleObjects();
				SwapSprites();
			}
			if (actualTemporaryToggleCoroutine != null)
				StopCoroutine(actualTemporaryToggleCoroutine);
			actualTemporaryToggleCoroutine = StartCoroutine(TemporaryToggleCoroutine());
		}
	}

	void ToggleObjects()
	{
		foreach(Transform obj in toggleList)
		{
			if (obj.gameObject.activeInHierarchy == false)
				obj.gameObject.SetActive(true);
			else
				obj.gameObject.SetActive(false);
		}
	}

	void SwapSprites()
	{
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		if (sr.sprite == disabledSprite)
			sr.sprite = enabledSprite;
		else
			sr.sprite = disabledSprite;
	}

	IEnumerator ToggleCooldownCoroutine()
	{
		canToggle = false;
		yield return new WaitForSeconds(toggleTimer);
		canToggle = true;
	}

	IEnumerator TemporaryToggleCoroutine()
	{
		canToggle = false;
		yield return new WaitForSeconds(toggleTimer);
		ToggleObjects();
		SwapSprites();
		canToggle = true;
	}
}
