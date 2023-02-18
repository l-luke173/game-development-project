using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Camera currentCamera;
    public GameObject cursor;
    public float moveSpeed;
    public float lookSpeed;
    private Vector2 currentMove;
    private Vector2 currentControllerLook;
    private Vector2 currentMouseLook;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Update()
    {
        //Use the y axis on currentMove to control camera zoom (FOV).
        float yMove = currentMove[1] * Time.deltaTime * moveSpeed;
        if (yMove > 0.01 || yMove < -0.01)
        {
            Debug.Log(currentCamera.fieldOfView - yMove);
            if (currentCamera.fieldOfView - yMove < 60 && currentCamera.fieldOfView - yMove > 15)
                currentCamera.fieldOfView -= yMove;
        }

        //Use the x and y axis on currentLook to control cursor.
        float xLook = ((float)currentControllerLook[0] * Time.deltaTime * lookSpeed) + Mouse.current.delta.ReadValue().x;
        float yLook = ((float)currentControllerLook[1] * Time.deltaTime * lookSpeed) + Mouse.current.delta.ReadValue().y;

        Vector3 newLook = new Vector3(xLook, yLook);
        if (cursor.transform.localPosition.x + newLook.x > Screen.width / 2 || cursor.transform.localPosition.x + newLook.x < -Screen.width / 2)
            newLook = new Vector3(0, newLook.y);
        if (cursor.transform.localPosition.y + newLook.y > Screen.height / 2 || cursor.transform.localPosition.y + newLook.y < -Screen.height / 2)
            newLook = new Vector3(newLook.x, 0);
        cursor.transform.localPosition += newLook;

        currentCamera.gameObject.transform.localRotation = Quaternion.Euler(-cursor.transform.localPosition.y / Screen.height * 27, cursor.transform.localPosition.x / Screen.width * 48, 0f);

        Debug.Log(Mouse.current.delta.ReadValue());
        Debug.Log(currentMouseLook);
    }

    //This method uses the wasd keys and right stick on the gamepad.
    public void Move(InputAction.CallbackContext value)
    {
        //Store the most recent Vector2 value from the input.
        currentMove = value.ReadValue<Vector2>();
    }

    //This method uses the left stick on the gamepad.
    public void Look(InputAction.CallbackContext value)
    {
        //Store the most recent Vector2 value from the input.
        currentControllerLook = value.ReadValue<Vector2>();
    }
}