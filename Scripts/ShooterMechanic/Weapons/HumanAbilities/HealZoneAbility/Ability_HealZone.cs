using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_HealZone : BasicAbility
{
    [Header("Settings")]
    [SerializeField]
    private float _healingPerTick = 10f;
    [SerializeField]
    private float _secondsToExist = 5f;
    [SerializeField, Tag]
    private string _useOnTag;

    [Header("References")]
    [SerializeField]
    private GameObject _healZonePrefab;
    [SerializeField]
    private AbilitiesUIManager _abilitiesUI;


    //Update Methods========================================================================================================================================================

    //Unity Update
    private void Update()
    {
        if (_currentCooldown >= 0)
        {
            _abilitiesUI.DisableAbility();
            _currentCooldown -= Time.deltaTime;
            _abilitiesUI.UpdateCooldownNr(_currentCooldown);
        }
        else
            _abilitiesUI.EnableAbility();
    }

    //Public BasicWeapon Override Methods===================================================================================================================================

    //
    public override bool CanUse()
    {
        return _currentCooldown <= 0;
    }

    public override void OnActivate()
    {
        Debug.Log("HealZone Activated");
        GameObject healZone = Instantiate(_healZonePrefab, MainReferenceHolder.Instance.GetPlayerMovementController().GetOriginPoint(), Quaternion.identity);
        healZone.GetComponent<HealZoneManager>().ActivateHealZone(_healingPerTick, _secondsToExist, _useOnTag);
        _currentCooldown = _usageCooldown;
    }

}
