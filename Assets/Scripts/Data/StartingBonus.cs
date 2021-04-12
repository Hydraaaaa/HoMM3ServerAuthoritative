using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Heroes 3/Starting Bonus")]
public class StartingBonus : ScriptableObject
{
    public Sprite Sprite => m_Sprite;

    [SerializeField] Sprite m_Sprite;
}
