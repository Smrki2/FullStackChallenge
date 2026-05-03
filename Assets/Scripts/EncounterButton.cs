using UnityEngine;
using UnityEngine.SceneManagement;

public class EncounterButton : MonoBehaviour
{
    public Monster monster;
    public int monsterIndex;

    public void OnClick()
    {
        if (monsterIndex <= GameManager.instance.currentMonsterIndex)
        {
            GameManager.instance.currentMonster = monster;
            GameManager.instance.lastFoughtMonsterIndex = monsterIndex;
            SceneManager.LoadScene("BattleScene");
        }
    }
}
