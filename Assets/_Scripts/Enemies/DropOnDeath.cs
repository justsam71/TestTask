using UnityEngine;
using System.Collections.Generic;

public class DropOnDeath : MonoBehaviour
{
    [Header("Drop Settings")]
    //public int dropAmount = 1;
    public List<GameObject> pickupPrefabs = new();  

    private void OnEnable()
    {
        EventBus.Subscribe<DiedEvent>(Drop);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<DiedEvent>(Drop);
    }

    private void Drop(DiedEvent e)
    {
        if (gameObject != e.owner)
            return;

        if (pickupPrefabs == null || pickupPrefabs.Count == 0)
            return;

        GameObject prefab = pickupPrefabs[Random.Range(0, pickupPrefabs.Count)];

        GameObject pickUp = Instantiate(prefab, transform.position, Quaternion.identity);

        pickUp.transform.localScale = prefab.transform.localScale;
    }
}