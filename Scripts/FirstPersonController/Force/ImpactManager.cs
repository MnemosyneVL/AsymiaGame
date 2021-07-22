using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float _mass = 3.0f; // defines the character mass
    [Header("References")]
    [SerializeField]
    private CharacterController _characterController;

    //other fields
    Vector3 _impact = Vector3.zero;


    //Public Methods========================================================================================================================================================

    //Add Impact method: Adds impact----------------------------------------------------------------------------------------------------------------------------------------
    public void AddImpact(Vector3 direction, float force)
    {
        direction.Normalize();
        if (direction.y < 0) direction.y = -direction.y; // reflect down force on the ground
        _impact += direction.normalized * force / _mass;
    }

    //Update Methods========================================================================================================================================================

    //Unity Update----------------------------------------------------------------------------------------------------------------------------------------------------------
    void Update()
    {
        // apply the impact force:
        if (_impact.magnitude > 0.2f) _characterController.Move(_impact * Time.deltaTime);
        // consumes the impact energy each cycle:
        _impact = Vector3.Lerp(_impact, Vector3.zero, 5 * Time.deltaTime);
    }
}
