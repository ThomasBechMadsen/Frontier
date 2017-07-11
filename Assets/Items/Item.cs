using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] //To show in the editor
public class Item{

    public string itemName;
    public float weight;
    public Sprite sprite;
    public GameObject worldObject;

    public Item(string itemName, float weight, Sprite sprite, GameObject worldObject)
    {
        this.itemName = itemName;
        this.weight = weight;
        this.sprite = sprite;
        this.worldObject = worldObject;
}
}
