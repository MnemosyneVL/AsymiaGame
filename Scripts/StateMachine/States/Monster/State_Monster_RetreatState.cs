using StateMachineSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State_Monster_RetreatState : State<MonsterAIController>
{

    NavMeshPath _path = new NavMeshPath();
    MonsterAIController _monsterAI;
    FirstPersonController3D _movementManager;
    MonsterAgent _monsterAgent;

    bool _reachedGoal = false;
    int _currentCheckpoint = 0;
    bool _positionReached = false;
    bool _blockState = false;
    float _recalculationDelay = 1f;
    float _recalculationVar = 0f;

    public override void EnterState(MonsterAIController _owner)
    {
        Debug.Log("Entering Monster Retreat State");
        _monsterAI = _owner;
        _movementManager = _monsterAI.GetMovementManager();
        _reachedGoal = false;
        CalculateNewPath();
        _blockState = false;
        _recalculationVar = _recalculationDelay;
        _monsterAgent = _monsterAI.GetMonsterAgent();
    }

    public override void ExitState(MonsterAIController _owner)
    {

    }

    public override void UpdateState(MonsterAIController _owner)
    {

        //check transitions
        CheckTransitions();
        if (_blockState) return;
        //perform state tick update
        if (!_reachedGoal)
        {
            if (_recalculationVar > 0f)
                _recalculationVar -= Time.deltaTime;
            else
            {
                CalculateNewPath();
                _recalculationVar = _recalculationDelay;
            }
            ProceedToGoal();

        }
    }

    private void CalculateNewPath()
    {
        Debug.Log("calculating new path");
        Vector3 retreatPosition = new Vector3(50f, 0f, 50f);
        Debug.DrawLine(retreatPosition, _monsterAI.GetGeneratorPos(), Color.yellow, 10f);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(retreatPosition, out hit, 10f, NavMesh.AllAreas))
        {
            NavMesh.CalculatePath(_monsterAI.GetMonsterPos(), hit.position, NavMesh.AllAreas, _path);
            for (int i = 0; i < _path.corners.Length - 1; i++)
                Debug.DrawLine(_path.corners[i], _path.corners[i + 1], Color.red, 100f);
            Debug.LogWarning("Line drawn");
            _reachedGoal = false;
        }
        else
        {
            return;
        }

        _currentCheckpoint = 0;
    }



    private void ProceedToGoal()
    {
        if (_path.corners.Length <= 0)
        {
            CalculateNewPath();
            return;
        }

        Vector3 movementDirection = _path.corners[_currentCheckpoint] - _monsterAI.GetMonsterPos();
        movementDirection.Normalize();
        //movementDirection = Quaternion.AngleAxis(_humanAI.GetPlayerTransform().eulerAngles.y, Vector3.up) * movementDirection;
        _monsterAgent.MoveMonster(new Vector2(movementDirection.x, movementDirection.z));

        //_movementManager.MovePlayerAbsoluteOnInput(new Vector2(movementDirection.x, movementDirection.z));

        if (Vector3.Distance(_path.corners[_currentCheckpoint], _monsterAI.GetMonsterPos()) < _monsterAI.GetCheckPointReachDistance())
        {
            SetNewCheckpoint();
        }

    }

    private void SetNewCheckpoint()
    {
        _currentCheckpoint += 1;
        if (_currentCheckpoint > _path.corners.Length - 1)
        {
            _reachedGoal = true;
        }
    }

    private void CheckTransitions()
    {
        if (_reachedGoal)
        {
            _monsterAI.RequestStateTransition(State_Monster_RotateAroundGenerator.Instance);
            _blockState = true;
        }
    }



    private static State_Monster_RetreatState _instance;

    private State_Monster_RetreatState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static State_Monster_RetreatState Instance
    {
        get
        {
            if (_instance == null)
            {
                new State_Monster_RetreatState();
            }

            return _instance;
        }
    }
}
