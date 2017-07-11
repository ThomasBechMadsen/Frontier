using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UsableItem : Item {

    protected ItemHandler handler;

    public UsableItem(ItemHandler handler, string itemName, float weight, Sprite sprite, GameObject worldObject) : base(itemName, weight, sprite, worldObject)
    {
        this.handler = handler;
    }

    public abstract void use();
}
