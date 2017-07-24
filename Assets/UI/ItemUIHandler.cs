using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//https://www.youtube.com/watch?v=c47QYgsJrWc

public class ItemUIHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    public static GameObject itemBeingDragged;
    Vector3 startPosition;
    Transform startParent;


    public void OnBeginDrag(PointerEventData eventData)
    {
        itemBeingDragged = gameObject;
        startPosition = transform.position;
        startParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemBeingDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        Transform hands = GameObject.Find("Hands").transform.GetChild(0).GetChild(0); //Do better
        if (transform.parent == startParent)
        {
            transform.position = startPosition;
        }
        else if(transform.parent == hands || startParent == hands)
        {
            GameObject.Find("InventoryCanvas").GetComponent<Inventory>().updateHandsModel();
        }
    }

}
