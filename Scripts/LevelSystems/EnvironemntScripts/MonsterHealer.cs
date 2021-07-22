using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHealer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float _minHealDistance;
    [SerializeField]
    private int _ticksPerSecond = 2;
    [SerializeField]
    private float _healthPerTick;

    [Header("References")]
    [SerializeField]
    private Transform _monsterTransform;

    //other fields
    private bool _isHealing = false;


    private float _healCoolDown;
    private IHealable _monsterHealableScript;
    Transform _topmostPartent;

    //Debug Methods=========================================================================================================================================================
    private void OnDrawGizmos()
    {


        if (_isHealing) Gizmos.color = Color.green;
        else Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(this.transform.position, _minHealDistance);
    }
    //Initialization=====================
    private void Awake()
    {
        _topmostPartent = _monsterTransform.transform.root;
        _monsterHealableScript = _topmostPartent.gameObject.GetComponentInChildren<IHealable>();

    }

    private void Update()
    {
        _isHealing = false;
        if (_healCoolDown >= 0) _healCoolDown -= Time.deltaTime;

        float distanceToMonster = Vector3.Distance(this.transform.position, _monsterTransform.position);
        if (distanceToMonster <= _minHealDistance) return;

            _isHealing = true;
        if (_healCoolDown <= 0)
        {

            _monsterHealableScript.Heal(_healthPerTick);
            _healCoolDown = 1f / _ticksPerSecond;
        }
    }
}
