# TankTraining
[TankTraining](https://unity3d.com/learn/tutorials/projects/tanks-tutorial) is a tutorial from Unity that I finished. After that I started to enhance the project with some new features.

## New features
By now, at the master branch three game features were added. One was the GuidedShell which allow the player to shoot guided shell and the other to send a bomber to drop bombs on the enemy. At last, a machine gun was added to the tank.

For the managing the game, a User Interface was added to permit the user to buy shells, guided shells and planes according to the hits each player made previously.

### GuidesShell
Basically, I took the Shell prefab and changed the material to Red. After that I created and added to this new prefab the script GuidedShellScript. This script is responsible for moving the shell toward the enemy.

### MashinGun
For the bullet I downloaded the [Ammunition Pack](https://assetstore.unity.com/packages/3d/ammunition-pack-demo-82208) and created the prefab MachinGunBullet. To manage the bullet behavior and damage it make to the tank, the MachinGunScript was created.

### Plane
From the [BrainCloudBombers](https://assetstore.unity.com/packages/templates/tutorials/braincloud-bombers-for-unet-50656) asset I took the bomber asset and created two different Bomber prefabs and two different bomb prefabs. For the plane, the BomberMovement script and BomberShooting script were created. The first script being responsible for moving the plane in the game and the second to drop the bombs on the enemy.

### User Interface
Three canvases were added to the MessageCanvas. The InfoCanvas is responsible for showing the game status, like player ammo and score. MenuCanvas as the main menu of the game, where the players can buy ammo, start or quit the game. And for last, the AmmoCanvas is where the players can buy ammo for the game.
