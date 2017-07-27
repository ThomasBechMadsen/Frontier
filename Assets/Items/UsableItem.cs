using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UsableItem : ItemData {

    protected ItemHandler handler;

    public UsableItem(ItemHandler handler, string itemName, float weight, Sprite sprite, GameObject worldItem, GameObject handsItem) : base(itemName, weight, sprite, worldItem, handsItem)
    {
        this.handler = handler;
    }

    public abstract void use();
}
