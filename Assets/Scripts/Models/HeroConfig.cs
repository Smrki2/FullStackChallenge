using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class HeroConfig
{
    public Stats base_stats;
    public List<Move> default_moves;
    public Stats level_up_stats;
    public int xp_per_battle;
    public int xp_to_level_up;
}
