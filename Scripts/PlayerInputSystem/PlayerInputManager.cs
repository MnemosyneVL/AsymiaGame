using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private FirstPersonController3D _playerController;
    [SerializeField]
    private WeaponHolder _weaponHolder;

    private void Update()
    {
        UpdateMouseLook();
        UpdateMovement();
        UpdateShootingControls();
        UpdateAbilitiesControls();
    }

    private void FixedUpdate()
    {

    }

    //MouseLook Update
    private void UpdateMouseLook()
    {
        //reading input from mouse
        Vector2 mouseDeltaInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        _playerController.ChangeLookOnInput(mouseDeltaInput);
    }
    //Movement Update
    private void UpdateMovement()
    {
        //calculating input
        Vector2 inputDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _playerController.MovePlayerRelativeOnInput(inputDir);
        if(Input.GetButtonDown("Jump"))
        {
            _playerController.Jump(); 
        }

    }
    //WeaponControls Update
    private void UpdateShootingControls()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _weaponHolder.ActivateCurrentWeapon();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            _weaponHolder.DeactivateCurrentWeapon();
        }
    }

    private void UpdateAbilitiesControls()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _weaponHolder.ActivateAbility(0);
        }
        if(Input.GetKeyUp(KeyCode.E))
        {
            _weaponHolder.DeactivateAbility(0);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            _weaponHolder.ActivateAbility(1);
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            _weaponHolder.DeactivateAbility(1);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            _weaponHolder.ActivateAbility(2);
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            _weaponHolder.DeactivateAbility(2);
        }
    }
}
