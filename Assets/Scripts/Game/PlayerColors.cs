using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerColors : ScriptableObject
{
    [Serializable]
    public class PlayerElements
    {
        public Sprite Sidebar;
        public Sprite SidebarSmall;
        public Sprite BorderTopLeft;
        public Sprite BorderTop;
        public Sprite BorderLeft;
        public Sprite BorderRight;
        public Sprite BorderBottomLeft;
        public Sprite BorderBottomRight;
        public Sprite Resources;
        public Sprite BottomBarFill;
        public Sprite Date;
    }

    public PlayerElements[] Elements;
    public HeroFlagVisualData[] Flags;
    public Color[] Colors;
}
