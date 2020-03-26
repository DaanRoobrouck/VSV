using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Create Card", menuName = "Card")]
public class CardCreator : ScriptableObject
{
    public int Index;
    public Sprite Icon;
    public string Description;
}
