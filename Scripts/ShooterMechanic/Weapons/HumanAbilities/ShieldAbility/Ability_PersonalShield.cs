using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability_PersonalShield : BasicAbility
{
    [Header("Settings")]
    [SerializeField]
    private float _damageMultipyer = 0.25f;
    [SerializeField]
    private float _secondsToWork = 5f;

    [Header("References")]
    [SerializeField]
    private Image _imageEffect;
    [SerializeField]
    private AbilitiesUIManager _abilitiesUI;

    //Other global fields
    private float _currentWorkTime;
    private bool _abilityActivated = false;

    //Initialization Methods================================================================================================================================================

    private void Awake()
    {
        _imageEffect.enabled = false;
    }

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

        if (_abilityActivated)
        {
            _currentWorkTime -= Time.deltaTime;
            if (_currentWorkTime <= 0) DisableAbilityEffect();
        }
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
        Debug.Log("Shield Activated");
        MainReferenceHolder.Instance.GetPlayerHealthManager().ChangeDamageMultiplyer(_damageMultipyer);
        _currentWorkTime = _secondsToWork;
        _currentCooldown = _usageCooldown;
        _abilityActivated = true;

        _imageEffect.enabled = true;
    }
    private void DisableAbilityEffect()
    {
        MainReferenceHolder.Instance.GetPlayerHealthManager().ChangeDamageMultiplyer(1f);

        _abilityActivated = false;

        _imageEffect.enabled = false;
    }
}
