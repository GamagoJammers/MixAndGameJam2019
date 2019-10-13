using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkController : MonoBehaviour
{
	private LineRenderer line;
	public bool appearing;

    void Awake()
    {
		line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
		if(!GameManager.instance.isPaused && !appearing)
		{
			line.positionCount = GameManager.instance.positionsBuffer.Count;
			line.SetPositions(GameManager.instance.positionsBuffer.ToArray());
		}
    }

	public IEnumerator AppearingCouroutine()
	{
		int posNb = 2;
		while (posNb < GameManager.instance.positionsNb)
		{
			Vector3[] tmp = new Vector3[GameManager.instance.positionsBuffer.Count]; 
			GameManager.instance.positionsBuffer.CopyTo(tmp);
			List<Vector3> tempPos = new List<Vector3>(tmp);
			tempPos.Reverse();
			tempPos.RemoveRange(posNb, GameManager.instance.positionsBuffer.Count - posNb);
			line.positionCount = tempPos.Count;
			line.SetPositions(tempPos.ToArray());
			posNb+=5;
			yield return new WaitForEndOfFrame();
		}
		appearing = false;
	}
}
