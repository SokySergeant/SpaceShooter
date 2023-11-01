# Space Shooter
 
[See code before optimization](https://github.com/SokySergeant/SpaceShooter/releases/tag/BeforeOptimizationTag)

[See code after optimization](https://github.com/SokySergeant/SpaceShooter/releases/tag/AfterOptimizationTag)

[See standalone executable](https://github.com/SokySergeant/SpaceShooter/releases/tag/Executable)

# Before optimization
The game started out with a simple player movement and shooting, a healthpack spawner, and an enemy spawner that spawns enemies every few seconds. The projectiles the player shot had a script attached that would move them a tiny bit forward every frame. Enemies worked similarly, each having a script that moved them towards the player. Every frame, the enemy also would check if its box collider is overlapping with anything, and if it is, it would check if its the player and damage them. (The game also has a godmode for stress testing purposes.) Here are some snapshots of how the game ran at this point, as seen in the profiler: (Keep in mind this was done on a 7 old computer)
- 1000 enemies: 42 fps
- 1500 enemies: 30 fps
- 3500 enemies: 6 fps
- 10000 enemies: 1 fps
- 15000 enemies: 0.2 fps

As you can see, this did not run very well. The game quickly slowed to a crawl when increasing the amount of enemies alive at any time. Looking in the profiler, it turned out Physics2D was by far taking the most amount of CPU usage. The lowest percentage of CPU usage I saw it take was 55%, at a very low amount of enemies. Around 3500 enemies, Physics2D would take 72% of CPU usage.

I also noticed that, in the case that a lot of enemies end up overlapping each other, for example when the player does loops around a group of them and they end up bunched together, performance would take a HUGE hit, lagging to the point of the game being unplayable even at 1000 enemies. This is, I assumed, also because of the way collision was handled. Every enemy in that cluster had to parse through every other enemy that was overlapping it and check if it's the player.

Another thing I noticed in the profiler was that, no matter what enemy amount I had, the GarbageCollector's CPU usage was extremely small, which is not what I had expected. At higher number of enemies, I thought it would create tall lag spikes every few seconds with the amount of enemies being deleted every frame, but I didn't see this behavior at all.

I also took screenshots using the Memory Profiler. At 3500 enemies it was using 2.7 GB of memory, and 4.5 GB at 10000 enemies. Most of the memory was taken up by RenderTexture and Texture2D (75% together), and everything else was negligible (2% or less per thing).

# After optimization
Now the optimization process started. Since the physics seemed to be my biggest issue, I started there. I heard that sphere / circle colliders are more performant than cube / box colliders. I switched my enemies to use a circle collider instead of the box one, but this didn't seem to make much of a difference at all. My second option was to make my own collision system in DOTS. This was done in a SystemBase which iterates over all enemies and checks their distance to the player. If the distance is smaller than the enemy's hitbox size, the enemy is destroyed and the player is damaged. This ran much better and also had no issues when lots of enemies overlapped, since they no longer collide with eachother. [See code here.](https://github.com/SokySergeant/SpaceShooter/blob/main/Assets/Scripts/Enemy/EnemyCollisionSystemBase.cs)

Next I made a system for moving enemies, which uses a job with burst. [See here.](https://github.com/SokySergeant/SpaceShooter/blob/main/Assets/Scripts/Enemy/EnemyMovingSystem.cs) This job runs a function inside of an enemy aspect which moves it towards its target location. The target location is updated to the player's position in a different system. [See here.](https://github.com/SokySergeant/SpaceShooter/blob/main/Assets/Scripts/Enemy/EnemySetTargetSystemBase.cs) I split them up this way because, the more split up the systems are, the free-er they are to run side by side in different threads and thus make the code run faster overall. 

The enemy spawner was also remade in DOTS, since it needs to spawn entities. It derives from SystemBase and no longer uses a coroutine for timers, and instead uses a custom one. It increments a time variable by deltaTime every frame and spawns enemies if that value goes above the given time limit. [See code here.](https://github.com/SokySergeant/SpaceShooter/blob/main/Assets/Scripts/EnemySpawner/EnemySpawnerSystemBase.cs)

Projectiles got a similar treatment to the enemies. They have [a moving system which increments their position forward using a job with burst](https://github.com/SokySergeant/SpaceShooter/blob/main/Assets/Scripts/Projectile/ProjectileMovingSystem.cs), and a [custom collision system that checks the distance between the projectiles and enemies](https://github.com/SokySergeant/SpaceShooter/blob/main/Assets/Scripts/Projectile/ProjectileCollisionSystemBase.cs), and destroys the enemy if the distance is smaller than their hitbox size. I also added a range system to the projectiles. Originally, the projectiles would just move forward infinitely, even when they went off screen. This impacted performance quite a bit if the player had shot a lot of them. Therefore, I added a system which iterates through all projectiles and checks if they've travelled futher than their given range (which is right outside the game's view), and if so, destroys them. [See here. ](https://github.com/SokySergeant/SpaceShooter/blob/main/Assets/Scripts/Projectile/ProjectileRangeSystemBase.cs)

Since projectiles are entities, I also needed a projectile spawning system entity that could instantiate them whenever the player needed. This was done in a script deriving from SystemBase that has a method called SpawnProjectile. This method is added to a delegate on the player that gets called whenever the player wants to shoot. [See system here.](https://github.com/SokySergeant/SpaceShooter/blob/main/Assets/Scripts/Projectile/ProjectileSpawnerSystemBase.cs)
The player itself remained basically identical, since it's a single object and moving it to DOTS wouldn't make much of a change.

This is how the game runs now according to the profiler:
- 1000 entities: 150 fps
- 1500 entities: 110 fps
- 3500 entities: 60 fps
- 10000 entities: 30 fps
- 15000 entities: 17 fps

This was a definite improvement! CPU usage was also better, with the biggest chunk being taken by Other (30%), then Rendering (26%). All of my custom systems took about 10% CPU, of which 7% was the collision systems. The Memory Profiler looked pretty similar to before optimization, using 2.65 GB memory at 3500 entities and 4.48 GB memory at 10000 entities. Most of it was again taken by RenderTexture and Texture2D (71% together), 8% taken by Shader, and the rest spread throughout less than 1%ers.

# Some more things I could've done but either didn't have time or didn't figure it out:
Seeing as rendering the sprites was what took the most memory, I would've liked to look into how to make that more performant. One thing I would've done is change the enemies' sprites to a lower def one. The current sprite they use was made to be the same size as the player, but I have since made enemies much smaller to the point where they are a single red dot on the screen, and they don't really need a sprite as detailed as the player's for that. I would also have liked to figure out how to pool enemies and projectiles to maybe improve a little on performance, but since the GC was already taking so very little of it, I can't imagine it would help too much. I would also have liked to figure out how you're supposed to work with DOTS and multiple scenes. Originally, I had a seperate scene for the main menu and one for the game itself, but I had to instead move them into the same scene. This was because DOTS systems will run regardless of which scene you're in, so they would try to run in the main menu and crash the app. 
