using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveable 
{
    void Immobilize();
    void Push(Vector3 direction, float force);
    //Buffs
    void ChangeMovementSpeedMultiplyer(float newValue);

}
