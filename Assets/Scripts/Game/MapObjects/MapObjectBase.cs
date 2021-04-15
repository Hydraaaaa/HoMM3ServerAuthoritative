using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjectBase : MonoBehaviour
{
    public byte[] MouseCollision { get; set; }
    public byte[] InteractionCollision { get; set; }

    public virtual void OnRightClick()
    {

    }
}
