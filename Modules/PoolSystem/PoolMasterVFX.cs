/**
 * PoolMasterVFX.cs
 * Created by: Joao Borks [joao.borks@gmail.com]
 * Created on: 18/07/17 (dd/mm/yy)
 */

using UnityEngine;
using System.Collections.Generic;

public class PoolMasterVFX : PoolMasterBase<PoolMasterVFX>
{
    const string PoolSectionName = "VFX";

    /// <summary>
    /// Stores all pool data as a <see cref="ScriptableObject"/>. The asset is get by <see cref="Resources.Load{T}(string)"/>
    /// </summary>
    VFXPoolData data;
    /// <summary>
    /// Stores a <see cref="PoolSystem"/> for each vfx type
    /// </summary>
    PoolSystem[] vfxPools;
    /// <summary>
    /// Allows acessing the <see cref="vfxPools"/> by using the name of the effects
    /// </summary>
    Dictionary<string, int> arrayInterpreter;

    protected override void Awake()
    {
        data = Resources.Load<VFXPoolData>("VFXPoolData");
        base.Awake();
    }

    #region Pool Functions
    public PoolSystem GetVFXPool(string vfxName)
    {
        return vfxPools[arrayInterpreter[vfxName]];
    }

    public GameObject GetVFX(string vfxName)
    {
		return GetVFXPool(vfxName).GetObject();
    }
    #endregion Pool Functions

    /// <summary>
    /// Creates the <see cref="PoolSystem"/>s and fills the collections
    /// </summary>
    protected override void SetupPools()
    {
        var length = data.vfxPoolPresets.Length;
        arrayInterpreter = new Dictionary<string, int>(length);
        vfxPools = new PoolSystem[length];

        PoolPreset preset;
        for (int i = 0; i < length; i++)
        {
            preset = data.vfxPoolPresets[i];
            arrayInterpreter.Add(preset.presetName, i);
            vfxPools[i] = new PoolSystem(preset.poolPrefab, poolParent, preset.poolSize, PoolSectionName);
        }
    }
}