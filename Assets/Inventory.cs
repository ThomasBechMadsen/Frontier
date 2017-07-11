using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    public float maxWeight;
    private List<Item> items = new List<Item>();
    Item handsItem = new Item("Empty Hands", 0, null, null);
    public Item itemInHands;
    public GameObject itemInHandsWorld;
    public Canvas inventoryCanvas;
    public GameObject itemUIObject;
    public bool uiEnabled = false;

    //TEST
    public GameObject musketTestObject;

    void Start() // Testing
    {
        itemInHands = handsItem;
        addItem(new Item("Ammo", 0.1f, null, null));
        addItem(new Item("Ammo", 0.1f, null, null));
        addItem(new Item("Ammo", 0.1f, null, null));
        addItem(new Rifle(GetComponent<ItemHandler>(),"Old Trusty", 1, null, musketTestObject, 100, 100, 3, 1, true));
        //addItem(new Axe(GetComponent<ItemHandler>(), "Wood Axe", 2, null, null, 100, 1));
        putInHands(items[3]);
    }

    private float getInventoryWeight()
    {
        float result = 0;
            foreach (Item item in items)
            {
                result += item.weight;
            }
            return result;
    }

    public bool addItem(Item item)
    {
        if (getInventoryWeight() + item.weight <= maxWeight)
        {
            items.Add(item);
            updateInventory();
            return true;
        }
        else
        {
            print("Too heavy!");
        }
        return false;
    }

    public bool removeItem(Item item)
    {
        bool b = items.Remove(item);
        updateInventory();
        return b;
    }

    public void dropItem(Item item)
    {
        print("Dropping item");
        GameObject g = Instantiate(item.worldObject, transform.TransformPoint(Vector3.forward), Quaternion.Euler(Vector3.forward)); //Adjust position?
        g.GetComponent<Rigidbody>().isKinematic = false;
        g.GetComponent<MeshCollider>().enabled = true;
        if (item == itemInHands)
        {
            for (int i = 0; i < itemInHandsWorld.transform.childCount; i++)
            {
                Destroy(itemInHandsWorld.transform.GetChild(i).gameObject);
            }
        }
    }

    public void putInHands(Item item)
    {
        if (!addItem(itemInHands))
        {
            dropItem(itemInHands);
        }

        itemInHands = item;
        for(int i = 0; i < itemInHandsWorld.transform.childCount; i++) {
            Destroy(itemInHandsWorld.transform.GetChild(i).gameObject);
        }
        if (item.worldObject != null) {
            Instantiate(item.worldObject, itemInHandsWorld.transform);
        }
        else
        {
            print("worldObject was null!");
        }
    }

    public Item getFirstOccurenceOf(string name)
    {
        foreach (Item item in items)
        {
            if(item.itemName.Equals(name))
            {
                return item;
            }
        }
        return null;
    }

    public void showInventory()
    {
        foreach(Item i in items)
        {
            GameObject uiI = Instantiate(itemUIObject, inventoryCanvas.transform.Find("UIInventory").transform);
            uiI.GetComponent<uiItemScript>().item = i;
            uiI.transform.Find("ItemImage").GetComponent<Image>().sprite = i.sprite;
            uiI.transform.Find("ItemImage").GetComponent<Image>().enabled = true;
        }
        if (itemInHands != handsItem)
        {
            GameObject uiItemInHands = inventoryCanvas.transform.Find("ItemInHands").gameObject;
            uiItemInHands.GetComponent<uiItemScript>().item = itemInHands;
            uiItemInHands.transform.Find("ItemImage").GetComponent<Image>().sprite = itemInHands.sprite;
            uiItemInHands.transform.Find("ItemImage").GetComponent<Image>().enabled = true;
        }
        inventoryCanvas.gameObject.SetActive(true);
        //uiInventory.gameObject.SetActive(true);
        uiEnabled = true;
    }

    public void hideInventory()
    {
        //uiInventory.gameObject.SetActive(false);
        inventoryCanvas.gameObject.SetActive(false);
        foreach (Transform child in inventoryCanvas.transform.Find("UIInventory").transform)
        {
            Destroy(child.gameObject);
        }
        uiEnabled = false;
    }

    public void updateInventory()
    {
        if (uiEnabled) {
            hideInventory();
            showInventory();
        }
    }
}
