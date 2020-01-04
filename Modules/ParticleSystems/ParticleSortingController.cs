using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSortingController : MonoBehaviour
{
    [SerializeField] int sortingOrderOffset;
    [SerializeField] SortingGroup[] sortingGroups;

    private void Reset()
    {
        var renders = GetComponentsInChildren<ParticleSystemRenderer>();
        sortingGroups = new SortingGroup[renders.Length];
        for (int i = 0; i < renders.Length; i++)
        {
            sortingGroups[i] = new SortingGroup()
            {
                editorName = renders[i].gameObject.name,
                renders = new ParticleSystemRenderer[] { renders[i] },
                sortingOrder = i
            };
        }
    }

    public void SetSortingOrder(int reference)
    {
        for (int i = 0; i < sortingGroups.Length; i++)
        {
            for (int j = 0; j < sortingGroups[i].renders.Length; j++)
            {
                sortingGroups[i].renders[j].sortingOrder = reference + sortingGroups[i].sortingOrder;
            }
        }
    }

    [System.Serializable]
    struct SortingGroup
    {
        public string editorName;
        public ParticleSystemRenderer[] renders;
        public int sortingOrder;
    }
}
