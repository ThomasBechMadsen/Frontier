﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject itemObject;
    public GameObject UIInventory;
    public GameObject UIItemInHands;
    public GameObject UIWeight;
    private Transform itemModelHolder;

    void Start()
    {
        itemModelHolder = transform.parent.GetChild(0).GetChild(0);
        addItem(new Rifle(transform.parent.GetComponent<ItemHandler>(), "Old Trusty", 5, null, (GameObject)Resources.Load("worldItems/MusketWorld"), (GameObject)Resources.Load("handsItems/MusketHands"), 100, 100, 3, 1, true));
        addItem(new ItemData("Ammo", 0.1f, null, null, null));
        addItem(new Axe(transform.parent.GetComponent<ItemHandler>(), "Axe", 3, null, null, null, 100, 1));
        //deleteItem(getObject(0));
        //dropItem(getObject(0));

    }

    public bool addItem(ItemData item)
    {
        foreach(Transform slot in UIInventory.transform)
        {
            if (slot.childCount == 0)
            {
                GameObject newItem = Instantiate(itemObject, slot);
                newItem.GetComponent<worldObjectItemContainer>().item = item;
                newItem.GetComponent<Image>().sprite = item.sprite;
                newItem.transform.Find("Text").GetComponent<Text>().text = item.itemName;
                return true;
            }
        }
        print("Inventory full");
        return false;
    }

    public bool deleteItem(GameObject item)
    {
        foreach (Transform slot in UIInventory.transform)
        {
            if (slot.childCount > 0)
            {
                if (slot.GetChild(0).gameObject == item)
                {
                    Destroy(slot.GetChild(0).gameObject);
                    return true;
                }
            }
        }
        print("Item not found");
        return false;
    }

    public void dropItem(GameObject item)
    {
        print("Dropping item");
        deleteItem(item);
        GameObject g = Instantiate(item.GetComponent<worldObjectItemContainer>().item.worldItem, transform.parent.TransformPoint(Vector3.forward), Quaternion.Euler(Vector3.forward));
        g.GetComponent<Rigidbody>().isKinematic = false;
        g.GetComponent<MeshCollider>().enabled = true;
        g.GetComponent<worldObjectItemContainer>().item = item.GetComponent<worldObjectItemContainer>().item;
        /*if (item == itemInHands)
        {
            for (int i = 0; i < itemInHandsWorld.transform.childCount; i++)
            {
                Destroy(itemInHandsWorld.transform.GetChild(i).gameObject);
            }
        }*/
    }

    public void showInventory()
    {
        UIInventory.transform.parent.gameObject.SetActive(true);
        UIItemInHands.transform.parent.gameObject.SetActive(true);
        UIWeight.GetComponent<Text>().text = "Weight: " + getInventoryWeight() + " kgs";
    }

    public void hideInventory()
    {
        UIInventory.transform.parent.gameObject.SetActive(false);
        UIItemInHands.transform.parent.gameObject.SetActive(false);
    }

    public GameObject getFirstOccurenceOf(string name)
    {
        foreach (Transform slot in UIInventory.transform)
        {
            if (slot.childCount > 0)
            {
                if (getSlotItemData(slot).itemName.Equals(name))
                {
                    return slot.GetChild(0).gameObject;
                }
            }
        }
        return null;
    }

    public float getInventoryWeight()
    {
        float totalWeight = 0;
        foreach (Transform slot in UIInventory.transform)
        {
            if (slot.childCount > 0)
            {
                totalWeight += getSlotItemData(slot).weight;
            }
        }
        return totalWeight;
    }

    public ItemData getItemInHands()
    {
        if (UIItemInHands.transform.GetChild(0).childCount > 0)
        {
            return getSlotItemData(UIItemInHands.transform.GetChild(0));
        }
        return null;
    }

    public void updateHandsModel()
    {
        if (itemModelHolder.childCount > 0)
        {
            Destroy(itemModelHolder.GetChild(0).gameObject);
        }
        if (getItemInHands() != null) {
            if (getItemInHands().handsItem != null)
            {
                Instantiate(getItemInHands().handsItem, itemModelHolder, false);
            }
        }
    }

    private ItemData getSlotItemData(Transform slot)
    {
        return slot.GetChild(0).GetComponent<worldObjectItemContainer>().item;
    }

    private GameObject getObject(int i)
    {
        return UIInventory.transform.GetChild(i).transform.GetChild(0).gameObject;
    }
}