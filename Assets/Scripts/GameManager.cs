using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance { get; private set; }

	//[HideInInspector]
	public bool isPaused;

	public PlayerController player;

	public int positionsNb;
	public List<Vector3> positionsBuffer;

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
	}
}
