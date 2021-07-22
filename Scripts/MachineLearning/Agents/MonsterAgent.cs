using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class MonsterAgent : Agent
{
    [Header("References")]
    [SerializeField]
    private MonsterController _monsterController;
    [SerializeField]
    private SceneController _sceneController;

    //other fields
    Vector2 _movementDirection = Vector2.zero;
    float _mouseRotation =0f;
    bool _useAbility1 = false;
    bool _useAbility2 = false;
    bool _useAbility3 = false;
    bool _useAbility4 = false;

    //ML Override methods===================================================================================================================================================

    public override void Initialize()
    {
        InitializeRewards();
        _sceneController.ResetScene();
    }

    public override void OnEpisodeBegin()
    {
        _sceneController.ResetScene();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //position
        Vector2 monsterPos = _monsterController.GetMonsterPosition();
        sensor.AddObservation(monsterPos.x);
        sensor.AddObservation(monsterPos.y);
        //rotation
        sensor.AddObservation(_monsterController.GetHeadVerticalRotation());
        sensor.AddObservation(_monsterController.GetBodyHorizontalRotation());
        //cooldowns
        sensor.AddObservation(_monsterController.GetCooldownAbility1());
        sensor.AddObservation(_monsterController.GetCooldownAbility2());
        sensor.AddObservation(_monsterController.GetCooldownAbility3());
        sensor.AddObservation(_monsterController.GetCooldownAbility4());
        //playercooldowns
        sensor.AddObservation(_monsterController.GetCooldownPlayerAbility1());
        sensor.AddObservation(_monsterController.GetCooldownPlayerAbility2());
        sensor.AddObservation(_monsterController.GetCooldownPlayerAbility3());
        //status
        sensor.AddObservation(_monsterController.GetMonsterHealth());
        sensor.AddObservation(_monsterController.GetPlayerHealth());
        sensor.AddObservation(_monsterController.GetGeneratorHealth());
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        //movement
        Vector2 movementDir = new Vector2(actionBuffers.ContinuousActions[0], actionBuffers.ContinuousActions[1]);
        _monsterController.MoveMonster(movementDir);
        //rotation
        Vector2 rotationDir = new Vector2(actionBuffers.ContinuousActions[2], 0f);
        _monsterController.RotateMonster(rotationDir);
        //abilities
        if (actionBuffers.DiscreteActions[0] == 1) _monsterController.ActivateAbility1();
        if (actionBuffers.DiscreteActions[1] == 1) _monsterController.ActivateAbility2();
        if (actionBuffers.DiscreteActions[2] == 1) _monsterController.ActivateAbility3();
        if (actionBuffers.DiscreteActions[3] == 1) _monsterController.ActivateAbility4();

        if(_monsterController.MonsterHasFallen())
        {
            OnMonsterKilled();
        }

        AddReward(_monsterController.RequestOtherRewards());

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // Read input values and round them. GetAxisRaw works better in this case
        // because of the DecisionRequester, which only gets new decisions periodically.
        //int vertical = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));
        //int horizontal = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));

        var actions = actionsOut.ContinuousActions;
        actions[0] = _movementDirection.x;
        actions[1] = _movementDirection.y;

        //Vector2 mouseDeltaInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        actions[2] = _mouseRotation;

        var descrete = actionsOut.DiscreteActions;
        if (_useAbility1) descrete[0] = 1; else descrete[0] = 0;
        if (_useAbility2) descrete[1] = 1; else descrete[1] = 0;
        if (_useAbility3) descrete[2] = 1; else descrete[2] = 0;
        if (_useAbility4) descrete[3] = 1; else descrete[3] = 0;

        _movementDirection = Vector2.zero;
        _mouseRotation = 0f;
        _useAbility1 = false;
        _useAbility2 = false;
        _useAbility3 = false;
        _useAbility4 = false;

    }

    //Public Reward Methods=================================================================================================================================================

    float _maxMonsterHealth;
    float _maxPlayerHealth;
    float _maxGeneratorHealth;

    private void InitializeRewards()
    {
        _monsterController._generatorHealthManager.SetOnDeathAction(OnGeneratorKillled);
        _monsterController._generatorHealthManager.SetOnRecieveDamageAction(OnGeneratorRecieveDamage);
        _maxGeneratorHealth = _monsterController._generatorHealthManager.GetMaxHealth();

        _monsterController._playerHealthManager.SetOnDeathAction(OnPlayerKilled);
        _monsterController._playerHealthManager.SetOnRecieveDamageAction(OnPlayerRecieveDamage);
        _maxPlayerHealth = _monsterController._playerHealthManager.GetMaxHealth();

        _monsterController._monsterHealthManager.SetOnDeathAction(OnMonsterKilled);
        _monsterController._monsterHealthManager.SetOnRecieveDamageAction(OnMonsterRecieveDamage);
        _maxMonsterHealth = _monsterController._monsterHealthManager.GetMaxHealth();

    }

    public void OnMonsterRecieveDamage(float damageValue)
    {
        AddReward(-damageValue/_maxMonsterHealth);
    }
    public void OnPlayerRecieveDamage(float damageValue)
    {
        AddReward(damageValue / _maxPlayerHealth);
    }
    public void OnGeneratorRecieveDamage(float damageValue)
    {
        AddReward(damageValue / _maxGeneratorHealth);
    }
    public void OnGeneratorKillled()
    {
        Debug.Log("Generator Dead");
        AddReward(10f);
        EndEpisode();
    }
    public void OnPlayerKilled()
    {
        AddReward(3f);
        //_monsterController.ResetPlayer();
    }
    public void OnMonsterKilled()
    {
        AddReward(-10f);
        EndEpisode();
    }
    //Public Controls methods
    public void MoveMonster(Vector2 direction)
    {
        _movementDirection = direction;
    }
    public void RotateMonster(float mouseRotation)
    {
        _mouseRotation = mouseRotation;
    }
    public void UseAbility1()
    {
        _useAbility1 = true;
    }
    public void UseAbility2()
    {
        _useAbility2 = true;
    }
    public void UseAbility3()
    {
        _useAbility3 = true;
    }
    public void UseAbility4()
    {
        _useAbility4 = true;
    }

}
