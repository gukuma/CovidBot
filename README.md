# CovidBot



## What does it do? 
Unity MLAgent social isolation enforcement bot

*Randomly generated environment* to train a unity ML Agent with 360 vision and sensor components to navigate, avoid single wanderers, and break apart groups of wanderers. *Wanderers* will walk aimlessly in the space, unless bumping into another *Wanderer* and then both will stop and become a *Talker*. 
Infinite walking is implemented by turning walls into portals to opposite wall, maintaining all other motion data. 


## Agent Detail 
Rewards: 
```
Nudge 'Talker': AddReward(1)
Nudge 'Wanderer': AddReward(-0.5) 
Make no move: AddReward(-0.1)
```

Observation Space: 
```
Velocity (x, z): 2
Position (x, z): 2
# of infected: 1
Raycast Array: 16 X 3
```

Action Space: 
```
4 Possibilities:
X Direction Movements: 2
Y Direction Movements: 2
```

### Dependencies 
| Unity  | MLAgents | Python |
| ------------- | ------------- | -----------|
|2019.4.13| 0.8 Release  | 3.7  |


 ![](playermode.gif)

### Download Demos


> [Download Mac Demo](https://drive.google.com/drive/my-drive)

> [Download Windows Demo](https://drive.google.com/drive/my-drive)




### Utilities
1. Map Generator
   - Map Size Slider: changes map size (constrained at 3.5f to 10f, ideally suggested at 3f to 5f)
   - Wanderer Density Slider: increases the amount of wanderers in the space (constrained at 0.2f to 2f, ideally suggested at 0.5-1f)
   - Generate Map Button: Press to generate new map
2. Switch from AI mode to Player mode
   - Press in AI mode or Player mode to switch to the other
   - NOTE: AI MODE NEURAL MODEL NOT FULLY TRAINED, UPLOADED "ENFORCER.nn" HAD MINIMAL EPISODES
     
 ![](utilities.gif)




## Backlogs
- LOTS!!

