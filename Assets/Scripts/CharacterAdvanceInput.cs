using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CMF;
using UnityEngine.InputSystem;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

public class CharacterAdvanceInput : CharacterInput
{
    private InputMaster inputMaster;
    private Vector2 moveDir;
    private bool isJumpKeyPressed;
    private bool isSprintKeyPressed;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        inputMaster = new InputMaster();
        inputMaster.Player.Move.performed += MoveAction;
        inputMaster.Player.Jump.performed += JumpAction;
        inputMaster.Player.Sprint.performed += SprintAction;
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        inputMaster.Enable();
    }

    private void MoveAction(CallbackContext ctx)
    {
        moveDir = ctx.ReadValue<Vector2>();
    }

    private void JumpAction(CallbackContext ctx)
    {
        isJumpKeyPressed = ctx.ReadValue<float>() >= 0.9f;
    }

    private void SprintAction(CallbackContext ctx)
    {
        isSprintKeyPressed = ctx.ReadValue<float>() >= 0.9f;
    }

    public override float GetHorizontalMovementInput()
    {
        return moveDir.x;
    }

    public override float GetVerticalMovementInput()
    {
        return moveDir.y;
    }

    public override bool IsJumpKeyPressed()
    {
        return isJumpKeyPressed;
    }

    public override bool IsSprintKeyPressed()
    {
        return isSprintKeyPressed;
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        inputMaster.Disable();
    }
}
