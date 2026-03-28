# OfflineTaskManager

Offline Kanban-style task manager for desktop. No cloud, no accounts, no tracking. Data stays on your machine in a local JSON file.

## Features

- Kanban board with drag-and-drop (To Do, In Progress, Done)
- Multiple projects with separate boards
- Data persisted to a local JSON file
- No internet connection required

## Install

1. Download the latest `.zip` from [Releases](../../releases).
2. Extract anywhere.
3. Run `OfflineTaskManager.exe`.

Don't move the `.exe` out of the extracted folder — it needs the accompanying data files.

## Building from Source

1. Clone this repo.
2. Open in **Unity 6000.3.6f1**.
3. Open the main scene in `Assets/Scenes/`.
4. Hit Play.

## Data Location

Tasks are saved to `TaskManager_saveData.json` in your local app data folder:

- **Windows:** `%userprofile%\AppData\LocalLow\bitemdev\OfflineTaskManager\`
