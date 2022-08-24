using System;
using UnityEngine;
using UnityEngine.UI;


public class FuelBar : MonoBehaviour{
    public static FuelBar instance;
    [SerializeField] private Slider slider;

    private void Awake() => instance = this;

    /// <summary>
    ///     Setup the fuel bar.
    /// </summary>
    /// <param name="MaxFuel">The maximum fuel.</param>
    public void Setup(float MaxFuel){
        slider.maxValue = MaxFuel;
        slider.value = MaxFuel;
    }
    
    /// <summary>
    ///     Changes the fuel to "fuel".
    /// </summary>
    /// <param name="fuel">The fuel to change to.</param>
    public void SetFuel(float fuel){
        slider.value = fuel;
    }
    
    /// <summary>
    ///     Converts the fuel bar to health bar
    /// </summary>
    /// <param name="health">The health to convert to.</param>
    public void ConvertToHealth(int health){
        slider.maxValue = health;
        slider.value = health;
    }
}