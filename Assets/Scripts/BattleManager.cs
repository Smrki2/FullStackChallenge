using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI monsterHealthText;
    [SerializeField] private TextMeshProUGUI monsterName;
    [SerializeField] private GameObject heroHealth;
    [SerializeField] private Image[] moves;
    private string showMonsterHealth;
    [SerializeField] private Image monsterImage;
    [SerializeField] private Sprite goblinWarriorSprite;
    [SerializeField] private Sprite goblinMageSprite;
    [SerializeField] private Sprite spiderSprite;
    [SerializeField] private Sprite witchSprite;
    [SerializeField] private Sprite dragonSprite;
    [SerializeField] private Image heroHealthBar;
    [SerializeField] private Image monsterHealthBar;

    enum Entity
    {
        Hero,
        Monster
    }
    
    void Start()
    {
        SetupMonster();
        SetupPlayer();
    }
    private void SetupMonster()
    {
        monsterImage.sprite = GetMonsterSprite(GameManager.instance.currentMonster.id);
        GameManager.instance.monsterEffects.Clear();
        GameManager.instance.monsterCurrentHealth = GameManager.instance.currentMonster.hp;
        monsterName.text = GameManager.instance.currentMonster.name;
        showMonsterHealth = GameManager.instance.monsterCurrentHealth.ToString() + "/"+ GameManager.instance.currentMonster.hp.ToString();
        monsterHealthText.text = showMonsterHealth;
    }
    private void SetupPlayer()
    {
        GameManager.instance.heroCurrentHealth = GameManager.instance.heroStats.hp;
        GameManager.instance.heroEffects.Clear();
        heroHealth.GetComponentInChildren<TextMeshProUGUI>().text = GameManager.instance.heroCurrentHealth.ToString() + "/" + GameManager.instance.heroStats.hp.ToString();
        ShowEquippedMoves(moves);
    }
    private void ShowEquippedMoves(Image[] moveSlots)
    {
        for (int i = 0; i < GameManager.instance.equippedMoves.Count; i++)
        {
            Sprite icon = Object.FindAnyObjectByType<SpriteIconManager>().GetMoveSprite(GameManager.instance.equippedMoves[i].id);
            if (icon != null)
                moveSlots[i].GetComponent<Image>().sprite = icon;
            MoveSlot slot = moveSlots[i].GetComponent<MoveSlot>();
            slot.move = GameManager.instance.equippedMoves[i];
            moveSlots[i].GetComponentInChildren<TextMeshProUGUI>().text = GameManager.instance.equippedMoves[i].name;
            moveSlots[i].GetComponent<Button>().onClick.RemoveAllListeners();
            moveSlots[i].GetComponent<Button>().onClick.AddListener(() => OnMoveSelected(slot));
        }
    }
    public void OnMoveSelected(MoveSlot slot)
    {
        ExecuteMove(slot.move, Entity.Hero);
        TickEffects(Entity.Hero);
        UpdateHealthUI();
        CheckForDeath();
        StartCoroutine(FetchMonsterMove());
    }
    private void ExecuteMove(Move move, Entity entity)
    {
        bool isHero = entity == Entity.Hero;
        Stats monsterStats = new Stats();
        monsterStats.hp = GameManager.instance.currentMonster.hp;
        monsterStats.attack = GameManager.instance.currentMonster.attack;
        monsterStats.defense = GameManager.instance.currentMonster.defense;
        monsterStats.magic = GameManager.instance.currentMonster.magic;
        Stats casterStats = isHero ? ApplyEffects(GameManager.instance.heroStats, GameManager.instance.heroEffects) : ApplyEffects(monsterStats, GameManager.instance.monsterEffects);
        Stats targetStats = !isHero ? ApplyEffects(GameManager.instance.heroStats, GameManager.instance.heroEffects) : ApplyEffects(monsterStats, GameManager.instance.monsterEffects);

        int damage = 0;
        switch (move.effect)
        {
            case "damage":
                {
                    if (move.type == "physical")
                    {
                        damage = casterStats.attack + move.value - targetStats.defense;
                        damage = Mathf.Max(0, damage);
                    }
                    else
                    {
                        damage = casterStats.magic + move.value;
                    }
                    if (isHero)
                        GameManager.instance.monsterCurrentHealth -= damage;
                    else
                        GameManager.instance.heroCurrentHealth -= damage;
                    break;
                }
            case "damage_debuff_defense":
                {
                    if (move.type == "physical")
                    {
                        damage = casterStats.attack + move.value - targetStats.defense;
                        damage = Mathf.Max(0, damage);
                    }
                    else
                    {
                        damage = casterStats.magic + move.value;
                    }
                    if (damage > 0)
                    {
                        ActiveEffect effect = new ActiveEffect();
                        effect.affectedStat = "defense";
                        effect.value = -move.value;
                        effect.turnsRemaining = 2;
                        if (isHero)
                        {
                            GameManager.instance.monsterEffects.Add(effect);
                            GameManager.instance.monsterCurrentHealth -= damage;
                        }
                        else
                        {
                            GameManager.instance.heroEffects.Add(effect);
                            GameManager.instance.heroCurrentHealth -= damage;
                        }
                    }
                    break;
                }
            case "damage_debuff_magic":
                {
                    if (move.type == "physical")
                    {
                        damage = casterStats.attack + move.value - targetStats.defense;
                        damage = Mathf.Max(0, damage);
                    }
                    else
                    {
                        damage = casterStats.magic + move.value;
                    }
                    if (damage > 0)
                    {
                        ActiveEffect effect = new ActiveEffect();
                        effect.affectedStat = "magic";
                        effect.value = -move.value;
                        effect.turnsRemaining = 2;
                        if (isHero)
                        {
                            GameManager.instance.monsterEffects.Add(effect);
                            GameManager.instance.monsterCurrentHealth -= damage;
                        }
                        else
                        {
                            GameManager.instance.heroEffects.Add(effect);
                            GameManager.instance.heroCurrentHealth -= damage;
                        }
                    }
                    break;
                }
            case "damage_heal":
                {
                    if (move.type == "physical")
                    {
                        damage = casterStats.attack + move.value - targetStats.defense;
                        damage = Mathf.Max(0, damage);
                    }
                    else
                    {
                        damage = casterStats.magic + move.value;
                    }
                    if (isHero)
                    {
                        GameManager.instance.monsterCurrentHealth -= damage;
                        GameManager.instance.heroCurrentHealth += damage;
                        GameManager.instance.heroCurrentHealth = Mathf.Min(GameManager.instance.heroCurrentHealth, GameManager.instance.heroStats.hp);
                    }
                    else
                    {
                        GameManager.instance.heroCurrentHealth -= damage;
                        GameManager.instance.monsterCurrentHealth += damage;
                        GameManager.instance.monsterCurrentHealth = Mathf.Min(GameManager.instance.monsterCurrentHealth, GameManager.instance.currentMonster.hp);
                    }
                    break;
                }
            case "buff_attack":
                {
                    ActiveEffect effect = new ActiveEffect();
                    effect.affectedStat = "attack";
                    effect.value = move.value;
                    effect.turnsRemaining = 2;
                    if (isHero)
                        GameManager.instance.heroEffects.Add(effect);
                    else
                        GameManager.instance.monsterEffects.Add(effect);
                    break;
                }
            case "buff_defense":
                {
                    ActiveEffect effect = new ActiveEffect();
                    effect.affectedStat = "defense";
                    effect.value = move.value;
                    effect.turnsRemaining = 2;
                    if (isHero)
                        GameManager.instance.heroEffects.Add(effect);
                    else
                        GameManager.instance.monsterEffects.Add(effect);
                    break;
                }
            case "buff_magic":
                {
                    ActiveEffect effect = new ActiveEffect();
                    effect.affectedStat = "magic";
                    effect.value = move.value;
                    effect.turnsRemaining = 2;
                    if (isHero)
                        GameManager.instance.heroEffects.Add(effect);
                    else
                        GameManager.instance.monsterEffects.Add(effect);
                    break;
                }
            case "buff_magic_cost_hp":
                {
                    ActiveEffect effect = new ActiveEffect();
                    effect.affectedStat = "magic";
                    effect.value = move.value;
                    effect.turnsRemaining = 2;
                    damage = move.value;
                    if (isHero)
                    {
                        GameManager.instance.heroEffects.Add(effect);
                        GameManager.instance.heroCurrentHealth -= damage;
                    }
                    else
                    {
                        GameManager.instance.monsterEffects.Add(effect);
                        GameManager.instance.monsterCurrentHealth -= damage;
                    }
                    break;
                }
            case "debuff_attack":
                {
                    ActiveEffect effect = new ActiveEffect();
                    effect.affectedStat = "attack";
                    effect.value = -move.value;
                    effect.turnsRemaining = 2;
                    if (isHero)
                        GameManager.instance.monsterEffects.Add(effect);
                    else
                        GameManager.instance.heroEffects.Add(effect);
                    break;
                }
            case "heal":
                {
                    int healAmount = casterStats.magic + move.value;
                    if(isHero)
                    {
                        GameManager.instance.heroCurrentHealth += healAmount;
                        GameManager.instance.heroCurrentHealth = Mathf.Min(GameManager.instance.heroCurrentHealth, GameManager.instance.heroStats.hp);
                    }
                    else
                    {
                        GameManager.instance.monsterCurrentHealth += healAmount;
                        GameManager.instance.monsterCurrentHealth = Mathf.Min(GameManager.instance.monsterCurrentHealth, GameManager.instance.currentMonster.hp);
                    }
                    break;
                }
            default:
                {
                    Debug.Log("Unknown Effect: " + move.effect);
                    break;
                }
        }
    }
    private void TickEffects(Entity entity)
    {
        bool isHero = entity == Entity.Hero;
        if (isHero)
        {
            foreach (ActiveEffect effect in GameManager.instance.heroEffects)
            {
                effect.turnsRemaining--;
            }
            GameManager.instance.heroEffects.RemoveAll(effect => effect.turnsRemaining <= 0);
        }
        else
        {
            foreach (ActiveEffect effect in GameManager.instance.monsterEffects)
            {
                effect.turnsRemaining--;
            }
            GameManager.instance.monsterEffects.RemoveAll(effect => effect.turnsRemaining <= 0);
        }
    }
    private void CheckForDeath()
    {
        if(GameManager.instance.monsterCurrentHealth <= 0)
        {
            GameManager.instance.currentMonsterIndex = Mathf.Max(GameManager.instance.currentMonsterIndex, GameManager.instance.lastFoughtMonsterIndex + 1);
            GameManager.instance.heroXp += GameManager.instance.config.hero.xp_per_battle;
            if (GameManager.instance.heroXp >= GameManager.instance.config.hero.xp_to_level_up)
            {
                GameManager.instance.heroXp -= GameManager.instance.config.hero.xp_to_level_up;
                GameManager.instance.heroLevel++;
                GameManager.instance.heroStats.hp += GameManager.instance.config.hero.level_up_stats.hp;
                GameManager.instance.heroStats.attack += GameManager.instance.config.hero.level_up_stats.attack;
                GameManager.instance.heroStats.defense += GameManager.instance.config.hero.level_up_stats.defense;
                GameManager.instance.heroStats.magic += GameManager.instance.config.hero.level_up_stats.magic;
            }
            Move learnedMove = GameManager.instance.currentMonster.moves[Random.Range(0, GameManager.instance.currentMonster.moves.Count)];
            GameManager.instance.AddMove(learnedMove);
            GameManager.instance.lastLearnedMove = learnedMove;
            SceneManager.LoadScene("PostBattleScene");
        }
        else if(GameManager.instance.heroCurrentHealth <= 0)
        {
            SceneManager.LoadScene("MapScene");
        }
    }
    private void UpdateHealthUI()
    {
        monsterHealthText.text = GameManager.instance.monsterCurrentHealth + "/" + GameManager.instance.currentMonster.hp;
        heroHealth.GetComponentInChildren<TextMeshProUGUI>().text = GameManager.instance.heroCurrentHealth + "/" + GameManager.instance.heroStats.hp;
        heroHealthBar.fillAmount = (float)GameManager.instance.heroCurrentHealth / GameManager.instance.heroStats.hp;
        monsterHealthBar.fillAmount = (float)GameManager.instance.monsterCurrentHealth / GameManager.instance.currentMonster.hp;
    }
    private IEnumerator FetchMonsterMove()
    {
        string url = "http://localhost:5000/battle/monster-move?" +
            "monster_id=" + GameManager.instance.currentMonster.id +
            "&monster_hp=" + GameManager.instance.monsterCurrentHealth +
            "&monster_max_hp=" + GameManager.instance.currentMonster.hp +
            "&hero_hp=" + GameManager.instance.heroCurrentHealth;

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            MonsterMoveResponse response = JsonUtility.FromJson<MonsterMoveResponse>(request.downloadHandler.text);
            ExecuteMove(response.move, Entity.Monster);
            TickEffects(Entity.Monster);
            UpdateHealthUI();
            CheckForDeath();
        }
    }
    private Stats ApplyEffects(Stats stats, List<ActiveEffect> effects)
    {
        Stats modified = new Stats();
        modified.hp = stats.hp;
        modified.attack = stats.attack;
        modified.defense = stats.defense;
        modified.magic = stats.magic;

        foreach (ActiveEffect effect in effects)
        {
            switch (effect.affectedStat)
            {
                case "attack": 
                    modified.attack += effect.value; 
                    break;
                case "defense": 
                    modified.defense += effect.value; 
                    break;
                case "magic": 
                    modified.magic += effect.value; 
                    break;
            }
        }
        return modified;
    }
    private Sprite GetMonsterSprite(string monsterId)
    {
        switch (monsterId)
        {
            case "dragon": return dragonSprite;
            case "witch": return witchSprite;
            case "giant_spider": return spiderSprite;
            case "goblin_warrior": return goblinWarriorSprite;
            case "goblin_mage": return goblinMageSprite;
            default: return null;
        }
    }
}
