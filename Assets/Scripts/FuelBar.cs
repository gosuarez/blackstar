using UnityEngine;
using UnityEngine.UI;

public class FuelBar : MonoBehaviour
{
    private Slider _fuelBar;
    public Gradient fuelGradient;
    public Image fill;

    private void Awake()
    {
        _fuelBar = GetComponent<Slider>();
    }

    public void SetMaxFuel(int maxFuel)
    {
        _fuelBar.maxValue = maxFuel;
        _fuelBar.value = maxFuel;
        //Sets the color of the fuel bar to the value assign to the right (green). The opposite will be red (0f).
        fill.color = fuelGradient.Evaluate(1f);
    }

    public void SetFuelValue(int fuelValue)
    {
        _fuelBar.value = fuelValue;
        //Identifies the specific value assign to the fuel bar
        fill.color = fuelGradient.Evaluate(_fuelBar.normalizedValue);
    }

}
