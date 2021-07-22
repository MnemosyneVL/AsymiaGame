using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LivingEntityStatusManager : MonoBehaviour, IDamageable, IHealable
{
    [Header("Settings")]
    [SerializeField]
    private float _maxHealth = 100f;
    [SerializeField]
    private float _currentHealth = 100f;

    [Header("References")]
    [SerializeField]
    private Transform _playerTransform;

    [Required("If no reference is given, system will search for UI manager under the same component")]
    [SerializeField]
    private HealthUIManager _managerUI;

    //Actions
    private Action _OnDeathAction;
    private Action<float> _OnRecieveDamageAction;

    //other Fields
    private Action _onHealthChange;

    //buff Fields
    private float _damageMultiplyer = 1f;
    private float _healingMultiplyer = 1f;

    //Initialization Methods================================================================================================================================================
    //Public Initialization Methods-----------------------------------------------------------------------------------------------------------------------------------------
    public void SetOnDeathAction(Action action)
    {
        _OnDeathAction = action;
    }
    public void SetOnRecieveDamageAction(Action<float> action)
    {
        _OnRecieveDamageAction = action;
    }

    //Unity Start Method
    private void Start()
    {
        if (_managerUI == null)
        {
            _managerUI = this.transform.GetComponent<HealthUIManager>();
        }

        if (_managerUI != null)
        {
            _managerUI.InitializeUIManager(this);
            _onHealthChange += _managerUI.RefreshUI;
        }
    }

    //Public Interface methods========================================================================================================================================================

    //Incapsulated Fields requesters----------------------------------------------------------------------------------------------------------------------------------------

    //Returns current health value
    public float GetHealth() => _currentHealth;

    public float GetHealthPercentage() => _currentHealth / _maxHealth;

    //Returns max health value
    public float GetMaxHealth() => _maxHealth;

    //Health Manupulation Methods-------------------------------------------------------------------------------------------------------------------------------------------

    //Deals certain amount of damage to player    
    public void DealDamage(float amount)
    {

        amount *= _damageMultiplyer;//Apply buff values

        _currentHealth -= amount;//Apply damage

        if (_currentHealth <= 0) DeathAction();//Check if health lower then 0 and starts death action

        _onHealthChange?.Invoke();//Invoke health change event
        _OnRecieveDamageAction?.Invoke(amount);
    }

    //Heals certain amount of health
    public void Heal(float amount)
    {
        amount *= _healingMultiplyer;//Apply buff values

        Debug.Log("Healing");
        _currentHealth += amount;//Apply healing

        if (_currentHealth > _maxHealth) _currentHealth = _maxHealth;//Check if health higher then max and fixes if needed

        _onHealthChange?.Invoke();//Invoke health change event
    }

    public void ResetMaxHealth()
    {
        _currentHealth = _maxHealth;
    }

    //Buff Methods----------------------------------------------------------------------------------------------------------------------------------------------------------

    //Changes value for damage multipyer buff
    public void ChangeDamageMultiplyer(float newValue)
    {
        _damageMultiplyer = newValue;
    }

    public void ChangeHealingMultiplyer(float newValue)
    {
        _healingMultiplyer = newValue;
    }

    //Internal Logic========================================================================================================================================================

    //On Death Events-------------------------------------------------------------------------------------------------------------------------------------------------------

    //Death Action when health reaches zero
    //!!!! DEBUG !!!!! Current logic must be changed later
    private void DeathAction()
    {
        _OnDeathAction?.Invoke();
    }


}
