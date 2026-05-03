using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PostBattleManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI learnedMoveText;
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private TextMeshProUGUI levelText;
    void Start()
    {
        learnedMoveText.text = "You learned: " + GameManager.instance.lastLearnedMove.name;
        xpText.text = "XP: " + GameManager.instance.heroXp + "/" + GameManager.instance.config.hero.xp_to_level_up;
        levelText.text = "Level: " + GameManager.instance.heroLevel;
    }
    public void OnContinueClick()
    {
        SceneManager.LoadScene("MapScene");
    }
}
