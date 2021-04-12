using UnityEngine;

[CreateAssetMenu(menuName = "Heroes 3/Hero Container")]
public class HeroContainer : ScriptableObject
{
    public Hero Hero => m_Hero;

    [SerializeField] Hero m_Hero;
}