using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_AcidTrail : BasicAbility
{
    [Header("Settings")]
    [SerializeField]
    private float _damagePerTick = 2f;
    [SerializeField]
    private float _timeToExist = 3f;
    [SerializeField]
    private int _acidBalls = 20;
    [SerializeField]
    private float _acidBallsFrequency = 2f;

    [Header("References")]
    [SerializeField]
    private GameObject _acidBallPrefab;
    [SerializeField]
    private AbilitiesUIManager _abilitiesUI;

    //Other global fields
    private bool _abilityActivated = false;
    private int _spawnedBalls;
    private float _spawnBallCooldown;

    //Update Methods========================================================================================================================================================

    //Unity Update
    private void Update()
    {
        if (_currentCooldown >= 0)
        {
            //_abilitiesUI.DisableAbility();
            _currentCooldown -= Time.deltaTime;
            //_abilitiesUI.UpdateCooldownNr(_currentCooldown);
        }
        else
        {
            //_abilitiesUI.EnableAbility();
        }

        if (_abilityActivated)
        {
            if (_spawnedBalls >= _acidBalls)
            {
                _abilityActivated = false;
                return;
            }
            if (_spawnBallCooldown > 0)
            {
                _spawnBallCooldown -= Time.deltaTime;
            }
            else
            {
                Transform ownerTopmostPartent = _ownerCollider.transform.root;
                FirstPersonController3D monsterMovementController = ownerTopmostPartent.gameObject.GetComponentInChildren<FirstPersonController3D>();

                GameObject fireball = Instantiate(_acidBallPrefab, monsterMovementController.GetOriginPoint() , Quaternion.identity);
                fireball.GetComponent<AcidBallManager>().ActivateAcidBall(_ownerCollider, _damagePerTick, _timeToExist);
                _spawnedBalls++;
                _spawnBallCooldown = 1f / _acidBallsFrequency;
            }
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
        Debug.Log("FireBreath Activated");
        _abilityActivated = true;
        _spawnedBalls = 0;
        _spawnBallCooldown = 0f;
        //GameObject healZone = Instantiate(_fireBallPrefab, PlayerReferenceHolder.Instance.RequestFPSCOntroller().GetOriginPoint(), Quaternion.identity);
        //healZone.GetComponent<HealZoneManager>().ActivateHealZone(_healingPerTick, _secondsToExist);
        _currentCooldown = _usageCooldown;
    }

    //ML methods-----------------------------------------------------------------------------------------------------------------------------------------------------------

    public override float RequestCooldown()
    {
        return _currentCooldown;
    }
}
