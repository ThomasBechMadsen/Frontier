using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float walkSpeed;
    public float sprintSpeed;
    public float crouchSpeed;
    public float proneSpeed;
    //private Rigidbody rigidBody;
    private CharacterController characterController;
    public float mouseSensitivity = 5.0f;
    public float mouseSmoothing = 2.0f;
    private float moveMultiplier;
    private float verticalSpeed = 0;
    private enum Stance {standing, crouched, prone};
    private Stance currentStance = Stance.standing;
    private InventoryNew inventory;
    private Camera playerCamera;
    private bool usingInventory = false;

    Vector2 smoothV;
    Vector2 mouseLook;

    // Use this for initialization
    void Start () {
        //rigidBody = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        playerCamera = transform.Find("Main Camera").GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        moveMultiplier = walkSpeed;
        inventory = transform.Find("InventoryCanvas").GetComponent<InventoryNew>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Inventory"))
        {
            if (inventory.UIItemInHands.transform.parent.gameObject.activeSelf && inventory.UIInventory.transform.parent.gameObject.activeSelf)
            {
                inventory.hideInventory();
                Cursor.lockState = CursorLockMode.Locked;
                usingInventory = false;
            }
            else
            {
                inventory.showInventory();
                Cursor.lockState = CursorLockMode.None;
                usingInventory = true;
            }
        }

        if (!usingInventory) {
            movementHandle();
            rotationHandle();
            stanceHandle();

            //Shooting
            if (Input.GetButtonDown("Fire1"))
            {
                if (inventory.getItemInHands() is UsableItem)
                {
                    UsableItem a = inventory.getItemInHands() as UsableItem;
                    a.use();
                }
            }

            //Reloading
            if (Input.GetButtonDown("Reload"))
            {
                if (inventory.getItemInHands() is Rifle) {
                    Rifle rifle = inventory.getItemInHands() as Rifle;
                    rifle.reload();
                }
            }

            if (Input.GetButtonDown("Aim"))
            {
                if (inventory.getItemInHands() is Rifle)
                {
                    Rifle rifle = inventory.getItemInHands() as Rifle;
                    rifle.aim(true);
                }
            }

            if (Input.GetButtonUp("Aim"))
            {
                if (inventory.getItemInHands() is Rifle)
                {
                    Rifle rifle = inventory.getItemInHands() as Rifle;
                    rifle.aim(false);
                }
            }

            if (Input.GetButtonDown("Interact"))
            {
                RaycastHit raycastHit;
                Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
                if (Physics.Raycast(ray, out raycastHit, 2))
                {
                    //Debug.DrawLine(playerCamera.transform.position, raycastHit.point, Color.red, 10);
                    if (raycastHit.collider.gameObject.CompareTag("Pickupable"))
                    {
                        if (inventory.addItem(raycastHit.collider.transform.GetChild(0).GetComponent<pickupScript>().itemYield))
                        {
                            Destroy(raycastHit.collider.gameObject);
                        }
                    }
                }
            }
        }
    }


    private void setStance(Stance stance)
    {
        switch (stance)
        {
            case Stance.standing: //Max height = 2.0
                currentStance = Stance.standing;
                characterController.height = 1.6f;
                characterController.radius = 0.4f;
                moveMultiplier = walkSpeed;
                break;
            case Stance.crouched: //Max height = 1.5
                currentStance = Stance.crouched;
                characterController.height = 1.1f;
                characterController.radius = 0.4f;
                moveMultiplier = crouchSpeed;
                break;
            case Stance.prone:    //Max height = 1.0
                currentStance = Stance.prone;
                characterController.height = 0f;
                characterController.radius = 0.35f;
                moveMultiplier = proneSpeed;
                break;
        }
    }

    private void movementHandle()
    {
        Vector3 moveVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        moveVector = moveVector * moveMultiplier;

        verticalSpeed -= 9.82f;
        if (characterController.isGrounded)
        {
            verticalSpeed = 0;
        }
        moveVector.y = verticalSpeed;

        characterController.Move(transform.TransformDirection(moveVector * Time.deltaTime));
    }

    private void rotationHandle()
    {
        //camera direction and character rotation
        Vector2 mouseCoords = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        mouseCoords = Vector2.Scale(mouseCoords, new Vector2(mouseSmoothing * mouseSensitivity, mouseSmoothing * mouseSensitivity));

        smoothV.x = Mathf.Lerp(smoothV.x, mouseCoords.x, 1f / mouseSmoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, mouseCoords.y, 1f / mouseSmoothing);
        mouseLook += smoothV;
        mouseLook.y = Mathf.Clamp(mouseLook.y, -90, 90);

        transform.Find("Main Camera").transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        transform.localRotation = Quaternion.AngleAxis(mouseLook.x, transform.up);
    }

    private void stanceHandle()
    {
        //Playerstances
        if (Input.GetButtonDown("Prone")) //TODO Transform player to not clip into ground
        {
            if (currentStance != Stance.prone)
            { //Max height = 1.0
                setStance(Stance.prone);
            }
            else
            {
                setStance(Stance.standing);
            }
        }
        else
        {
            if (Input.GetButtonDown("Crouch")) //Max height = 1.5
            {
                setStance(Stance.crouched);
            }
            if (Input.GetButtonUp("Crouch"))
            {
                setStance(Stance.standing);
            }
        }
        if (currentStance == Stance.standing)
        {
            if (Input.GetButtonDown("Sprint"))
            {
                moveMultiplier = sprintSpeed;
            }
            if (Input.GetButtonUp("Sprint"))
            {
                moveMultiplier = walkSpeed;
            }
        }
    }
}
