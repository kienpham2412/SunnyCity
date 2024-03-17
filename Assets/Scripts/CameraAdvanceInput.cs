using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CMF;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

public class CameraAdvanceInput : CameraInput
{
    private InputMaster inputMaster;
    private Vector2 rotation;

    private void Awake()
    {
        inputMaster = new InputMaster();
        inputMaster.Player.LookAround.performed += LookAroundAction;
    }

    void OnEnable()
    {
        inputMaster.Enable();
    }

    private void LookAroundAction(CallbackContext ctx)
    {
        rotation = ctx.ReadValue<Vector2>();
    }

    public override float GetHorizontalCameraInput()
    {
        return rotation.x;
    }

    public override float GetVerticalCameraInput()
    {
        return rotation.y;
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        inputMaster.Disable();
    }
}
