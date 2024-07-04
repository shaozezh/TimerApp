using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecrementIndex : MonoBehaviour
{
    public void UpdateIndexAfter(int index)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            SwipeToDelete swipeToDelete = transform.GetChild(i).gameObject.GetComponent<SwipeToDelete>();
            if (swipeToDelete.index > index)
            {
                swipeToDelete.index--;
            }
        }
    }
}
