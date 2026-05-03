# RPG Gauntlet - Nordeus Full Stack Challenge

## How to Run

### Server
cd server
py app.py
Server runs on http://localhost:5000

### Client
1. Open Unity project in Unity Hub
2. Open Map scene
3. Press Play

## Tech Stack
- **Server:** Python + Flask
- **Client:** Unity 2D (C#)

## Architecture
- Server handles game configuration and monster AI
- Client handles all gameplay, UI and state management
- Two REST endpoints:
  - `GET /run/config` - fetches monster and hero configuration
  - `GET /battle/monster-move` - fetches monster move based on battle state

## Features
- Turn-based RPG combat system
- 5 monsters in sequence, unlocked progressively
- Move learning system - learn monster moves on victory
- Move management panel - equip and swap moves
- Buff/debuff system with turn duration
- XP and level up progression with stat increases
- Server-driven monster AI and game configuration

## Gameplay Video
[Watch here](https://youtu.be/q2JEL-y8-J0)