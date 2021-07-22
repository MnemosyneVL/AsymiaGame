using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class WeaponHolder : MonoBehaviour, IAttacker
{
    [Header("Weapon References")]
    [SerializeField]
    private BasicAbility _equippedWeapon;
    [SerializeField,ReorderableList]
    private List<BasicAbility> _equippedAbilities;

    [Header("References")]
    [SerializeField]
    private Collider _ownerCollider;
    [SerializeField]
    private EntityTeamManager _teamManager;

    //other fields

    private bool _abilitiesLocked = false;
    private bool _weaponLocked = false;



    //Initialization Methods================================================================================================================================================
    //Unity Start
    private void Start()
    {
        if(_equippedWeapon!= null)
        {
            _equippedWeapon.InitilizeAbility(_ownerCollider,_teamManager);
        }

        foreach(BasicAbility ability in _equippedAbilities)
        {
            ability.InitilizeAbility(_ownerCollider,_teamManager);
        }
    }
    //Update Methods========================================================================================================================================================

    //Unity update
    private void Update()
    {

    }

    //Public Class Methods==================================================================================================================================================

    //Public Getters--------------------------------------------------------------------------------------------------------------------------------------------------------

    public BasicAbility GetEquippedWeapon()
    {
        return _equippedWeapon;
    }
    public BasicAbility GetEquippedAbilitiy(int abilityNr)
    {
        if (abilityNr > _equippedAbilities.Count - 1) return null;
        return _equippedAbilities[abilityNr];
    }
    public Collider GetOwnerCollider() => _ownerCollider;

    //Weapon slot logic-----------------------------------------------------------------------------------------------------------------------------------------------------

    //Equip new weapon
    public void EquipNewWeapon(BasicAbility newWeapon)
    {
        
    }

    //Use currently equiped weapon
    public void ActivateCurrentWeapon()
    {
        if (_weaponLocked) return;
        if (_equippedWeapon.CanUse())
        {
            _equippedWeapon.OnActivate();
            //LockAbilities();
        }
    }

    public void DeactivateCurrentWeapon()
    {
        _equippedWeapon.OnDeactivate();
        //UnlockAllAbilities();
    }

    //Abilities logic-------------------------------------------------------------------------------------------------------------------------------------------------------
    
    //Use ability
    public void ActivateAbility(int abilityNr)
    {
        Debug.Log($"Ability Nr:{abilityNr} activated");
        if(abilityNr>_equippedAbilities.Count-1) return;
        if (_abilitiesLocked) return;
        if (_equippedAbilities[abilityNr].CanUse())
        {
            _equippedAbilities[abilityNr].OnActivate();
            //LockAbilities();
        }

    }

    public void DeactivateAbility(int abilityNr)
    {
        _equippedAbilities[abilityNr].OnDeactivate();
        //UnlockAllAbilities();
    }

    public void ResetAllAbilities()
    {
        foreach (BasicAbility ability in _equippedAbilities)
        {
            ability.RequestCooldown();
        }
    }

    //IAttacker Interface Implementation====================================================================================================================================

    //Locks abilties for set amount of seconds. if seconds is set to 0 then locks abilities forever.
    public void LockAbilities()
    {
        _abilitiesLocked = true;
        _weaponLocked = true;
    }

    public void UnlockAllAbilities()
    {
        _abilitiesLocked = false;
        _weaponLocked = false;
    }


}
