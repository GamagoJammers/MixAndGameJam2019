using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternatingTrap : MonoBehaviour
{
	private SpriteRenderer sr;
	private Collider2D coll;

	public Sprite deactivated;
	public Sprite menacing;
	public Sprite activated;

	public float timer;
	public float deactivatedTime;
	public float menacingTime;
	public float activatedTime;

	void Awake()
    {
		sr = GetComponent<SpriteRenderer>();
		coll = GetComponent<Collider2D>();

		sr.sprite = deactivated;
		timer = deactivatedTime;
		coll.enabled = false;
    }
	
    void Update()
    {
		timer -= Time.deltaTime;
		if(timer <= 0.0f)
		{
			if(sr.sprite == deactivated)
			{
				sr.sprite = menacing;
				timer = menacingTime;
			}
			else if(sr.sprite == menacing)
			{
				sr.sprite = activated;
				timer = activatedTime;
				coll.enabled = true;
			}
			else if(sr.sprite == activated)
			{
				sr.sprite = deactivated;
				timer = deactivatedTime;
				coll.enabled = false;
			}
		}
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			if (GameManager.instance.actualDeathCoroutine == null)
			{
				GameManager.instance.actualDeathCoroutine = GameManager.instance.StartCoroutine(GameManager.instance.DeathCoroutine());
			}
		}
	}
}
