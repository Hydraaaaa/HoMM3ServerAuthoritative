using UnityEngine;

[CreateAssetMenu]
public class Hero : ScriptableObject
{
    public Sprite Portrait => m_Portrait;

    [SerializeField] Sprite m_Portrait;
}
