# Agents.md — FoldSpace Simulation Agents

This file outlines the agents involved in the FoldSpace simulation and their corresponding logic, behaviors, and script-level responsibilities. These agents include both mobile drone agents and infrastructure-side planning systems.

---

## Mobile Agents

### `DroneAgent.cs`

The primary autonomous unit in FoldSpace. Each instance represents a drone navigating through a path of `VolumeAgent` nodes in 4D space (x, y, z, t).

* **Responsibilities:**

  * Move physically through Unity world space along a path defined by the UNet planner
  * Trigger takeoff/arrival logic with origin/destination ATC controllers
  * Optionally: support for collision avoidance, dynamic replanning, or state-based logic

* **Key Methods:**

  * `InitializePath(List<Vector3> path)`: assigns a world-space path from A\* result
  * `Update()`: interpolates drone position along the path

* **Path Source:** UNetController → GraphSearchAlgorithms → lazy-expanded VolumeGraph

### `DroneAgentStatus.cs`

An attached status UI component per drone. Renders a floating label and UI panel showing metadata and current 4D position.

* **Key Features:**

  * Computes nearest `VolumeAgent` at runtime using `VolumeUtils`
  * Displays: flight ID, call sign, altitude, (x, y, z, t), origin, and destination
  * Optional 2D UI integration via the `DroneInfo` panel

---

## Infrastructure Agents

### `UNetController.cs`

This class acts as the UTM system and centralized path planner.

* **Key Responsibilities:**

  * Find a 4D path from origin ATC to destination ATC using A\*
  * Reserve the `VolumeAgent`s along the path
  * Reject or retry failed requests due to conflicts

* **Functionality Highlights:**

  * Lazy graph search — volumes are only created as needed
  * Time-index wrapping — makes limited 4D grid reusable
  * Reservations stored using flight IDs in a dictionary

### `ATCController.cs`

Each vertiport has an ATC component that reports takeoff/landing events.

* **Key Methods:**

  * `NotifyTakeoff(DroneAgent)`
  * `NotifyLanding(DroneAgent)`

This is a placeholder for possible future queueing, landing slot assignment, or per-port UTM logic.

---

## Agent Collaboration

* **SimulationManager** instantiates `DroneAgent`s and schedules their launch over time.
* **UNetController** assigns each one a unique 4D path
* **DroneAgent** moves through that path in physical space, while
* **DroneAgentStatus** and **VolumeVisualizer** track and debug their motion

---

Next: see `Architecture.md` for systemic layering and control flow.
