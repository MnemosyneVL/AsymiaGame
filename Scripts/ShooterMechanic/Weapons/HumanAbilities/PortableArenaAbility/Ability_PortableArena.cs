using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_PortableArena : BasicAbility
{
    [Header("Settings")]
    [SerializeField]
    private float _secondsToWork = 30f;

    [Header("References")]
    [SerializeField]
    private GameObject _arenaPrefab;
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
        EnableAbilityEffect();
    }

    //Internal Logic========================================================================================================================================================
    private void EnableAbilityEffect()
    {
        Debug.Log("Arena Activated");
        GameObject healZone = Instantiate(_arenaPrefab, MainReferenceHolder.Instance.GetPlayerMovementController().GetOriginPoint(), Quaternion.identity);
        healZone.GetComponent<PortableArenaManager>().ActivatePortableArena(_secondsToWork);
        _currentCooldown = _usageCooldown;
    }
}
