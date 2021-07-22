using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_ThrowStone : BasicAbility
{
    [Header("Settings")]
    [SerializeField]
    private float _damage = 10f;
    [SerializeField]
    private float _explosionRadius = 5f;

    [Header("References")]
    [SerializeField]
    private GameObject _rockPrefab;
    [SerializeField]
    private AbilitiesUIManager _abilitiesUI;

    //Other global fields

    //Update Methods========================================================================================================================================================

    //Unity Update
    private void Update()
    {
        if (_currentCooldown >= 0f)
        {
            //_abilitiesUI.DisableAbility();
            _currentCooldown -= Time.deltaTime;
            //_abilitiesUI.UpdateCooldownNr(_currentCooldown);
        }
        else 
        {
            //_abilitiesUI.EnableAbility();
        }

    }

    //Public BasicWeapon Override Methods===================================================================================================================================

    //
    public override bool CanUse()
    {
        return _currentCooldown <= 0f;
    }

    public override void OnActivate()
    {
        Debug.Log("Rock Throw Activated");
        Transform ownerTopmostPartent = _ownerCollider.transform.root;
        FirstPersonController3D monsterMovementController = ownerTopmostPartent.gameObject.GetComponentInChildren<FirstPersonController3D>();
        GameObject rockObject = Instantiate(_rockPrefab, monsterMovementController.GetCameraHolder().position, Quaternion.identity);
        rockObject.GetComponent<ThrowStoneScript>().ActivateStone(_ownerCollider , monsterMovementController.GetCameraHolder().forward, _explosionRadius, _damage);
        _currentCooldown = _usageCooldown;
    }

    //ML methods-----------------------------------------------------------------------------------------------------------------------------------------------------------

    public override float RequestCooldown()
    {
        return _currentCooldown;
    }
}
