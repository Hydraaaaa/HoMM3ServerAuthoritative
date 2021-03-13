using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StartingBonus : ScriptableObject
{
    public Sprite Sprite => m_Sprite;

    [SerializeField] Sprite m_Sprite;
}
