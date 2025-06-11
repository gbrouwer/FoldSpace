## FoldSpace: Complete Project Documentation

### Table of Contents

* [Introduction](#introduction)
* [Architecture](#architecture)
* [Design Principles](#design-principles)
* [Installation Guide](#installation-guide)
* [Settings](#settings)
* [Agents](#agents)
* [Scripts Reference](#scripts-reference)
* [Roadmap and Agile Table](#roadmap-and-agile-table)
* [Research References](#research-references)
* [ToDo Items](#todo-items)
* [Updated Handover Instructions](#updated-handover-instructions)

---

### Introduction

FoldSpace is a Unity-based drone simulation framework focused on studying congestion, coordination, and control strategies in a multi-agent urban air mobility context. The current architecture uses a centralized model with ATC, UTM, and DroneAgents operating within defined airspace volumes.

Goals:

* Explore emergent behaviors from multi-agent pathfinding
* Simulate traffic congestion at vertiports
* Visualize 4D (x, y, z, t) volume traversal
* Support handoff between ATC and UTM systems

### Architecture

FoldSpace consists of modular controllers and managers:

* **SimulationManager**: Oversees drone spawning and scenario setup
* **ATCController**: Manages local takeoff/landing at vertiports
* **UNetController**: Computes routes using A\* over VolumeGraph
* **VolumeGraph & VolumeAgent**: Represents 4D space and pathfinding units
* **DroneAgent**: Executes assigned path using stepwise motion
* **DroneAgentStatus**: Adds world-facing floating label for in-flight telemetry

### Design Principles

* **Centralized Routing**: UTM plans full drone paths ahead of launch.
* **Volume Reservation**: VolumeAgent enforces exclusivity per timestep.
* **Visual Grounding**: 3D+time volumes visualized via ReachabilityVisualizer.
* **ATC Handoff**: Origin and destination ATC handle departure and arrival phases.

### Installation Guide

```bash
# Requirements
- Unity 2023.3+ (URP enabled)
- TextMeshPro Essentials installed

# Setup
1. Clone the FoldSpace repo
2. Open the Unity project
3. Assign `dronePrefab`, `centralATC`, `spokeATC`, and `uNetController` in SimulationManager
4. Press Play to simulate traffic from spoke → hub
```

### Settings

* `spawnInterval`: Time between launches (default 2s)
* `numberOfFlights`: Total drones to simulate
* `labelHeight`: Vertical offset for drone label
* `labelScale`: World-space scale for TextMeshPro label

### Agents

* **DroneAgent**: Assigned a full path from UTM. Moves node-to-node via world positions.
* **VolumeAgent**: Represents a point in space and time. Can be reserved.
* **DroneAgentStatus**: Visualizes drone’s ID, call sign, altitude, position, origin, and destination.

### Scripts Reference

#### SimulationManager.cs

**Purpose**: Manages the scenario: instantiates flights, assigns paths, logs completions.

**Dependencies**: `FlightIntent`, `DroneAgent`, `DroneAgentStatus`, `ATCController`, `UNetController`, `VolumeAgent`

**Called By**: Unity (via MonoBehaviour Start/Update)

**Calls**: `Instantiate`, `AssignPath`, `NotifyTakeoff`, `SetFlightInfo`

**Key Methods**:

* `void Start()` Initializes drone parent, begins scenario setup.

* `void InitializeScenario()` Enqueues a set number of FlightIntents from spoke → central.

* `void Update()` Launches one drone every `spawnInterval` seconds.

* `void LaunchDrone(FlightIntent intent)`

  * Gets path from UTM
  * Spawns drone prefab
  * Assigns path to DroneAgent
  * Adds DroneAgentStatus label

**Code Snippet**:

```csharp
GameObject droneGO = Instantiate(dronePrefab, ...);
DroneAgent agent = droneGO.GetComponent<DroneAgent>();
status.SetFlightInfo(intent.flightID, path, path.First(), path.Last(), intent.callSign);
```

---

#### DroneAgentStatus.cs

**Purpose**: Renders world-space label showing telemetry for each drone.

**Set by**: `SimulationManager` via `SetFlightInfo(...)`

**Uses**: TextMeshPro in 3D space. Rotates to face camera.

**Key Properties**: `flightID`, `callSign`, `altitude`, `currentAgent`, `originAgent`, `destinationAgent`

**Key Methods**:

* `SetFlightInfo(...)`: Initializes identity and routing info
* `Update()`: Tracks drone position, updates label text
* `LateUpdate()`: Orients label toward main camera

### Roadmap and Agile Table

| Feature                          | Status         | Points | Notes                           |
| -------------------------------- | -------------- | ------ | ------------------------------- |
| Spoke-to-Central Flight Intent   | ✅ Complete     | 1      | Queue loads from spoke only     |
| UNetController Pathfinding       | ✅ Complete     | 3      | VolumeAgent graph traversal     |
| Volume Reservation Logic         | ✅ Complete     | 2      | Agents lock reserved volumes    |
| Floating Drone Labels            | ✅ Complete     | 2      | TextMeshPro, billboarded        |
| ATC ↔ UTM handoff logic          | 🚧 In Progress | 4      | Requires tracking state changes |
| Descent & Landing Logic          | 🕓 Planned     | 3      | Triggered at final node         |
| Volume Congestion Detection      | 🕓 Planned     | 5      | Visual + logical                |
| Holding Pattern Volumes          | 🕓 Planned     | 5      | Dynamic waiting areas           |
| Curriculum-style Traffic Scaling | 🕓 Future      | 5      | Agent performance under load    |

### Research References

* Sunil, E. (2018). *An Analysis of Decentralized Airspace Structure...*
* Heesbeen, B., NLR. Work on Traffic Manager tool and validation.
* ATMx studies on conflict resolution, intent-based planning, and flexible airspace.

Key Themes:

* Structured vs free-routing tradeoffs
* 4D trajectory planning
* Volume-based airspace reservation
* Centralized UTM architectures

### ToDo Items

*

### Updated Handover Instructions

#### Overview

FoldSpace is a Unity-based simulation of urban drone traffic using rule-based ATC/UTM and centralized path planning. Its goal is to simulate emergent congestion patterns and inter-agent dependencies in 4D airspace.

#### Where We Are

* Centralized model running: spoke-to-central ATC, UTM handoff simulated
* Volume grid visualized and fully reachable from known entry point
* DroneAgent prefab operational, status label implemented, visual debug info working

#### What's Next

* Add descent and touchdown logic to DroneAgent
* Finalize full ATC ↔ UTM handoff coordination
* Begin modeling congestion as queue saturation + volume conflict
* Simulate holding patterns, adaptive re-planning

#### All Code is Modular

* All logic flows through SimulationManager
* Controllers do no rendering — visuals are layered with ReachabilityVisualizer and DroneAgentStatus

#### Rules for Successor

* Maintain clean script boundaries: no logic in prefabs or UI
* Use MonoBehaviours only for scene interaction — keep UTM, Graph, etc. cleanly decoupled
* All visualization should remain optional and separable
* Don’t introduce multi-directional traffic yet unless needed — current goal is hub congestion

---

This document includes all known context, cross-referenced sources, and in-code logic dependencies. Please preserve this structure as simulation complexity increases.
