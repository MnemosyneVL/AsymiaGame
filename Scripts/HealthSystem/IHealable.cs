using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealable 
{
    //Incapsulated fields requests
    float GetHealth();
    float GetMaxHealth();

    //Manipulations with health
    void Heal(float amount);

    //Buffs
    void ChangeHealingMultiplyer(float newValue);
}
