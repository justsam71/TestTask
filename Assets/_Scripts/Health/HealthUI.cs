using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text hpTXT;
    [SerializeField] private MonoBehaviour target; 

    private void OnEnable()
    {
        EventBus.Subscribe<HealthChangedEvent>(OnHealthChanged);
        EventBus.Subscribe<DiedEvent>(OnDied);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<HealthChangedEvent>(OnHealthChanged);
        EventBus.Unsubscribe<DiedEvent>(OnDied);
    }

    private void OnHealthChanged(HealthChangedEvent e)
    {
        if (e.health != target.GetComponent<Health>()) return;

        slider.maxValue = e.max;
        slider.value = e.current;

        hpTXT.text = $"{e.health.currentHealth} HP";
    }

    private void OnDied(DiedEvent e)
    {
        if (e.health != target.GetComponent<Health>()) return;

        Debug.Log($"{target.name} died!");
    }
}