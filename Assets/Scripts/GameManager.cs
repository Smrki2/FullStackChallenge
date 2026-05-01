using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public RunConfig config = null;
    public List<Move> learnedMoves = new List<Move>();
    public List<Move> equippedMoves = new List<Move>();
    public Monster currentMonster;
    public Stats heroStats;
    public int heroCurrentHealth;
    public int heroLevel = 1;
    public int heroXp = 0;



    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AddMove(Move newMove)
    {
        if(!learnedMoves.Contains(newMove))
        {  
            learnedMoves.Add(newMove);
        }
    }
    public void AddMoves(List<Move> newMoves)
    {
        foreach (Move newMove in newMoves)
        {
            if (!learnedMoves.Contains(newMove))
                learnedMoves.Add(newMove);
        }
    }
    public void EquipAndLearnDefaultMoves()
    {
        equippedMoves.Clear();
        equippedMoves.AddRange(config.hero.default_moves);
        AddMoves(config.hero.default_moves);
    }
}
