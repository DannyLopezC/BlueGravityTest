using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "Clothes_", menuName = "Clothes/Clothes")]
public class Clothes : ScriptableObject
{
    public int id;
    public int price;

    public Sprite sprite;
}