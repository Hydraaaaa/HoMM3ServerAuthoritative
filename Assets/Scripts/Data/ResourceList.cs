using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Heroes 3/Resource List")]
public class ResourceList : ScriptableObject
{
    public List<MapObjectVisualData> Resources => m_Resources;

    [SerializeField] List<MapObjectVisualData> m_Resources;
}
