# Design.md — FoldSpace Design Philosophy and Simulation Goals

This document captures the conceptual foundations and design tradeoffs behind the FoldSpace simulation, focusing on drone behavior, 4D planning space, and the interplay between centralized control and emergent dynamics.

---

## Core Design Principles

### 1. **4D Routing Through Airspace**

Airspace is discretized not only by position (x, y, z) but also by time (t). Each drone navigates a path made of discrete `VolumeAgent`s that represent a unique spatial cell at a specific time index.

* **Benefit:** Prevents spatial collisions between drones sharing similar routes
* **Challenge:** Exponential growth of graph size — solved via lazy instantiation

### 2. **Lazy Graph Construction**

Instead of creating the full grid upfront, each `VolumeAgent` is instantiated only when needed during planning or visualization.

* Implemented in `VolumeGraph.GetOrCreateAgent(...)`
* Ensures scalability even with high-resolution spatial and temporal settings

### 3. **Cyclic Time Indexing**

To keep the time dimension finite, all time indices wrap modulo `totalTimeSteps`. This creates a "rolling horizon" allowing new drones to reuse the grid.

* Implemented via `VolumeUtils.WrapTimeIndex(...)`
* Makes the system sustainable for continuous operation

### 4. **Centralized But Modular Control**

All routing decisions are made by a centralized controller (`UNetController`). However, the architecture is designed to support decentralization later.

* **Current Logic:** Global conflict detection, reservation, and planning
* **Future Extension:** Decentralized UTM with negotiation between ATC controllers

---

## Agent Model

### Drone Behavior

Drones are spawned at a spoke ATC, receive a planned path from UNetController, and then follow that path using Unity physics.

* **FlightIntent** objects encapsulate route metadata
* **DroneAgent.cs** drives motion along world-space points
* **DroneAgentStatus.cs** visualizes logical state in real time

### Path Planning Mechanics

* **Start Node:** Closest valid volume to origin at `t=0 + offset`
* **Goal Node:** Closest valid volume to destination (usually at same t)
* **Search:** `GraphSearchAlgorithms.AStar()` returns a path that respects both spatial and temporal constraints

---

## Visualization Strategy

### World-Space Rendering

* `VolumeVisualizer.cs`: Debug-render each volume in a path as a glowing cube
* Drones are physical 3D objects with attached `TextMeshPro` labels

### UI Representation

* A vertical panel (`DroneInfo`) in the UI Canvas shows current drone metadata
* Updates live using the same logic as the floating labels

---

## Design Constraints

* **Terrain-Aware:** VolumeAgents are not created if their bounding cube intersects with terrain geometry
* **Resolution-Aware:** Grid density and time steps must be balanced for memory and performance
* **Emergence-Ready:** While current drones are deterministic, the structure anticipates emergence under reinforcement learning or imperfect controllers

---

This file defines the simulation's conceptual contract — how design choices support real-time, scalable 4D drone traffic management.
