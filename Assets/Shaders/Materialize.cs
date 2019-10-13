using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Materialize : MonoBehaviour
{
	private bool active = false;
	private float value = 1;
	public float speed = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		if (!GameManager.instance.isPaused)
		{
			if (value > -0.2 && active)
			{
				value -= Time.deltaTime * speed;
			}

			foreach (var material in GetComponent<Renderer>().materials)
			{
				material.SetFloat("_Materialize_Amount", value);
			}
		}
	}

	private void OnEnable()
	{
		active = true;
	}

	private void OnDisable()
	{
		value = 1;
	}
}
