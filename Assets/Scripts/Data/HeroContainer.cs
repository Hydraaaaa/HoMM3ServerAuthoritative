using UnityEngine;

[CreateAssetMenu]
public class HeroContainer : ScriptableObject
{
    public Hero Hero => m_Hero;

    [SerializeField] Hero m_Hero;
}