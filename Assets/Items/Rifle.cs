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
    public Vector3 aimedPosition = new Vector3(0, 0.243f, -0.412f);
    public float aimTransitionTime = 1;


    public Rifle(ItemHandler handler, string itemName, float weight, Sprite sprite, GameObject worldItem, GameObject handsItem, float damage, int noiseDistance, float reloadTime, float clickTime, bool loaded)
        : base(handler, itemName, weight, sprite, worldItem, handsItem)
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
        handler.fireSingleShotWeapon(this);
    }

    public void reload()
    {
        handler.reloadWeapon(this);

    }

    public void aim()
    {
        handler.aimWeapon(this);
    }

}
