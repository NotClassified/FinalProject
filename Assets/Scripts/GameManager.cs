using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    GameObject player;

    bool timerOn = false;
    float timer = 0;
    public event System.Action<float> Timelapse;
    int targetCheckpoint = 0;
    public static event System.Action GameOver;

    LevelManager levelManager;

    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
        Checkpoint.checkpointHit += NextCheckpoint;
        GameOver += delegate { timerOn = false; };
    }

    private void OnDestroy()
    {
        Checkpoint.checkpointHit -= NextCheckpoint;
        GameOver -= delegate { timerOn = false; };
    }

    private void Start()
    {
        player = Instantiate(playerPrefab, levelManager.playerSpawn, Quaternion.identity);
        timerOn = true;
    }

    private void Update()
    {
        if (timerOn)
        {
            timer += Time.deltaTime;
            if (Timelapse != null)
            {
                Timelapse(timer);
            }
        }
    }

    void NextCheckpoint(int checkpointIndex)
    {
        if (targetCheckpoint == checkpointIndex)
        {
            targetCheckpoint++;
            print("checkpoint reached: " + targetCheckpoint);

            if (targetCheckpoint >= levelManager.checkPointAmount) //last checkpoint
            {
                print("finished");
                if (GameOver != null)
                    GameOver();
            }
        }
    }
}
