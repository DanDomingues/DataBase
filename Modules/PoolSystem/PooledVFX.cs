/**
 * PooledVFX.cs
 * Created by: Joao Borks [joao.borks@gmail.com]
 * Created on: 18/07/17 (dd/mm/yy)
 */

using UnityEngine;
using System.Collections;

public class PooledVFX : MonoBehaviour, IPoolObject
{
    public PoolSystem Pool { get; set; }

    [SerializeField, Range(.1f, 5)]
    float flashDuration = .5f;

    ParticleSystem pSystem;
    WaitForSeconds lifetimeWait;
    AudioSource source;
    Light flash;
    float origIntensity;

    void Awake()
    {
        source = GetComponentInChildren<AudioSource>();
        flash = GetComponentInChildren<Light>();
        if (flash)
            origIntensity = flash.intensity;
        pSystem = GetComponentInChildren<ParticleSystem>();
        lifetimeWait = new WaitForSeconds(Mathf.Max(pSystem.main.duration, pSystem.main.startLifetimeMultiplier));
    }

    void OnEnable()
    {
        if (source)
            source.pitch = Random.Range(.95f, 1.05f);
        if (flash)
            StartCoroutine(FlashLight());
        StartCoroutine(DisposeAfterDone());
    }

    IEnumerator FlashLight()
    {
        float time = 0;
        while (time < flashDuration)
        {
            time += Time.deltaTime;
            flash.intensity = (1 - (time / flashDuration)) * origIntensity;
            yield return null;
        }
    }

    IEnumerator DisposeAfterDone()
    {
        yield return lifetimeWait;
        Pool.DisposeObject(gameObject);
        if (flash)
            flash.intensity = origIntensity;
    }
}