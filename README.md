# FoldSpace

## Introduction

![Drone Path Visualization](docs/cover.png)

In preparation for this interview, I started this project as a way to understand how multi-agent drone navigation might work in 4D airspace. If I want to learn a new skill, method, or technology, I like designing and building something with it myself — often before diving into the textbooks. I’ve found that by building a system from the ground up, I can get a better sense of its structure, constraints, and possibilities than by starting with theory alone. And it usually makes you appreciate the clever things people have come up with in the textbooks and papers you end up reading afterward even more.

Right now, everything is pretty basic. There’s a centralized controller — called the `UNetController` — that plans routes through a uniform grid of `VolumeAgents`. These VolumeAgents represent small cubes of airspace at specific times. The system builds a global path for each drone from a spoke vertiport to a central hub, making sure the paths don’t intersect in time or space. Drones then just follow those paths step by step. They’re not really autonomous. It’s all centrally planned.

There’s also some tooling: a few pathfinding algorithms (BFS, Dijkstra, A*), a simulation manager that spawns the drones and vertiports, and a basic visualizer that can show reachability from a given volume. It’s just enough to make the environment work — and to let me start playing around with drone AI in a multi-agent setting. Specifically, I wanted to explore whether reinforcement learning could provide enough agent intelligence for the drones to self-organize to:
- avoid collisions,  
- deal with congestion,  
- and develop optimal holding patterns.

I spent some time reading up on swarm intelligence, the BOID flocking model, and multi-agent optimization. But I realized that the real-world drones I wanted to model didn’t necessarily meet the assumptions those methods rely on. Unlike starlings in a murmuration, commercial drones aren’t going to be near-identical copies of one another. That creates a very different challenge — one that regulatory bodies will have to contend with. The airspace will be populated by drones from different manufacturers, possibly running different operating systems and software, and using different AI models. They might rely on different parameter settings, thresholds, and interpretations when sensing or signaling their environment. They might not even be using the same measurement system.  

> (American Drone: “You are within 2 school buses, a medium-sized minivan, and a Honus Wagner baseball card in mint condition of me.”  
> European Drone: “I am what??”)

So, expecting drones to coordinate directly in dense or contested airspace seems hard to scale — and even harder to regulate. Even if they come with flagship NVIDIA GPUs.

That’s what led me to explore a different idea: what if the drones weren’t the smart ones — what if the **airspace** was?

Part of the inspiration for this came from a strange and brilliant organism: the slime mold Dictyostelium discoideum. When food is plentiful, it lives as a scattered population of single-celled amoebae. But when resources run out, tens of thousands of those independent cells begin signaling to one another, eventually aggregating into a kind of mobile super-organism — a “slug” — that moves together to find a better environment. Once there, the slug transforms again, this time into a fruiting body where some cells sacrifice themselves to form the stalk, while others become spores. It’s a powerful example of how local signaling and simple rules can give rise to coordinated, adaptive structure — and it raises interesting questions about when a group becomes an individual. That idea — that intelligent behavior and structure can emerge not from a single brain, but from local interaction between uniform parts — is part of what motivated me to think of the airspace itself as active and self-organizing. It’s not a perfect analogy, but the behavior of VolumeAgents in Smart Airspace feels like it’s playing in the same conceptual space.

So, instead of treating the `VolumeAgents` as passive cells in a planning graph, this concept gives them agency. Each `VolumeAgent` would monitor its local traffic conditions, share pressure or congestion signals with its direct neighbors in both space and time, and influence routing decisions accordingly. Drones would remain simple, but the space they move through would actively shape behavior — much like natural systems where local signals propagate to produce global order.

To take this further, one possibility is to let the airspace **adapt its own resolution**. In high-traffic areas, `VolumeAgents` could subdivide into smaller child volumes — increasing spatial and temporal precision where needed. In quiet zones, they could merge into coarser representations to simplify computation. This dynamic grid would respond to traffic density and flow conditions, reallocating precision where it matters most. It’s inspired by biological and distributed systems, like slime molds and cellular automata, where local rules about structure and communication can produce resilient, adaptive patterns over time.

Critically, this introduces a shift in how the system needs to be reasoned about. By allowing `VolumeAgents` to monitor local traffic, share congestion signals with their neighbors, and act on those signals, we’re no longer working with a centralized control system. We’re building a **complex system** — one in which global behavior emerges from a network of simple, locally interacting parts.

This behavior is diffusion-like in nature: traffic information can spread outward from congested areas, subtly shaping the flow of drones before a problem occurs. If properly tuned, this mechanism might allow the system to self-regulate — without requiring full global awareness or high-level AI planning. Even basic intelligence at the level of individual `VolumeAgents` may be sufficient to generate useful global patterns: adaptive routing, soft rerouting under load, and even spontaneous holding patterns could emerge from a system designed around communication and pressure, rather than control.

---

## 🔗 Table of Contents

* [🛤️ Roadmap](docs/Roadmap.md) – what's built, what's building, what's next
* [🧱 Architecture](docs/Architecture.md) – system components and responsibilities
* [🧠 Design Principles](docs/Design.md) – high-level thinking and constraints
* [🤖 Agents](docs/Agents.md) – drone, ATC, UTM, and volume agent roles
* [📜 Scripts](docs/Scripts.md) – purpose-driven summaries of every script
* [⚙️ Settings](docs/Settings.md) – simulation tuning parameters
* [📋 To Do](docs/ToDo.md) – prioritized backlog of open items

---

## 🚀 Quick Start

1. Clone the repository
2. Open the Unity project (Unity 6000.1.15f, Universal Rendering Pipeline)
3. Open `SimulationManager.cs` in the scene
4. Play to simulate drone scheduling and 4D path assignment
