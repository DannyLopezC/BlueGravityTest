using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable, CreateAssetMenu(fileName = "Weapon_", menuName = "Clothes/Weapons")]
public class Weapon : ScriptableObject
{
    [ReadOnly] public int id;
    public int damage;
    public float force;
    public int price;

    public Sprite sprite;
}