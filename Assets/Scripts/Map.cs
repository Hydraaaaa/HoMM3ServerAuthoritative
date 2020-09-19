using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : ScriptableObject
{
    public string Name;
    [TextArea(1, 10)]
    public string Description;

    public int Size;
    public bool HasUnderground;
}
