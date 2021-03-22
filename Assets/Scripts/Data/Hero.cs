using UnityEngine;

[CreateAssetMenu]
public class Hero : ScriptableObject
{
    public int ID => m_ID;

    [SerializeField] int m_ID;
    public Sprite Portrait;
}
