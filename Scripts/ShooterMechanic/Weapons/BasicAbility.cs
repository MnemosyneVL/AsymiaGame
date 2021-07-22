using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAbility : MonoBehaviour
{

    [Header("BaseClassSettigns")]
    [SerializeField]
    protected float _usageCooldown;
    //other references
    protected Collider _ownerCollider;
    protected EntityTeamManager _ownerTeamManager;
    protected float _currentCooldown;

    //Public Interface Methods==============================================================================================================================================

    //Initialization----------------------------------------------------------------------------------------------------------------------------------------------------

    //Initialize ability
    public void InitilizeAbility(Collider ownerCollider, EntityTeamManager ownerTeam)
    {
        _ownerCollider = ownerCollider;
        _ownerTeamManager = ownerTeam;
    }

    //Public Control Methods--------------------------------------------------------------------------------------------------------------------------------------------

    //Checks if ability can be used
    public virtual bool CanUse() => true;

    //Action on button down 
    public virtual void OnActivate() { }

    //Action on button up
    public virtual void OnDeactivate() { }

    //Returns current cooldown in seconds
    public virtual float RequestCooldown() => _currentCooldown;

    public virtual float RequestCooldownPercentage() => _currentCooldown / _usageCooldown;

    public virtual void ResetCooldown()
    {
        _currentCooldown = 0f;
    }
}
