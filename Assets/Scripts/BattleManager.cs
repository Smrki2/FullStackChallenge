using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI monsterHealthText;
    [SerializeField] private TextMeshProUGUI monsterName;
    [SerializeField] private GameObject heroHealth;
    [SerializeField] private Image[] moves;
    private string showMonsterHealth;
    private int monsterHealth;
    
    void Start()
    {
        SetupMonster();
        SetupPlayer();
    }
    private void SetupMonster()
    {
        monsterHealth = GameManager.instance.currentMonster.hp;
        monsterName.text = GameManager.instance.currentMonster.name;
        showMonsterHealth = GameManager.instance.currentMonster.hp +"/"+monsterHealth.ToString();
        monsterHealthText.text = showMonsterHealth;
    }
    private void SetupPlayer()
    {
        heroHealth.GetComponentInChildren<TextMeshProUGUI>().text = GameManager.instance.heroCurrentHealth.ToString() + "/" + GameManager.instance.heroStats.hp.ToString();
        ShowEquippedMoves(moves);
    }
    private void ShowEquippedMoves(Image[] moveSlots)
    {
        for (int i = 0; i < GameManager.instance.equippedMoves.Count; i++)
        {
            MoveSlot slot = moveSlots[i].GetComponent<MoveSlot>();
            slot.move = GameManager.instance.equippedMoves[i];
            moveSlots[i].GetComponentInChildren<TextMeshProUGUI>().text = GameManager.instance.equippedMoves[i].name;
            moveSlots[i].GetComponent<Button>().onClick.RemoveAllListeners();
            moveSlots[i].GetComponent<Button>().onClick.AddListener(() => OnMoveSelected(slot));
        }
    }
    public void OnMoveSelected(MoveSlot slot)
    {

    }
}
