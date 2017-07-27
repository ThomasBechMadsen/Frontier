using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] //To show in the editor
public class ItemData{

    public string itemName;
    public float weight;
    public Sprite sprite;
    public GameObject worldItem;
    public GameObject handsItem;

    public ItemData(string itemName, float weight, Sprite sprite, GameObject worldItem, GameObject handsItem)
    {
        this.itemName = itemName;
        this.weight = weight;
        this.sprite = sprite;
        this.worldItem = worldItem;
        this.handsItem = handsItem;
    }

}
