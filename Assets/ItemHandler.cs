using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour {

    bool working;
    public Camera playerCamera;
    public GameObject inventoryCanvas;


    public bool swingAxe(Axe axe)
    {
        if (!working)
        {
            working = true;
            StartCoroutine(swingAxeRoutine(axe.treeDamage, axe.cooldownTime));
            return true;
        }
        return false;
    }

    public bool fireSingleShotWeapon(Rifle rifle)
    {
        if (!working)
        {
            working = true;
            StartCoroutine(fireSingleShotRifleRoutine(rifle));
            return true;
        }
        return false;
    }

    public bool reloadWeapon(Rifle rifle)
    {
        if (!working)
        {
            working = true;
            StartCoroutine(reloadRoutine(rifle));
            return true;
        }
        return false;
    }

    public void aimWeapon(bool aim)
    {
        transform.Find("Main Camera").GetComponent<Crosshair>().drawCrosshair(aim);
    }

    private IEnumerator reloadRoutine(Rifle rifle)
    {
        GameObject ammo = inventoryCanvas.GetComponent<InventoryNew>().getFirstOccurenceOf("Ammo");
        if (!rifle.loaded && ammo != null)
        {
            print("reloading...");
            yield return new WaitForSeconds(rifle.reloadTime);
            inventoryCanvas.GetComponent<InventoryNew>().deleteItem(ammo);
            rifle.loaded = true;
            working = false;
            print("Reloaded!");
        }
        else{
            print("Unable to reload");
        }
    }

    private IEnumerator fireSingleShotRifleRoutine(Rifle rifle)
    {
        if (rifle.loaded) {
            print("Bang!");
            animalNoiseAlert(rifle.noiseDistance);
            RaycastHit raycastHit;
            Vector3 rayDirection;
            if (rifle.aimed) //Add inaccuracy
            {
                rayDirection = new Vector3(Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f), 1);
            }
            else
            {
                rayDirection = new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), 1);
            }
            rayDirection = playerCamera.transform.TransformDirection(rayDirection);
            Ray ray = new Ray(playerCamera.transform.position, rayDirection);
            if (Physics.Raycast(ray, out raycastHit))
            {
                Debug.DrawLine(playerCamera.transform.position, raycastHit.point, Color.red, 10);
                print("initially hit: " + raycastHit.transform.name);
                if (raycastHit.collider.gameObject.tag == "Animal")
                {
                    raycastHit.collider.gameObject.GetComponent<AnimalHealth>().dealDamage(rifle.damage);
                }
            }
            yield return new WaitForSeconds(rifle.reloadTime);
        }
        else{
            print("click!");
            yield return new WaitForSeconds(rifle.clickTime);
        }
        working = false;
    }

    private IEnumerator swingAxeRoutine(float sharpness, float cooldownTime)
    {
            print("Swing!");
            RaycastHit raycastHit;
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            if (Physics.Raycast(ray, out raycastHit, 2))
            {
            Debug.DrawLine(playerCamera.transform.position, raycastHit.point, Color.red, 10);
            print("initially hit: " + raycastHit.transform.name);

                if (raycastHit.collider.gameObject.tag == "Tree")
                {
                    raycastHit.collider.gameObject.GetComponent<TreeScript>().chop(sharpness, transform.right);
                }
            }
            yield return new WaitForSeconds(cooldownTime);
            working = false;
    }

    private void animalNoiseAlert(int soundDistance) //Alert all animals within a certain distance by sound
    {
        GameObject[] animals = GameObject.FindGameObjectsWithTag("Animal");
        foreach(GameObject animal in animals)
        {
            DeerAI ai = animal.GetComponent<DeerAI>();
            float distance = Vector3.Distance(animal.transform.position, transform.position);
            //print(soundDistance);
            //print((distance - ai.scaredHearingDistance));
            if ((distance - ai.scaredHearingDistance) <= soundDistance)
            {
                ai.setState(DeerState.Scared);
            }
            else if ((distance - ai.onEdgeHearingDistance) <= soundDistance)
            {
                ai.increaseState();
            }
        }
    }
}
