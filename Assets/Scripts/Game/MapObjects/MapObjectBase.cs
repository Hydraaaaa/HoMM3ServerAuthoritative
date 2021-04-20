using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjectBase : MonoBehaviour
{
    public byte[] MouseCollision { get; set; }
    public byte[] InteractionCollision { get; set; }

    protected GameReferences m_GameReferences;

    public virtual void OnRightClick()
    {

    }
}
