using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Slider _healthSlider;

    //other fields
    private LivingEntityStatusManager _classReference;

    //Initialization Methods================================================================================================================================================

    //Initializes UI manager by passing the reference to target script
    public void InitializeUIManager(LivingEntityStatusManager referenceClass)
    {
        _classReference = referenceClass;
    }

    //Refreshes UI elements
    public void RefreshUI()
    {
        _healthSlider.value = _classReference.GetHealth() / _classReference.GetMaxHealth();
    }

}
