using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineCam;
    private CinemachineBasicMultiChannelPerlin noise;
    [SerializeField] private float intensity = 2f;
    [SerializeField] private float shakeMaxTime = 0.5f;
    private float shakeCurrentTime = 0f;

    private void Awake()
    {
        cinemachineCam = GetComponent<CinemachineVirtualCamera>();
        noise = cinemachineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        EventBus.Subscribe<PlayerShootEvent>(ShakeOnShoot);
        EventBus.Subscribe<HealthChangedEvent>(ShakeOnTakeDamage);
        

        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;
    }

    public void StartShaking(float amplitude, float frequency)
    {
        noise.m_AmplitudeGain = amplitude;
        noise.m_FrequencyGain = frequency;
    }

    public void StopShaking()
    {
        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;
    }

    public void ShakeOnShoot(PlayerShootEvent e)
    {
        noise.m_AmplitudeGain = intensity;
        noise.m_FrequencyGain = intensity;
        shakeCurrentTime = shakeMaxTime;
    }

    public void ShakeOnTakeDamage(HealthChangedEvent e)
    {
        if (e.health.gameObject.CompareTag("Player"))
        {
            noise.m_AmplitudeGain = intensity;
            noise.m_FrequencyGain = intensity;
            shakeCurrentTime = shakeMaxTime;
        }
    }

    private void Update()
    {
        if (shakeCurrentTime > 0)
        {
            shakeCurrentTime -= Time.deltaTime;
            if (shakeCurrentTime <= 0f)
            {
                noise.m_AmplitudeGain = 0f;
                noise.m_FrequencyGain = 0f;
            }
        }
    }
}
