# 🧭 Maui-QuestOrientation

**Maui-QuestOrientation** is an offline Android game built with .NET MAUI. It combines real-world orienteering using a paper map with interactive, text-based quest mechanics.

Players explore physical terrain with a printed map that contains control points (CPs). Each CP is marked with an alphanumeric code. When a player finds a CP in real life and enters its code into the app, a story event is triggered — such as discovering a location or facing a choice in a branching narrative.

---

## 🎯 Key Features

- **Offline gameplay** — no internet connection required.
- **Real-world navigation** using a paper map and physical movement.
- **Alphanumeric code input** at each control point to progress the game.
- **Interactive story events** with choices that influence the game’s direction.
- **Inventory system** — players can collect and use items to affect events or other players.
- **Branching paths** — different choices and actions lead to different storylines and routes.
- **Asynchronous player interaction** — items used on other players generate special codes that can be entered to receive effects.
- **Game progress is saved locally**, allowing the player to continue after closing the app.
- **Main menu** with options to:
  - Start a new game
  - Continue current game
  - Open settings

---

## 📝 Game Flow

1. Player receives a paper map with control points (CPs).
2. Player finds a CP in real life and enters the printed code into the app.
3. If the code matches the current game state, a new event (cutscene) is shown.
4. The player makes choices, collects items, or triggers new branches of the story.
5. Progress is automatically saved.
