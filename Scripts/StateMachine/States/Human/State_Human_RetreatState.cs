using StateMachineSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State_Human_RetreatState : State<HumanAIController>
{
    NavMeshPath _path = new NavMeshPath();
    HumanAIController _humanAI;
    FirstPersonController3D _movementManager;

    bool _reachedGoal = false;
    int _currentCheckpoint = 0;
    bool _positionReached = false;
    bool _blockState = false;

    private static State_Human_RetreatState _instance;

    private State_Human_RetreatState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static State_Human_RetreatState Instance
    {
        get
        {
            if (_instance == null)
            {
                new State_Human_RetreatState();
            }

            return _instance;
        }
    }

    public override void EnterState(HumanAIController _owner)
    {
        Debug.Log("Entering Monster Chase State");
        _humanAI = _owner;
        _movementManager = _humanAI.GetMovementManager();
        _reachedGoal = false;
        CalculateNewPath();
        _blockState = false;
    }

    public override void ExitState(HumanAIController _owner)
    {

    }

    public override void UpdateState(HumanAIController _owner)
    {
        //check transitions
        CheckTransitions();
        if (_blockState) return;
        //perform state tick update
        if (!_reachedGoal)
        {
            ProceedToGoal();
        }
    }

    private void CalculateNewPath()
    {
        Vector3 positionFarFromMonster = (_humanAI.GetMonsterPos() * -2f);
        Debug.DrawLine(positionFarFromMonster, _humanAI.GetMonsterPos(), Color.yellow, 10f);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(positionFarFromMonster, out hit, 50f, NavMesh.AllAreas))
        {
            NavMesh.CalculatePath(_humanAI.GetPlayerPos(), hit.position, NavMesh.AllAreas, _path);
            for (int i = 0; i < _path.corners.Length - 1; i++)
                Debug.DrawLine(_path.corners[i], _path.corners[i + 1], Color.red, 100f);
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

        Vector3 movementDirection = _path.corners[_currentCheckpoint] - _humanAI.GetPlayerPos();
        movementDirection.Normalize();
        //movementDirection = Quaternion.AngleAxis(_humanAI.GetPlayerTransform().eulerAngles.y, Vector3.up) * movementDirection;

        _movementManager.MovePlayerAbsoluteOnInput(new Vector2(movementDirection.x, movementDirection.z));

        if (Vector3.Distance(_path.corners[_currentCheckpoint], _humanAI.GetPlayerPos()) < _humanAI.GetCheckPointReachDistance())
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

    //State transitions ================================================
    private void CheckTransitions()
    {
        if(_reachedGoal)
        {
            _humanAI.RequestStateTransition(State_Human_PatrolGenerator.Instance);
            _blockState = true;
        }
    }

}
