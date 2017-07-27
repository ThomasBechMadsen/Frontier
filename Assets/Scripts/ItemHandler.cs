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

    public void aimWeapon(Rifle rifle)
    {
        StartCoroutine(aimRoutine(rifle));
    }

    private IEnumerator aimRoutine(Rifle rifle)
    {
        Animation anim = playerCamera.transform.GetChild(0).GetChild(0).GetComponent<Animation>();
        AnimationState animState = anim.PlayQueued("AimAnimation", QueueMode.PlayNow);
        if (rifle.aimed)
        {
            animState.speed = -1;
            animState.time = anim.clip.length;
        }
        rifle.aimed = !rifle.aimed;
        yield return new WaitForSeconds(anim.GetClip("AimAnimation").length);
        if (rifle.aimed) {
            playerCamera.transform.GetChild(0).GetChild(0).localPosition = rifle.aimedPosition;
        }
    }

    private IEnumerator reloadRoutine(Rifle rifle)
    {
        GameObject ammo = inventoryCanvas.GetComponent<Inventory>().getFirstOccurenceOf("Ammo");
        if (!rifle.loaded && ammo != null)
        {
            print("reloading...");
            yield return new WaitForSeconds(rifle.reloadTime);
            inventoryCanvas.GetComponent<Inventory>().deleteItem(ammo);
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
            RaycastHit raycastHit;
            Vector3 rayDirection;
            animalNoiseAlert(rifle.noiseDistance);
            rifle.loaded = false;
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
                print("Hit: " + raycastHit.transform.name);
                if (raycastHit.collider.tag == "Animal")
                {
                    raycastHit.collider.transform.parent.gameObject.GetComponent<AnimalHealth>().dealDamage(rifle.damage);
                }
            }
            playerCamera.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<ParticleSystem>().Play();
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
            DeerAI ai = animal.transform.parent.gameObject.GetComponent<DeerAI>();
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
