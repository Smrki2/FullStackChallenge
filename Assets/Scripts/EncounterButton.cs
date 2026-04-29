using UnityEngine;
using UnityEngine.SceneManagement;

public class EncounterButton : MonoBehaviour
{
    public Monster monster;

    public void OnClick()
    {
        GameManager.instance.currentMonster = monster;
        SceneManager.LoadScene("BattleScene");
    }
}
