# 📋 OfflineTaskManager

A clean, lightweight, and fully offline Kanban-style task management application built for desktop. 

Manage your projects, drag and drop tasks, and keep your data completely private. Everything is saved locally to your machine—no cloud tracking, no accounts required.

## ✨ Features
* **Completely Offline:** Your data never leaves your computer.
* **Drag-and-Drop Kanban Board:** Effortlessly move tasks between To Do, In Progress, and Done.
* **Multiple Projects:** Create and seamlessly switch between different project boards.
* **Persistent Local Storage:** Instantly saves your progress to a local, readable JSON file.
* **Aesthetic UI:** A clean, distraction-free interface.

## 🚀 How to Install and Run

You don't need Unity or any special software to use this app!

1. Go to the [Releases](../../releases) page on the right side of this repository.
2. Download the latest `.zip` file (e.g., `OfflineTaskManager_v1.0_Windows.zip`).
3. Extract the `.zip` folder anywhere on your computer.
4. Open the extracted folder and double-click `OfflineTaskManager.exe` to start!

*Note: Do not move the `.exe` file out of its folder. It needs the accompanying data files to run properly.*

## 🛠️ For Developers (Building from Source)
If you want to edit the code or see how it works:
1. Clone this repository to your machine.
2. Open the project using **Unity 6000.3.6f1**.
3. Open the main scene located in `Assets/Scenes/`.
4. Hit Play! 

## 📂 Where is my data saved?
Your tasks and projects are safely stored in a `TaskManager_saveData.json` file on your OS-level local app data folder:
* **Windows:** `%userprofile%\AppData\LocalLow\bitemdev\OfflineTaskManager\`