using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_FireBreath : BasicAbility
{
    [Header("Settings")]
    [SerializeField]
    private float _damagePerTick = 2f;
    [SerializeField]
    private float _timeToExist = 3f;
    [SerializeField]
    private int _fireBalls = 20;
    [SerializeField]
    private float _fireBallsFrequency = 2f;

    [Header("References")]
    [SerializeField]
    private GameObject _fireBallPrefab;
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

        if(_abilityActivated)
        {
            if (_spawnedBalls >= _fireBalls)
            {
                _abilityActivated = false;
                return;
            }
            if(_spawnBallCooldown > 0)
            {
                _spawnBallCooldown -= Time.deltaTime;
            }
            else
            {
                Transform topmostPartent = _ownerCollider.transform.root;
                FirstPersonController3D monsterMovementController = topmostPartent.gameObject.GetComponentInChildren<FirstPersonController3D>();
                GameObject fireball = Instantiate(_fireBallPrefab, monsterMovementController.GetCameraHolder().position - Vector3.up*0.25f, Quaternion.identity);
                fireball.GetComponent<FireBallManager>().ActivateFireBall(_ownerCollider, monsterMovementController.GetCameraHolder().forward ,_damagePerTick, _timeToExist);
                _spawnedBalls++;
                _spawnBallCooldown = 1f / _fireBallsFrequency;
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
