using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Slug", menuName = "Slug/New Slug")]
public class SlugTypes : ScriptableObject
{
    public string slugType;
    public int health;
    public bool isArmored;
    public int unitCost;
}
