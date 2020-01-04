using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Particle Colors", menuName = "Data/Presets/ParticleColorPresets")]
public class ParticleColorValues : ScriptableObject
{
    public ParticleSystem.MinMaxGradient[] values;
}
