using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEmissionController : MonoBehaviour
{
    [SerializeField] ParticleSystem[] particles;
    [SerializeField] float[] defaultEmissionRates;

    private void Start()
    {
       // GameplayEvents.Instance.OnParticlesPause += PauseEmission;
    }

    public void SetActive(bool value)
    {
        ParticleSystem.EmissionModule emission;
        for (int i = 0; i < particles.Length; i++)
        {
            emission = particles[i].emission;
            emission.rateOverTime = value ? defaultEmissionRates[i] : 0f;
        }
    }
    
    [ContextMenu("Get Particles")]
    public void GetParticles()
    {
        particles = gameObject.GetComponentsInChildren<ParticleSystem>();
        defaultEmissionRates = new float[particles.Length];
        for (int j = 0; j < particles.Length; j++)
        {
            defaultEmissionRates[j] = particles[j].emission.rateOverTime.constant;
        }
    }

    [ContextMenu("Disable Emission")]
    public void DisableParticles()
    {
        SetActive(false);
    }

    [ContextMenu("Pause Emission")]
    void PauseEmission(bool value)
    {
        for (int i = 0; i < particles.Length; i++)
        {
            if (value) particles[i].Pause();
            else particles[i].Play();
        }
    }

    [ContextMenu("Kill Particles")]
    public void KillCurrentParticles()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Clear();
        }
    }

}
