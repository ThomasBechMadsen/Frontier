using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : UsableItem
{

    public float treeDamage;
    public float cooldownTime;

    public Axe(ItemHandler handler, string itemName, float weight, Sprite sprite, GameObject worldObject, float sharpness, float cooldownTime)
        : base(handler, itemName, weight, sprite, worldObject)
    {
        this.treeDamage = sharpness;
        this.cooldownTime = cooldownTime;
    }

    override
    public void use()
    {
        handler.swingAxe(this);
    }
}
