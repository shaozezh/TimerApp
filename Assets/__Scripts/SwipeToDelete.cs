using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SwipeToDelete : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform topView;

    private Vector3 mouseStartPosition;
    private Vector3 topViewStartPosition;

    private float horizontalDragMinDistance = -30f;
    private float verticalDragMinDistance = 8f;

    private bool horizontalDrag = false;
    private bool verticalDrag = false;
    //public bool isReady = false;
    public int index;

    public void OnBeginDrag(PointerEventData eventData)
    {
        PanelManager panelManager = FindObjectOfType<PanelManager>();
        if (!panelManager.CheckReady())
        {
            return;
        }
        mouseStartPosition = Input.mousePosition;
        topViewStartPosition = topView.localPosition;
        ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, eventData, ExecuteEvents.beginDragHandler);
    }

    public void OnDrag(PointerEventData eventData)
    {
        PanelManager panelManager = FindObjectOfType<PanelManager>();
        if (!panelManager.CheckReady())
        {
            return;
        }
        Vector3 distance = Input.mousePosition - mouseStartPosition;

        if (!horizontalDrag && !verticalDrag)
        {
            if (distance.x < horizontalDragMinDistance)
            {
                horizontalDrag = true;
                return;
            } 
            else if (Mathf.Abs(distance.y) > verticalDragMinDistance)
            {
                verticalDrag = true;
                return;
            }
        }

        if (horizontalDrag)
        {
            float newPositionX = topViewStartPosition.x + Mathf.Min(0f, distance.x);
            topView.localPosition = new Vector3(newPositionX, topViewStartPosition.y, topViewStartPosition.z);
        } 
        else if (verticalDrag)
        {
            ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, eventData, ExecuteEvents.dragHandler);
        }
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        PanelManager panelManager = FindObjectOfType<PanelManager>();
        if (!panelManager.CheckReady())
        {
            return;
        }
        if (horizontalDrag)
        {
            Vector3 distance = Input.mousePosition - mouseStartPosition;

            if (distance.x < -204.0f)
            {
                RemoveTimer();
            }
            else
            {
                topView.localPosition = topViewStartPosition;
            }
        }

        horizontalDrag = false;
        verticalDrag = false;
        ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, eventData, ExecuteEvents.endDragHandler);
    }

    public void SetIndex(int newIndex)
    {
        index = newIndex;
    }

    public void RemoveTimer()
    {
        FindObjectOfType<AddSequence>().currentIndex--;
        if (transform.parent.childCount == 1)
        {
            EditMode editMode = transform.parent.GetComponent<EditMode>();
            GameObject editButton = editMode.editButton;
            editButton.GetComponent<Button>().enabled = false;
            editButton.transform.GetChild(3).gameObject.SetActive(true);
            if (editMode.playButton != null)
            {
                editMode.playButton.GetComponent<Button>().enabled = false;
                editMode.playButton.transform.GetChild(3).gameObject.SetActive(true);
            }
        }   
        if (gameObject.tag == "Interval")
        {
            FindObjectOfType<StoreTimer>().RemovePairAtIndex(index);
            transform.parent.GetComponent<DecrementIndex>().UpdateIndexAfter(index);
            FindObjectOfType<ColorPercentageBar>().UpdateColorPercentages();
        }
        else
        {
            FindObjectOfType<StoreTimer>().RemoveNameAtIndex(index);
            transform.parent.GetComponent<DecrementIndex>().UpdateIndexAfter(index);
        }    
        Destroy(gameObject);
    }

    public void DuplicateTimer()
    {
        GameObject newTimer = Instantiate(this.gameObject, this.transform.parent);
        newTimer.name = this.gameObject.name;
        newTimer.transform.SetSiblingIndex(this.transform.GetSiblingIndex() + 1);
        for (int i = index+1; i < transform.parent.childCount; i++)
        {
            transform.parent.GetChild(i).GetComponent<SwipeToDelete>().index++;
            //transform.parent.GetChild(i).SetSiblingIndex(this.transform.GetSiblingIndex() + 1 + i - index);
        }
        if (gameObject.tag == "Timer")
        {
            transform.parent.GetComponent<EditMode>().gameManager.InsertToList(index, 0);
        }
        else
        {
            transform.parent.GetComponent<EditMode>().gameManager.InsertToList(index, 1);
            FindObjectOfType<ColorPercentageBar>().UpdateColorPercentages();
        }

    }
}
