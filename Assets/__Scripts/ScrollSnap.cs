using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Unity.Burst.CompilerServices;
using System.Collections.Generic;

public class ScrollSnap : MonoBehaviour 
{
	// Public Variables
	public RectTransform panel;	// To hold the ScrollPanel
	public List<GameObject> bttn;
	public RectTransform center;	// Center to compare the distance for each button

	// Private Variables
	public float[] distance;	// All buttons' distance to the center
	public float[] distReposition;
	private bool dragging = false;	// Will be true, while we drag the panel
	private int bttnDistance;	// Will hold the distance between the buttons
	private int minButtonNum;	// To hold the number of the button, with smallest distance to center
	private int bttnLength;
	public int inputTime;
	void Start()
	{
		bttnLength = panel.childCount - 2;
		for (int i = 0; i < bttnLength; i++)
		{
			bttn.Add(panel.GetChild(2+i).gameObject);
		}
		distance = new float[bttnLength];
		distReposition = new float[bttnLength];

		// Get distance between buttons
		bttnDistance  = (int)Mathf.Abs(bttn[1].GetComponent<RectTransform>().anchoredPosition.y - bttn[0].GetComponent<RectTransform>().anchoredPosition.y);
	}

	void Update()
	{
		for (int i = 0; i < bttnLength; i++)
		{
			distReposition[i] = center.GetComponent<RectTransform>().position.y - bttn[i].GetComponent<RectTransform>().position.y;
			distance[i] = Mathf.Abs(distReposition[i]);

			if (distReposition[i] > 3000)
			{
				float curX = bttn[i].GetComponent<RectTransform>().anchoredPosition.x;
				float curY = bttn[i].GetComponent<RectTransform>().anchoredPosition.y;

				Vector2 newAnchoredPos = new Vector2 (curX, curY + (bttnLength * bttnDistance));
				bttn[i].GetComponent<RectTransform>().anchoredPosition = newAnchoredPos;
			}

			if (distReposition[i] < -3000)
			{
				float curX = bttn[i].GetComponent<RectTransform>().anchoredPosition.x;
				float curY = bttn[i].GetComponent<RectTransform>().anchoredPosition.y;

				Vector2 newAnchoredPos = new Vector2 (curX, curY - (bttnLength * bttnDistance));
				bttn[i].GetComponent<RectTransform>().anchoredPosition = newAnchoredPos;
			}
		}
	
		float minDistance = Mathf.Min(distance);	// Get the min distance

		for (int a = 0; a < bttnLength; a++)
		{
			if (minDistance == distance[a])
			{
				minButtonNum = a;
			}
		}
		inputTime = minButtonNum;

		if (!dragging)
		{
		//	LerpToBttn(minButtonNum * -bttnDistance);
			LerpToBttn (-bttn[minButtonNum].GetComponent<RectTransform>().anchoredPosition.y);
		}
	}

	void LerpToBttn(float position)
	{
		float newY = Mathf.Lerp (panel.anchoredPosition.y, position, Time.deltaTime * 5f);
		Vector2 newPosition = new Vector2 (panel.anchoredPosition.x, newY);

		panel.anchoredPosition = newPosition;
	}

	public void StartDrag()
	{
		dragging = true;
	}

	public void EndDrag()
	{
		dragging = false;
	}

}