using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : UsableItem {

    public bool loaded;
    public float reloadTime;
    public float clickTime;
    public float damage;
    public bool aimed = false;
    public int noiseDistance;


    public Rifle(ItemHandler handler, string itemName, float weight, Sprite sprite, GameObject worldObject, float damage, int noiseDistance, float reloadTime, float clickTime, bool loaded)
        : base(handler, itemName, weight, sprite, worldObject)
    {
        this.reloadTime = reloadTime;
        this.loaded = loaded;
        this.damage = damage;
        this.clickTime = clickTime;
        this.noiseDistance = noiseDistance;
    }

    override
    public void use()
    {
        if(handler.fireSingleShotWeapon(this)){
            loaded = false;
        }
    }

    public void reload()
    {
        handler.reloadWeapon(this);

    }

    public void aim(bool aim)
    {
        aimed = aim;
        handler.aimWeapon(aim);
    }

}
