using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InputBase : MonoBehaviour
{
    InputAction pressAction;
    protected virtual void Awake()
    {
        pressAction = new InputAction("touch", binding:"<Pointer>/press");

        pressAction.started += cn =>{
            if(cn.control.device is Pointer device){
                OnPressBegan(device.position.ReadValue());
            }
        };

        pressAction.performed += cn =>{
            if (cn.control.device is Pointer device){
                OnPress(device.position.ReadValue());
            }
        };

        pressAction.canceled += _ => OnRelease();
    }

    protected virtual void OnEnable()
    {
        pressAction.Enable();
    }

    protected virtual void OnDisable() {

        pressAction.Disable();

    }

    protected virtual void OnDestroy() {

        pressAction.Dispose();
        
    }

    protected virtual void OnPress(Vector3 position){}

    protected virtual void OnPressBegan(Vector3 position){}

    protected virtual void OnRelease(){}
    
}
