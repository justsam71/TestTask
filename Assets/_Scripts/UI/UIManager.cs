using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject InventoryScreen;
    [SerializeField] private GameObject MainScreen;

    [SerializeField] private TMP_Text enemyCounter;
    [SerializeField] private TMP_Text allEnemiesDefeated;

    [SerializeField] private string ammoId = "0";
    [SerializeField] private TMP_Text bulletsCounter;

    private Coroutine finalCounter = null;

    private int enemiesRemaining = 3;

    private void OnEnable()
    {
        EventBus.Subscribe<DiedEvent>(OnEnemyDie);
        EventBus.Subscribe<DiedEvent>(OnPlayerDie);
        EventBus.Subscribe<AllEnemiesDeadEvent>(OnAllEnemiesDefeated);
        EventBus.Subscribe<PlayerShootEvent>(OnPlayerShoot);
        EventBus.Subscribe<InventoryUpdatedEvent>(OnInventoryUpdated);
    }

    private void OnEnemyDie(DiedEvent e)
    {
        if (e.owner.TryGetComponent<Enemy>(out _))
        {
            enemiesRemaining = Mathf.Max(0, enemiesRemaining - 1);
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (enemyCounter != null)
            enemyCounter.text = $"Enemies left: {enemiesRemaining}/3";
    }

    private void OnPlayerDie(DiedEvent e)
    {
        if (e.owner.TryGetComponent<Player>(out _))
        {
            if (finalCounter != null) return;

            finalCounter = StartCoroutine(ShowRestartCountdown("Player died!"));
        }
    }
    private void OnAllEnemiesDefeated(AllEnemiesDeadEvent e)
    {
        if (finalCounter != null) return;

        finalCounter = StartCoroutine(ShowRestartCountdown("All enemies defeated!"));
    }

    private IEnumerator ShowRestartCountdown(string finalText)
    {
        int time = 3;

        while (time > 0)
        {
            allEnemiesDefeated.text = $"{finalText} Restart in {time} seconds.";
            yield return new WaitForSeconds(1f);
            time--;
        }

        allEnemiesDefeated.text = "Restarting...";
    }

    void OnInventoryUpdated(InventoryUpdatedEvent e) { UpdateBulletCount(); }
    void OnPlayerShoot(PlayerShootEvent e) { UpdateBulletCount(); }
    private void UpdateBulletCount()
    {
        bulletsCounter.text = $"{InventoryManager.Instance.Inventory.GetTotalAmount("0")}";
    }
}