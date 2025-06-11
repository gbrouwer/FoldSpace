# Roadmap.md — FoldSpace Project Roadmap

This roadmap outlines past milestones, current features, and planned future extensions for the FoldSpace drone simulation platform.

---

## ✅ Completed Milestones

### Core Architecture

* Centralized UTM-style planning (`UNetController`) established
* Discrete 4D grid structure defined via `VolumeGraph` and `VolumeAgent`
* Lazy graph instantiation implemented
* Cyclic time axis with wrapping logic

### Agent Logic

* `DroneAgent` movement through A\*-generated paths
* `DroneAgentStatus` live floating label + UI panel
* Reservation and conflict avoidance system using flight IDs

### Visualization and Debug Tools

* `VolumeVisualizer` for path rendering
* Real-time display of (x, y, z, t) and altitude per drone
* Terrain-aware volume placement (excludes low volumes)

### System Integration

* Launch scheduling controlled by `SimulationManager`
* Terrain sampling integrated to restrict invalid placements
* Runtime parameters exposed via Inspector (e.g., grid size, drone speed)

---

## 🚧 Current Work (June 2025)

* **Refinement of Temporal-Spatial Planning:**

  * Ensure only `t+1` temporal neighbors are evaluated
  * Balance drone launch timing with available time resolution

* **Camera Switching System:**

  * Automatic camera follows first drone
  * Manual camera cycling via keyboard

* **Realistic Terrain:**

  * Add elevation variations
  * Verify terrain samples match Unity’s visual and physics layers

* **Debugging**

  * Ensure correct assignment of wrapped time during flight intent scheduling
  * Fix issues where no path is found due to terrain misalignment

---

## 🔮 Planned Features

### 1. Emergent Multi-Agent Behavior

* Introduce ML-Agents (e.g., PPO) for decision-making
* Train drones to react to dynamic obstacles or alternate paths

### 2. ATC Capacity and Queuing

* Spoke and hub ATCs will maintain queue and state
* Arrival/departure slot control

### 3. Volume Priority and Conflict Resolution

* Soft vs. hard reservations
* Time-window-based negotiation or rerouting

### 4. Decentralized UTM

* Replace centralized `UNetController` with per-ATC negotiation
* Allow route overlap with probabilistic path selection

### 5. Data Logging and Replay

* Record simulation metadata for each drone (path, conflicts, re-routes)
* Replay interface with time slider

### 6. Volume-Centric Intelligence

* Explore transferring local logic into `VolumeAgent` instances
* Volumes act as semi-autonomous units evaluating access requests
* Drones negotiate or respond to local capacity rules, priorities, or dynamic congestion

---

## Long-Term Vision

FoldSpace aims to simulate the edge between centralized order and emergent complexity. By blending deterministic infrastructure with ML-driven agents, the platform explores how scale, timing, and local reasoning affect airspace coordination.

Possible future applications include:

* Testing UAM/UTM architectures
* Reinforcement learning in dynamic multi-agent environments
* Cognitive modeling of drone decision-making
* Visualizing spatiotemporal flows in constrained environments
