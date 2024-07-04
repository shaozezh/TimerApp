using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EditMode : MonoBehaviour
{
    public bool isEditMode;
    public GameObject editButton;
    public StoreTimer gameManager;
    public bool isSwap;
    public int[] indices;
    public int mode;
    public ColorPercentageBar bar;
    public GameObject playButton;
    void Start()
    {
        isEditMode = false;
        isSwap = false;
    }

    public void ToggleEditMode()
    {
        /*if (content.childCount == 0)
        {
            return;
        }*/
        isEditMode = !isEditMode;
        editButton.transform.GetChild(4).gameObject.SetActive(isEditMode);
        editButton.transform.GetChild(2).gameObject.SetActive(!isEditMode);
        if (mode == 0)
        {
            indices = new int[gameManager.sequences.Count];
        }
        else
        {
            indices = new int[gameManager.nameTimePairs.Count];
        }
        for (int i = 0; i < indices.Length; i++)
        {
            transform.GetChild(i).GetComponent<DragHandler>().EditSetup(isEditMode);
            
            if (!isEditMode)
            {
                int index = transform.GetChild(i).GetComponent<SwipeToDelete>().index;
                if (index != i)
                {
                    isSwap = true;
                }
                transform.GetChild(i).GetComponent<SwipeToDelete>().index = i;
                indices[i] = index;
            }     
        }
        if (isSwap)
        {
            gameManager.SwitchIndex(indices, mode);
            if (mode != 0)
            {
                bar.UpdateColorPercentages();
            }
            isSwap = false;
        }

    }

    public void SwapAfterDrag(string tag)
    {
        if (tag == "Interval")
        {
            indices = new int[gameManager.nameTimePairs.Count];
        }
        else
        {
            indices = new int[gameManager.sequences.Count];
        }
        
        for (int i = 0; i < indices.Length; i++)
        {

            int index = transform.GetChild(i).GetComponent<SwipeToDelete>().index;
            if (index != i)
            {
                isSwap = true;
            }
            transform.GetChild(i).GetComponent<SwipeToDelete>().index = i;
            indices[i] = index; 
        }
        if (isSwap)
        {
            gameManager.SwitchIndex(indices, mode);
            if (mode != 0)
            {
                bar.UpdateColorPercentages();
            }
            isSwap = false;
        }
    }
}
