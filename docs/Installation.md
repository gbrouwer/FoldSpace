# Installation.md — FoldSpace Setup Guide

This guide describes the steps required to build and run the FoldSpace drone simulation in Unity.

---

## Unity Requirements

* **Version:** Unity 6000.1.16 or later (Unity 6.x verified)
* **Render Pipeline:** Universal Render Pipeline (URP)
* **Scripting Backend:** IL2CPP or Mono
* **Target Platform:** PC/Mac/Linux Standalone (x86\_64)

---

## Initial Setup

1. **Clone the Repository**

   ```bash
   git clone <your-foldspace-repo-url>
   ```

2. **Open the Project**
   Launch Unity Hub and add the root folder as a new project. Ensure the Unity version matches 6000.1.16.

3. **Install Dependencies**

   * If using [SimplePriorityQueue](https://github.com/BlueRaja/High-Speed-Priority-Queue), ensure it is added to `Assets/Plugins`.
   * URP is required; if not present, install via Package Manager and convert all materials.
   * TMP Essentials will be auto-installed by Unity if missing.

4. **Scene Setup**

   * Load `Scenes/Foldspace_Main.unity`
   * Verify that `SimulationManager`, `UNetController`, and `VolumeGridBuilder` exist in the hierarchy.
   * Check that the terrain object is active and large enough to contain the grid.

5. **Run the Simulation**

   * Press Play
   * Observe drones spawning at spokes and flying to the central hub
   * Watch debug volumes appear in world space and GUI panels update

---

## Folder Structure

* `Assets/Scripts/` — All C# components (agents, controllers, planners, visualization)
* `Assets/Scenes/` — Scene files
* `Assets/Prefabs/` — Prefab assets for drones, volumes, ATC
* `Assets/Materials/` — URP-compliant materials
* `Assets/Resources/` — Runtime-loaded assets like drone models

---

## Optional Tools

* **Cinemachine** (Recommended): For camera blending and free orbit views
* **TextMeshPro**: Required for 3D status labels above drones
* **Graph Visualization Helpers**: For developers debugging `VolumeGraph` reachability

---

## Troubleshooting

### Drone Fails to Spawn

* Ensure terrain is present and high enough to allow drone grid clearance
* Increase `globalOffsetY` on `VolumeGridBuilder`

### No Path Found

* Increase `totalTimeSteps`
* Reduce `spawnInterval` to ensure drones do not collide

### Graph is Too Large

* Use lazy instantiation (default)
* Reduce `countX`, `countY`, `countZ` or increase `volSize`

---

## Build Instructions

1. File → Build Settings
2. Add current scene
3. Set platform to `PC, Mac & Linux Standalone`
4. Set architecture to `x86_64`
5. Build and run

---

This setup creates a working FoldSpace simulation environment, suitable for experimenting with centralized 4D drone routing.
