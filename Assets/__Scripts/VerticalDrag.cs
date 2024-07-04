using System.Collections;
using System.Collections.Generic;
using Lean.Transition;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class VerticalDrag : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform currentTransform;
    private GameObject mainContent;
    private Vector3 currentPossition;

    private int totalChild;
    public bool isEdit;
    public Button copyButton;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isEdit)
        {
            return;
        }
        currentPossition = currentTransform.position;
        mainContent = currentTransform.parent.gameObject;
        totalChild = mainContent.transform.childCount;
        copyButton.enabled = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
    if (!isEdit)
        {
            return;
        }
        currentTransform.position =
            new Vector3(currentTransform.position.x, eventData.position.y, currentTransform.position.z);

        for (int i = 0; i < totalChild; i++)
        {
            if (i != currentTransform.GetSiblingIndex())
            {
                Transform otherTransform = mainContent.transform.GetChild(i);
                int distance = (int) Vector3.Distance(currentTransform.position,
                    otherTransform.position);
                if (distance <= 10)
                {
                    Vector3 otherTransformOldPosition = otherTransform.position;
                    otherTransform.position = new Vector3(otherTransform.position.x, currentPossition.y,
                        otherTransform.position.z);
                    currentTransform.position = new Vector3(currentTransform.position.x, otherTransformOldPosition.y,
                        currentTransform.position.z);
                    currentTransform.SetSiblingIndex(otherTransform.GetSiblingIndex());
                    currentPossition = currentTransform.position;
                }
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isEdit)
        {
            return;
        }
        currentTransform.position = currentPossition;
        transform.parent.parent.parent.GetComponent<EditMode>().SwapAfterDrag(transform.parent.parent.tag);
        copyButton.enabled = true;
    }

    
}
