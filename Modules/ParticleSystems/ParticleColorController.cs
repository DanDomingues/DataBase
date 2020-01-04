using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleColorController : MonoBehaviour
{
    [SerializeField] GameObject pivot;
    [SerializeField] ParticleSystem[] systems;

    private void Reset()
    {
        systems = GetComponentsInChildren<ParticleSystem>();
    }
    private void OnDisable()
    {
        pivot.SetActive(false);
    }

    public void StartupWithColors(ParticleColorValues data)
    {
        ParticleSystem.MainModule main;
        for (int i = 0; i < systems.Length; i++)
        {
            main = systems[i].main;
            main.startColor = data.values[i];
        }
        pivot.SetActive(true);
    }
}
