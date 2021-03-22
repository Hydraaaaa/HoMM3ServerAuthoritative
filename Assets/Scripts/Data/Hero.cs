using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
public class Hero : ScriptableObject
{
    [FormerlySerializedAs("m_Portrait")]
    public Sprite Portrait;
}
