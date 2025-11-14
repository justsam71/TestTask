using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private Player player;
    [SerializeField] private float timeBeforeEnd = 4;


    private void Start()
    {
        StartRun();
    }

    private void OnEnable()
    {
        EventBus.Subscribe<AllEnemiesDeadEvent>(OnAllEnemiesDead);
        EventBus.Subscribe<DiedEvent>(OnPlayerDead);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<AllEnemiesDeadEvent>(OnAllEnemiesDead);
        EventBus.Unsubscribe<DiedEvent>(OnPlayerDead);
    }

    public void StartRun()
    {
        enemyManager.SpawnEnemies(3);
        player.SetNewPlayerState(PlayerState.Alive);
    }


    void OnAllEnemiesDead(AllEnemiesDeadEvent e)
    {
        Invoke("EndRun", timeBeforeEnd);
    }

    void OnPlayerDead(DiedEvent e)
    {
        if (e.owner.tag == "Player")
        {
            player.SetNewPlayerState(PlayerState.Dead);
            Invoke("EndRun", timeBeforeEnd);
        }
    }

    public void EndRun()
    {
        SaveSystem.Instance.SaveInventory();
        EventBus.Clear();
        SaveSystem.Instance.SubscribeToEvents();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}