using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{

    public CubeSpawner cubeSpawner;
    public TextMeshProUGUI text;
    public string buttonName;

    public void giveName(string name)
    {
        buttonName = name;
        text.text = name;

    }

    public void play()
    {
        cubeSpawner.Play(buttonName);
    }
}
