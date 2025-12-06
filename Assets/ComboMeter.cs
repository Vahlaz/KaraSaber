using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class ComboMeter : MonoBehaviour
{

    public TextMeshProUGUI text;
    private int points = 0;
    private int combo = 0;

    public void addCombo()
    {
        combo += 1;
        points += Mathf.RoundToInt(10 * (0.5f * combo));
        text.text = "combo: " + combo + "\npoints: " + points;
    }

    public void breakCombo()
    {
        combo = 0;
        text.text = "combo: " + combo + "\npoints: " + points;

    }

    public void zeroPoints()
    {
        combo = 0;
        points = 0;
        text.text = "combo: " + combo + "\npoints: " + points;

    }

}
