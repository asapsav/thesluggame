using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bacteria", menuName = "Bacteria/New Bacteria")]
public class BacteriaTypes : ScriptableObject
{
    public string bacteriaType;
    public int unitCost;
}
