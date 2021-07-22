using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealZoneManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private int _ticksPerSecond = 2;


    [Header("References")]
    [SerializeField]
    private SphereCollider _sphereCollider;

    //Other fields
    private float _healthPerTick;

    private float _healCoolDown;

    private string _useOnTag;

    private Dictionary<Collider, IHealable> _healables = new Dictionary<Collider, IHealable>();
    //Public Initialization Methods=========================================================================================================================================
    public void ActivateHealZone(float healthPerTick, float secondsToExist, string useOnTag)
    {
        Debug.Log("HealZone Created");
        _healthPerTick = healthPerTick;
        StartCoroutine(DestroyIn(secondsToExist));
        _useOnTag = useOnTag;
    }

    //Update Methods========================================================================================================================================================
    private void Update()
    {
        if (_healCoolDown >= 0) _healCoolDown -= Time.deltaTime;

        if (_healCoolDown <= 0)
        {

            foreach (KeyValuePair<Collider, IHealable> entry in _healables)
            {
                entry.Value.Heal(_healthPerTick);
            }
            _healCoolDown = 1f / _ticksPerSecond;
        }
    }


    //Internal Logic=========================================================================================================================================================

    private void OnTriggerEnter(Collider other)
    {
        Transform topmostPartent = other.transform.root;
        if (topmostPartent.tag != _useOnTag) return;
        IHealable referenceHolder = topmostPartent.gameObject.GetComponentInChildren<IHealable>();
        if (referenceHolder != null)
        {
            _healables.Add(other, referenceHolder);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        _healables.Remove(other);

    }
    /*
    //ColliderUpdate
    private void OnTriggerStay(Collider other)
    {
        Transform topmostPartent = other.transform.root;
        IHealable referenceHolder = topmostPartent.gameObject.GetComponentInChildren<IHealable>();
        if(referenceHolder != null && _healCoolDown <= 0)
        {
            referenceHolder.Heal(_healthPerTick);
            _healCoolDown = 1f / _ticksPerSecond;
        }
    }
    */

    //Destroys game object in set amount of seconds
    IEnumerator DestroyIn(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
