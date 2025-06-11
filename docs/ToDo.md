## ToDo.md

### High Priority

* Implement `DroneAgent.FollowPath()` to traverse assigned VolumeAgent positions
* Link `UNetController.AssignPath()` to actually dispatch a drone
* Add time-indexed step progression to drone (t += 1)
* Add emission gradient along path to visualize trajectory
* Run full simulation cycle: spawn → route → launch → complete → despawn

### Medium Priority

* Add conflict detection and rerouting
* Implement holding patterns if next volume is occupied
* Add reservation expiration / timeout logic
* Make `VolumeEdge` support dynamic costs (e.g., weather, congestion)

### Low Priority / Future

* Integrate ML-Agents for decentralized drone behavior
* Add 3D ATC tower model + light and radar animations
* Build shader that dynamically pulses based on reservation age or priority
* Make volumes split or merge adaptively based on drone density
* Replace primitive visuals with transparent Signed Distance Fields (SDFs)
