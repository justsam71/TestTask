using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item")]
public class ItemSO : ScriptableObject
{
    public string Id;
    public string DisplayName;
    public Sprite Icon;
    public bool Stackable;
    public int MaxStack = 99;
}