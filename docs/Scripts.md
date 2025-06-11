# FoldSpace Simulation Scripts Documentation

This document provides a comprehensive overview of the Unity C# scripts powering the FoldSpace multi-agent drone simulation. It reflects the latest implementation, including support for 4D (x, y, z, t) pathfinding, lazy graph instantiation, and real-time visualization.

---

## Core Pathfinding and Grid Infrastructure

### `VolumeGraph.cs`

A dynamic 4D graph used for drone pathfinding. Agents are created on-demand (lazy) based on spatial and temporal parameters.

* **Key Methods:**

  * `GetOrCreateAgent(Vector3Int pos, int t)`: lazily instantiates `VolumeAgent` if within bounds and above terrain
  * `GetNeighbors(VolumeAgent agent)`: returns neighbors at `t+1` in spatial directions only
* **Time Wrapping:** implemented to allow cyclic reuse of limited time steps

### `VolumeAgent.cs`

Represents a single cell in the 4D volume grid.

* **Properties:** `gridPosition`, `timeIndex`, `worldPosition`, `edges`, `neighbors`
* **Reservation:** Drones reserve volumes by ID to prevent collisions

### `VolumeEdge.cs`

Simple data structure representing a weighted directed edge between two `VolumeAgent`s.

---

## Path Planning Algorithms

### `GraphSearchAlgorithms.cs`

Implements pathfinding over a lazily constructed 4D graph.

* `AStar(start, goal)` — standard A\* with time and space heuristics
* `Dijkstra(start)` — used for reachability analysis
* `BreadthFirstSearch(start)` — lightweight visual reachability
* `DFS(start, goal)` — fallback search mode

All use `VolumeGraph.GetNeighbors(...)` to ensure lazy expansion.

---

## Utilities

### `VolumeUtils.cs`

Provides utility functions to find the nearest valid `VolumeAgent` given a world position.

* `FindClosestAgent(...)`
* `FindClosestAgentAboveHeight(...)`
* `WrapTimeIndex(int)` — modulo wrapping for cyclic time grids

---

## Drone and Simulation Management

### `SimulationManager.cs`

Top-level orchestrator that spawns drones and schedules flights at a cadence determined by grid temporal resolution.

* `LaunchDrone(...)`: spawns and routes individual flights
* `InitializeScenario()`: pre-populates queue with N flights
* **Uses**: `UNetController`, `VolumeVisualizer`

### `UNetController.cs`

Handles dynamic route planning and reservation through the volume grid.

* `AssignPath(origin, destination, startTime)` returns a viable path
* Ensures no overlap via `ReserveVolumes(...)`
* Uses A\* from `GraphSearchAlgorithms`

---

## Visual Debug and Labeling

### `VolumeVisualizer.cs`

Renders a debug cube for each `VolumeAgent` in a path using instanced materials with emission.

* `ShowVolumes(path)` — draws debug geometry
* `ClearVisuals()` — removes all children from visual root

### `DroneAgentStatus.cs`

Attaches a floating 3D label and GUI panel to each drone in-flight.

* Displays `callSign`, current volume position `(x, y, z, t)`, and altitude
* Syncs with camera position and updates per frame
* Uses `VolumeUtils.FindClosestAgent(...)` to track position in time

---

## Notes on Lazy Evaluation

* Only `VolumeAgent`s needed for pathfinding are created
* Spatial neighbors are connected only across `t+1`
* Total memory and CPU cost is drastically reduced

---

Let me know when you’re ready to extend this into individual file summaries or add architecture diagrams.
