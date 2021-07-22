using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable 
{
    //Incapsulated fields requests
    float GetHealth();
    float GetMaxHealth();

    //Manipulations with health
    void DealDamage(float amount);

    //Buffs
    void ChangeDamageMultiplyer(float newValue);

}
