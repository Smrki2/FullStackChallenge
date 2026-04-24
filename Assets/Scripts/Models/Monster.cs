using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Monster
{
    public int attack;
    public int defense;
    public int hp;
    public string id;
    public int magic;
    public List<Move> moves;
    public string name;
}
