from flask import Flask, jsonify, request
import random

app = Flask(__name__)


MONSTERS = [
    {
        "id": "goblin_warrior",
        "name": "Goblin Warrior",
        "hp": 60,
        "attack": 10,
        "defense": 4,
        "magic": 3,
        "moves": [
            {"id": "rusty_blade",  "name": "Rusty Blade",  "type": "physical", "effect": "damage",                  "value": 12},
            {"id": "dirty_kick",   "name": "Dirty Kick",   "type": "physical", "effect": "damage_debuff_defense",   "value": 7},
            {"id": "frenzy",       "name": "Frenzy",       "type": "none",     "effect": "buff_attack",             "value": 5},
            {"id": "headbutt",     "name": "Headbutt",     "type": "physical", "effect": "damage",                  "value": 18},
        ]
    },
    {
        "id": "goblin_mage",
        "name": "Goblin Mage",
        "hp": 55,
        "attack": 5,
        "defense": 3,
        "magic": 14,
        "moves": [
            {"id": "firebolt",     "name": "Firebolt",     "type": "magic",    "effect": "damage",              "value": 12},
            {"id": "arcane_surge", "name": "Arcane Surge", "type": "none",     "effect": "buff_magic",          "value": 6},
            {"id": "mana_drain",   "name": "Mana Drain",   "type": "magic",    "effect": "damage_debuff_magic", "value": 7},
            {"id": "hex_shield",   "name": "Hex Shield",   "type": "none",     "effect": "buff_defense",        "value": 6},
        ]
    },
    {
        "id": "giant_spider",
        "name": "Giant Spider",
        "hp": 75,
        "attack": 13,
        "defense": 6,
        "magic": 4,
        "moves": [
            {"id": "bite",         "name": "Bite",         "type": "physical", "effect": "damage",                  "value": 13},
            {"id": "web_throw",    "name": "Web Throw",    "type": "physical", "effect": "damage_debuff_defense",   "value": 8},
            {"id": "pounce",       "name": "Pounce",       "type": "physical", "effect": "damage",                  "value": 20},
            {"id": "skitter",      "name": "Skitter",      "type": "none",     "effect": "buff_defense",            "value": 6},
        ]
    },
    {
        "id": "witch",
        "name": "Witch",
        "hp": 80,
        "attack": 7,
        "defense": 5,
        "magic": 18,
        "moves": [
            {"id": "shadow_bolt",  "name": "Shadow Bolt",  "type": "magic",    "effect": "damage",              "value": 22},
            {"id": "drain_life",   "name": "Drain Life",   "type": "magic",    "effect": "damage_heal",         "value": 10},
            {"id": "curse",        "name": "Curse",        "type": "none",     "effect": "debuff_attack",       "value": 5},
            {"id": "dark_pact",    "name": "Dark Pact",    "type": "none",     "effect": "buff_magic_cost_hp",  "value": 8},
        ]
    },
    {
        "id": "dragon",
        "name": "Dragon",
        "hp": 120,
        "attack": 18,
        "defense": 12,
        "magic": 20,
        "moves": [
            {"id": "flame_breath", "name": "Flame Breath", "type": "magic",    "effect": "damage",          "value": 28},
            {"id": "claw_swipe",   "name": "Claw Swipe",   "type": "physical", "effect": "damage",          "value": 20},
            {"id": "intimidate",   "name": "Intimidate",   "type": "none",     "effect": "debuff_attack",   "value": 6},
            {"id": "dragon_scales","name": "Dragon Scales","type": "none",     "effect": "buff_defense",    "value": 8},
        ]
    },
]

HERO_DEFAULT_MOVES = [
    {"id": "slash",       "name": "Slash",       "type": "physical", "effect": "damage",       "value": 14},
    {"id": "shield_up",   "name": "Shield Up",   "type": "none",     "effect": "buff_defense", "value": 6},
    {"id": "battle_cry",  "name": "Battle Cry",  "type": "none",     "effect": "buff_attack",  "value": 6},
    {"id": "second_wind", "name": "Second Wind", "type": "magic",    "effect": "heal",         "value": 15},
]

HERO_BASE_STATS = {
    "hp": 100,
    "attack": 12,
    "defense": 6,
    "magic": 8,
}

LEVEL_UP_STATS = {
    "hp": 10,
    "attack": 2,
    "defense": 1,
    "magic": 2,
}

XP_PER_BATTLE = 30
XP_TO_LEVEL_UP = 100


@app.route('/run/config', methods=['GET'])
def get_run_config():
    return jsonify({
        "monsters": MONSTERS,
        "hero": {
            "base_stats": HERO_BASE_STATS,
            "level_up_stats": LEVEL_UP_STATS,
            "default_moves": HERO_DEFAULT_MOVES,
            "xp_per_battle": XP_PER_BATTLE,
            "xp_to_level_up": XP_TO_LEVEL_UP,
        }
    })


@app.route('/battle/monster-move', methods=['GET'])
def get_monster_move():
    monster_id  = request.args.get('monster_id')
    monster_hp  = int(request.args.get('monster_hp', 100))
    monster_max = int(request.args.get('monster_max_hp', 100))
    hero_hp     = int(request.args.get('hero_hp', 100))

    monster = next((m for m in MONSTERS if m["id"] == monster_id), None)
    if not monster:
        return jsonify({"error": "Monster not found"}), 404

    chosen_move = random.choice(monster["moves"])

    return jsonify({"move": chosen_move})


if __name__ == '__main__':
    app.run(debug=True, port=5000)