using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Button[] encounters;
    [SerializeField] private Image[] moves;
    [SerializeField] private Image[] panelMoves;

    [SerializeField] private GameObject movesPanel;
    [SerializeField] private Transform learnedMovesGrid;
    [SerializeField] private GameObject movePrefab;

    private Move selectedMove = null;


    void Start()
    {
        StartCoroutine(FetchRunConfig());
    }

    private IEnumerator FetchRunConfig()
    {
        if (GameManager.instance.config != null && GameManager.instance.config.monsters != null && GameManager.instance.config.monsters.Count > 0)
        {
            PopulateEncounters(GameManager.instance.config);
            ShowEquippedMoves(moves);
            ShowEquippedMovesPanel(panelMoves);
            yield break;
        }
        else
        {
            UnityWebRequest runConfig = UnityWebRequest.Get("http://localhost:5000/run/config");
            yield return runConfig.SendWebRequest();

            if (runConfig.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(runConfig.error);
            }
            else
            {
                RunConfig config = JsonUtility.FromJson<RunConfig>(runConfig.downloadHandler.text);
                GameManager.instance.config = config;
                PopulateEncounters(GameManager.instance.config);
                GameManager.instance.EquipAndLearnDefaultMoves();
                ShowEquippedMoves(moves);
                ShowEquippedMovesPanel(panelMoves);
            }
        }
    }
    private void PopulateEncounters(RunConfig config)
    {
        for (int i = 0; i < config.monsters.Count; i++)
        {
            encounters[i].GetComponentInChildren<TextMeshProUGUI>().text = config.monsters[i].name;
        }
    }
    private void ShowEquippedMoves(Image[] moveSlots)
    {
        for (int i = 0; i < GameManager.instance.equippedMoves.Count; i++)
        {
            MoveSlot slot = moveSlots[i].GetComponent<MoveSlot>();
            slot.move = GameManager.instance.equippedMoves[i];
            moveSlots[i].GetComponentInChildren<TextMeshProUGUI>().text = GameManager.instance.equippedMoves[i].name;
        }
    }
    private void ShowEquippedMovesPanel(Image[] moveSlots)
    {
        for (int i = 0; i < GameManager.instance.equippedMoves.Count; i++)
        {
            MoveSlot slot = moveSlots[i].GetComponent<MoveSlot>();
            slot.move = GameManager.instance.equippedMoves[i];
            moveSlots[i].GetComponentInChildren<TextMeshProUGUI>().text = GameManager.instance.equippedMoves[i].name;
            moveSlots[i].GetComponent<Button>().onClick.RemoveAllListeners();
            moveSlots[i].GetComponent<Button>().onClick.AddListener(() => OnEquippedMoveClick(slot));
        }
    }
    private void ShowLearnedMoves()
    {
        foreach(Transform child in learnedMovesGrid)
        {
            Destroy(child.gameObject);
        }
        foreach(Move move in GameManager.instance.learnedMoves)
        {
            GameObject moveSlot = Instantiate(movePrefab, learnedMovesGrid);
            MoveSlot slot = moveSlot.GetComponent<MoveSlot>();
            slot.move = move;
            moveSlot.GetComponentInChildren<TextMeshProUGUI>().text = move.name;
            moveSlot.GetComponent<Button>().onClick.AddListener(() => OnLearnedMoveClick(slot));
        }
    }
    public void OnMovesClick()
    {
        movesPanel.SetActive(true);
        ShowLearnedMoves();
    }
    public void OnBackClick()
    {
        movesPanel.SetActive(false);
    }
    public void OnLearnedMoveClick(MoveSlot slot)
    {
        if(slot.move == selectedMove)
        {
            selectedMove = null;
        }else
        {
            selectedMove = slot.move;
        }
    }
    public void OnEquippedMoveClick(MoveSlot slot)
    {
        Debug.Log("Selected move: " + (selectedMove != null ? selectedMove.name : "null"));
        Debug.Log("Slot move: " + (slot.move != null ? slot.move.name : "null"));
        if (selectedMove == null || GameManager.instance.equippedMoves.Contains(selectedMove))
            return;
        for(int i = 0; i < GameManager.instance.equippedMoves.Count; i++)
        {
            if (GameManager.instance.equippedMoves[i] == slot.move)
            {
                GameManager.instance.equippedMoves[i] = selectedMove;
                selectedMove = null;
                break;
            }
        }
        ShowEquippedMoves(moves);
        ShowEquippedMoves(panelMoves);
    }
}
