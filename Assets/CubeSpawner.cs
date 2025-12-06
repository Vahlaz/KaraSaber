using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Unity.Mathematics;

public class CubeSpawner : MonoBehaviour
{

    [Header("Menu")]
    public Canvas menu;

    [Header("Music")]
    public AudioSource audioplayer;


    [Header("prefabs")]
    public GameObject[] cubePrefabs;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    public ComboMeter comboMeter;

    List<(float time, int note)> spawnTimes = new List<(float, int)>();
    private float gap = 0f;
    private float bpm = 0f;

    private float offset = -5.6f;

    private float timer = 0f;
    private bool leftRigth = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Play(string name)
    {
        AudioClip audioClip = Resources.Load<AudioClip>("Music/" + name);
        TextAsset data = Resources.Load<TextAsset>("Music/" + name);
        string[] lines = data.text.Split("\n").ToArray();
        lines = lines.Take(lines.Length - 1).ToArray();
        foreach (string line in lines)
        {

            if (line[0] == ':')
            {
                string[] splitString = line.Split(' ');
                float beats = float.Parse(splitString[1].ToString());
                float tavut = splitString.Length - 4;
                int note = int.Parse(splitString[3].ToString());
                float timeSpacer = float.Parse(splitString[2].ToString()) / tavut;


                for (int i = 0; i < tavut; i++)
                {
                    float spawnTime = ((beats + timeSpacer * i) * bpm) + gap + offset;
                    spawnTimes.Add((spawnTime, note));
                }

            }
            else if (line[0] == '-')
            {
            }
            else if (line[..4] == "#BPM")
            {
                bpm = 60 / float.Parse(line[^4..]);
            }
            else if (line[..4] == "#GAP")
            {
                gap = float.Parse(line.Split(":")[1].ToString()) / 1000f;
            }

        }


        audioplayer.clip = audioClip;
        audioplayer.Play();
        comboMeter.zeroPoints();
        timer = 0;
        menu.gameObject.SetActive(false);
    }



    void Update()
    {

        timer += Time.deltaTime;
        if (spawnTimes.Count > 0 && timer >= spawnTimes[0].time)
        {

            Transform point = leftRigth ? spawnPoints[0] : spawnPoints[1];

            GameObject prefab = leftRigth ? cubePrefabs[UnityEngine.Random.Range(0, cubePrefabs.Length / 2)] : cubePrefabs[UnityEngine.Random.Range(cubePrefabs.Length / 2, cubePrefabs.Length)];

            leftRigth = !leftRigth;
            point.position = new Vector3(point.position.x, math.clamp(0.66f + (spawnTimes[0].note * 0.15f),0.3f,1f), point.position.z);

            Instantiate(prefab, point.position, point.rotation);
            spawnTimes.RemoveAt(0);
            if (spawnTimes.Count == 0 || !audioplayer.isPlaying)
            {
                stopGame();
            }
        }
    }


    public void stopGame()
    {
        audioplayer.Stop();
        spawnTimes = new List<(float time, int note)>();
        menu.gameObject.SetActive(true);
    }
}
