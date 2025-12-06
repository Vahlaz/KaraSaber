using UnityEngine;
using System.IO;
using UnityEngine.UI;
public class Menuhandler : MonoBehaviour
{
    [Header("Menu")]
    public Canvas menu;

    [Header("button Prefab")]
    public Button buttonPrefab;

    [Header("startPoint")]
    public Transform startPoint;

    public CubeSpawner cubeSpawner;


    void Start()
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Music");

        int index = 0;
        foreach (AudioClip clip in clips)
        {
            Button newButton = Instantiate(buttonPrefab, startPoint);
            newButton.transform.position = new Vector3(
                startPoint.position.x,
                startPoint.position.y - 0.3f * index, 
                startPoint.position.z
            );
            newButton.GetComponent<ButtonScript>().giveName(clip.name);
            newButton.GetComponent<ButtonScript>().cubeSpawner = cubeSpawner;
            index += 1;
        }

    }

}
