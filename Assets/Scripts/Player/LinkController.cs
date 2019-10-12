using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkController : MonoBehaviour
{
	private LineRenderer line;


    void Awake()
    {
		line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
		line.positionCount = GameManager.instance.positionsBuffer.Count;
		line.SetPositions(GameManager.instance.positionsBuffer.ToArray());
    }
}
