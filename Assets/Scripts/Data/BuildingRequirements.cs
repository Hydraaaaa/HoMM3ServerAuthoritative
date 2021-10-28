using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This exists because different blacksmiths exist, but common buildings just generally require a blacksmith
[CreateAssetMenu(menuName = "Heroes 3/Building Requirements")]
public class BuildingRequirements : ScriptableObject
{
    public BuildingRequirements[] Requirements => m_Requirements;

    [SerializeField] BuildingRequirements[] m_Requirements;
}
