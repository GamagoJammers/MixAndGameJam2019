using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Materialize : MonoBehaviour
{
	public bool active = false;
	public float value = 0.3f;
	public float speed = 1;

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
}
