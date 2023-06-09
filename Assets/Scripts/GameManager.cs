using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] int frameRate = -1;

    [SerializeField] GameObject playerPrefab;
    GameObject player;

    bool timerOn = false;
    float timer = 0;
    public static event System.Action<float> Timelapse;

    int targetCheckpoint = 0;
    public static event System.Action GameOver;

    LevelManager levelManager;

    private void Awake()
    {
        if (frameRate != -1)
        {
            Debug.LogWarning("frameRate changed");
        }
        Application.targetFrameRate = frameRate;

        levelManager = FindObjectOfType<LevelManager>();
        Checkpoint.checkpointHit += NextCheckpoint;
        GameOver += delegate 
        { 
            timerOn = false;
            TogglePlayerInput(false); 
        };
    }

    private void OnDestroy()
    {
        Checkpoint.checkpointHit -= NextCheckpoint;
        GameOver -= delegate 
        { 
            timerOn = false;
            TogglePlayerInput(false);
        };
    }

    private void Start()
    {
        player = Instantiate(playerPrefab, levelManager.playerSpawn, Quaternion.Euler(levelManager.spawnDirection));

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

            if (targetCheckpoint >= levelManager.checkPointAmount) //last checkpoint
            {
                if (GameOver != null)
                    GameOver();
            }
        }
    }

    void TogglePlayerInput(bool enablePlayerInput)
    {
        if (player != null)
        {
            foreach (MonoBehaviour component in player.GetComponents<MonoBehaviour>())
            {
                if (component is IContainsInput)
                {
                    component.enabled = enablePlayerInput;
                }
            }
        }
    }
}
