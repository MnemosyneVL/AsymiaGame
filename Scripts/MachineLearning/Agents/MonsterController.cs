using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{


    [Header("References")]
    [SerializeField]
    private Transform _monsterTransform;
    [SerializeField]
    private Transform _headTransform;
    [SerializeField]
    private WeaponHolder _monsterWeponHolder;
    [SerializeField]
    private WeaponHolder _playerWeaponHolder;
    //[SerializeField]
    //private HumanAIController _playerAIController;
    //[SerializeField]
    //private MonsterAIController _monsterAIController;
    [SerializeField]
    private FirstPersonController3D _monsterMovementController;
    [SerializeField]
    public LivingEntityStatusManager _monsterHealthManager;//COMMENT TODO MAKE it private and beautiful later
    [SerializeField]
    public LivingEntityStatusManager _playerHealthManager;//COMMENT TODO MAKE it private and beautiful later
    [SerializeField]
    public LivingEntityStatusManager _generatorHealthManager;//COMMENT TODO MAKE it private and beautiful later
    [SerializeField]
    private RewardManager_DistanceToGenerator _distanceToGenerator;
    [SerializeField]
    private RewardManager_LookingAtTarget _lookingAtTarget;



    //Public Methods========================================================================================================================================================
    //Other Observations

    public bool MonsterHasFallen()
    {
        return _monsterMovementController.HasFallenOffPlatform();
    }

    public float RequestOtherRewards()
    {
        float finalValue = 0f;
        finalValue += _distanceToGenerator.RequestRewardValue();
        finalValue += _lookingAtTarget.RequestReward();
        return finalValue;
    }
    //ObservationMethods for ML---------------------------------------------------------------------------------------------------------------------------------------------

    //Weapons observations
    public float GetCooldownAbility1() => _monsterWeponHolder.GetEquippedAbilitiy(0).RequestCooldownPercentage();
    public float GetCooldownAbility2() => _monsterWeponHolder.GetEquippedAbilitiy(1).RequestCooldownPercentage();
    public float GetCooldownAbility3() => _monsterWeponHolder.GetEquippedAbilitiy(2).RequestCooldownPercentage();
    public float GetCooldownAbility4() => _monsterWeponHolder.GetEquippedAbilitiy(3).RequestCooldownPercentage();

    //Human Weapon observations
    public float GetCooldownPlayerAbility1() => _playerWeaponHolder.GetEquippedAbilitiy(0).RequestCooldownPercentage();
    public float GetCooldownPlayerAbility2() => _playerWeaponHolder.GetEquippedAbilitiy(1).RequestCooldownPercentage();
    public float GetCooldownPlayerAbility3() => _playerWeaponHolder.GetEquippedAbilitiy(2).RequestCooldownPercentage();
    //Status observations
    public float GetPlayerHealth() => _playerHealthManager.GetHealthPercentage();
    public float GetMonsterHealth() => _monsterHealthManager.GetHealthPercentage();
    public float GetGeneratorHealth() => _generatorHealthManager.GetHealthPercentage();

    //Transform observations
    public Vector2 GetMonsterPosition() => new Vector2(_monsterTransform.position.x, _monsterTransform.position.z);
    public float GetBodyHorizontalRotation() => _monsterTransform.rotation.y;
    public float GetHeadVerticalRotation() => _headTransform.rotation.x;

    //public void ResetPlayer() => _playerAIController.ResetPlayer();

    //Action Methods for ML-------------------------------------------------------------------------------------------------------------------------------------------------
    
    //Weapon Actions
    public void ActivateAbility1() { _monsterWeponHolder.ActivateAbility(0); }
    public void ActivateAbility2() { _monsterWeponHolder.ActivateAbility(1); }
    public void ActivateAbility3() { _monsterWeponHolder.ActivateAbility(2); }
    public void ActivateAbility4() { _monsterWeponHolder.ActivateAbility(3); }
    
    //Movement Actions
    public void MoveMonster(Vector2 direction) { _monsterMovementController.MovePlayerAbsoluteOnInput(direction); }
    public void RotateMonster(Vector2 direction) { _monsterMovementController.ChangeLookOnInput(direction); }
}
