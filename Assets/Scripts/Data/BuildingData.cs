using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Heroes 3/Building Data")]
public class BuildingData : ScriptableObject
{
    public string DisplayName => m_DisplayName;
    public string Description => m_Description;

    public int WoodCost => m_WoodCost;
    public int MercuryCost => m_MercuryCost;
    public int OreCost => m_OreCost;
    public int SulfurCost => m_SulfurCost;
    public int CrystalCost => m_CrystalCost;
    public int GemCost => m_GemCost;
    public int GoldCost => m_GoldCost;
    public BuildingRequirements Requirements => m_Requirements;

    [SerializeField] string m_DisplayName;
    [SerializeField] string m_Description;

    [SerializeField] int m_WoodCost;
    [SerializeField] int m_MercuryCost;
    [SerializeField] int m_OreCost;
    [SerializeField] int m_SulfurCost;
    [SerializeField] int m_CrystalCost;
    [SerializeField] int m_GemCost;
    [SerializeField] int m_GoldCost;
    [SerializeField] BuildingRequirements m_Requirements;
}
