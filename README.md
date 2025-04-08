# Rot Zone

## Overview

Rot Zone is a 3D zombie survival shooter, where player fight off waves of zombies using multiple weapon types, each with distinct projectiles. The game features NavMesh-based AI, hand-aligned weapon grips using IK, and head aiming based on weapon aim. The game integrates various design patterns, including **Service Locator**, **Dependency Injection**, **Model-View-Controller (MVC)**, **Observer Pattern**, **Object Pooling**, and **State Machine**, ensuring modularity and scalability. **Scriptable Objects** handle flexible data storage, while **Unity's New Input System** provides precise player controls.

---

## Architectural Overview

Below is the block diagram illustrating the **core architecture**:

![Architectural Overview](docs/block_diagram.png)

---

## Gameplay Elements

### **1. Game States**
| **State**        | **Description**                          |
|------------------|------------------------------------------|
| `GAME_START`     | Initial loading and boot logic.          |
| `GAME_MENU`      | Main menu interface.                     |
| `GAME_CONTROL`   | Displaying control layout (UI only).     |
| `GAME_PLAY`      | Active gameplay session.                 |
| `GAME_PAUSE`     | Pauses game and shows menu.              |
| `GAME_RESTART`   | Resets all systems for a fresh start.    |
| `GAME_OVER`      | Ends current run when player dies.       |

---

### **2. Player States**

#### **Movement States**
| **State** | **Description**              |
|-----------|------------------------------|
| `IDLE`    | Player is not moving.        |
| `WALK`    | Player walks slowly.         |
| `RUN`     | Player sprints.              |
| `FALL`    | Player is falling.           |
| `HURT`    | Player takes damage.         |
| `DEAD`    | Player has died.             |

#### **Action States**
| **State** | **Description**              |
|-----------|------------------------------|
| `NONE`    | No active combat action.     |
| `AIM`     | Aiming with weapon.          |
| `FIRE`    | Shooting the weapon.         |
| `RELOAD`  | Reloading the weapon.        |

---

### **3. Enemy Types**
| **Enemy Type**   | **Description**                   |
|------------------|-----------------------------------|
| `SLOW_ZOMBIE`    | Moves slowly; less aggressive.    |
| `FAST_ZOMBIE`    | Moves fast; more dangerous.       |

---

### **4. Enemy States**
| **Enemy State**   | **Description**                                             |
|-------------------|-------------------------------------------------------------|
| `IDLE`            | Enemy is idle and not performing any action.                |
| `PATROL`          | Enemy is moving between dynamic calculated patrol points.   |
| `DETECT`          | Enemy has detected the player.                              |
| `CHASE`           | Enemy chases the player using NavMesh.                      |
| `ATTACK`          | Enemy is attacking the player when in range.                |
| `HURT`            | Enemy has taken damage and briefly staggers or reacts.      |
| `STUN`            | Enemy is stunned and cannot act temporarily.                |
| `DEAD`            | Enemy is dead and will ragdoll and return to pool.          |

---

### **5. Weapon and Projectile Types**
| **Weapon Type** | **Projectile Type**        |
|------------------|-----------------------------|
| `PISTOL`         | `PISTOL_PROJECTILE`         |
| `RIFLE`          | `RIFLE_PROJECTILE`          |
| `SHOTGUN`        | `SHOTGUN_PROJECTILE`        |

---

### **6. Wave States**
| **Wave State**   | **Description**                                       |
|------------------|-------------------------------------------------------|
| `WAVE_START`     | Begins a new wave, show loading text, game paused.    |
| `WAVE_PROGRESS`  | Wave is active and gameplay is activated.             |
| `WAVE_END`       | All enemies defeated; wave is completed.              |

---

## Design Patterns and Programming Principles

### 1. **Service Locator & Dependency Injection**  
Centralized System for all Service's creation using Soft Service Locator and passed services using method injection for flexibility and maintainability.

### 2. **Model-View-Controller (MVC)**  
Separates concerns for data, visuals, and interactions:
- **Controller**: Coordinates interactions between the model and view.
- **Model**: Handles data and game logic.
- **View**: Manages visuals and rendering.

### 3. **Observer Pattern**  
Enables event-driven communication between game elements like Sound and UI ensuring modular design.

### 4. **Object Pooling**  
Optimizes memory usage for projectiles & enemies.

### 5. **State Machine** 
Manages transitions between states derived from a GenericStateMachine base for different systems like 
**PlayerMovementStateMachine**, **PlayerActionStateMachine**, **EnemyStateMachine**, **GameStateMachine**, **WaveStateMachine**.

### 6. **Scriptable Objects**  
Stores reusable configurations for player, enemies, weapons, projectiles and waves etc.

---

## Services and Components

1. **GameService**: Manages core game setup and initializes the `GameController`.
   - **GameController**: Central controller managing all core systems.
   - **GameStateMachine**: Inherits from `GenericStateMachine`. Governs overall game flow.
     - `GameStartState`, `GameMenuState`, `GameControlState`, `GamePlayState`, `GamePauseState`, `GameRestartState`, `GameOverState`

2. **PlayerService**: Handles player setup, control, weapon handling, aiming, animation, and state transitions.
   - **PlayerConfig**: ScriptableObject containing movement speed, health, and other stats.
   - **PlayerController**: Controls player movement, input handling, weapon use, and animation hooks.
   - **PlayerModel**: Stores runtime values such as health, ammo, and movement data.
   - **PlayerView**: Visual and animation management.
   - **PlayerAnimationController**: Handles animation triggers and IK rig for head and hand alignment.
   - **PlayerWeaponController**: Manages equipped weapon and firing logic.
   - **PlayerStateMachine**:
     - `PlayerMovementStateMachine` and `PlayerActionStateMachine` control independent movement and combat states.
     - Movement: `Idle`, `Walk`, `Run`, `Fall`, `Hurt`, `Dead`
     - Action: `None`, `Aim`, `Fire`, `Reload`

3. **EnemyService**: Controls enemy logic, pooling, animations, and AI behavior through a state machine.
   - **EnemyType**: Enum for enemy Types.
   - **EnemyConfig**: ScriptableObject storing stats like movement speed and detection radius.
   - **EnemyPool**: Reuses enemy instances using `GenericObjectPool`.
   - **EnemyController**: Governs zombie logic — detection, chasing, attacking.
    - **Sub-Controllers**: `FastEnemyController`, `SlowEnemyController`.
   - **EnemyModel**: Stores runtime attributes like speed and health.
   - **EnemyView**: Handles enemy visuals and animations.
   - **EnemyAnimationController**: Triggers animations and controls IK if needed.
   - **EnemyStateMachine**:
     - `EnemyIdleState`, `EnemyPatrolState`, `EnemyDetectState`, `EnemyChaseState`, `EnemyAttackState`, `EnemyHurtState`, `EnemyStunState`, `EnemyDeadState`

4. **WeaponService**: Manages weapon setup, fire types, stats, and different weapon controllers.
   - **WeaponType**: Enum for weapon Types.
   - **WeaponConfig**: ScriptableObject storing weapon stats (e.g., damage, fire rate).
   - **WeaponController**: Base class for switching and firing logic.
    - **Sub-Controllers**: `PistolWeaponController`, `RifleWeaponController`, `ShotgunWeaponController`
   - **WeaponModel**: Holds weapon runtime data.
   - **WeaponView**: Handles weapon visuals and VFX.

5. **ProjectileService**: Fires and manages pooled projectiles for each weapon type.
   - **ProjectileType**: Enum for Projectile Types.
   - **ProjectileConfig**: ScriptableObject for projectile behavior.
   - **ProjectilePool**: Reuses projectile instances using `GenericObjectPool`.
   - **ProjectileController**: Base logic for projectile movement and collision.
    - **Sub-Controllers**: `PistolProjectileController`, `RifleProjectileController`, `ShotgunProjectileController`
   - **ProjectileModel**: Stores projectile speed, damage, etc.
   - **ProjectileView**: Manages visuals, including TrailRenderer.

6. **WaveService**: Governs the wave system, including spawning and transitions between wave phases.
   - **WaveType**: Enum for future extensibility (e.g., boss waves).
   - **WaveConfig**: ScriptableObject defining enemy counts and pacing.
   - **WaveStateMachine**: Manages wave progression via:
     - `WaveStartState`, `WaveProgressState`, `WaveEndState`

7. **SpawnService**: Spawns enemies or other entities as defined by wave or game events.
   - **SpawnEntityType**: Enum classifying spawnable entities.
   - **ISpawn**: Interface for extensible spawning logic.

8. **UIService**: Handles all UI interactions and in-game HUD updates.
   - **UIController**: Manages menu input and HUD logic.
   - **UIView**: Handles visual updates like score, health, and ammo.

9. **SoundService**: Manages background music and SFX, triggered via events.
   - **SoundType**: Enum categorizing sound triggers (e.g., `FIRE`, `RELOAD`)
   - **SoundConfig**: ScriptableObject containing audio clips.

10. **CameraService**: Sets up and manages the player-follow camera and configuration.
    - **CameraConfig**: ScriptableObject storing camera settings.

11. **InputService**: Uses Unity’s **Input System** to manage and process player inputs in a decoupled way.
    - **InputControls**: Auto-generated input mappings.

12. **EventService**: Event-based communication hub for decoupled gameplay elements. Used for UI updates, sound effects, wave triggers, and more.
    - **EventController**: Registers and invokes game events.

13. **Utilities**: Generic tools for scalable, reusable systems.
    - **GenericObjectPool**: Core pooling logic reused across enemies and projectiles.
    - **GenericStateMachine**, `IState`, `IStateOwner`: Framework for all modular state machines in the game.

---

## Development Workflow

| **Branch**                       | **Feature**                                              |
|----------------------------------|----------------------------------------------------------|
| `Feature-0-Project-Setup`        | Initial Unity project structure setup.                   |
| `Feature-1-Character-Setup`      | Player controller, config, animation, and IK setup.      |
| `Feature-2-Weapon-Setup`         | Weapon controllers and fire logic.                       |
| `Feature-3-Dependency-Injection` | Centrailized Services with Dependency Injection system.  |
| `Feature-4-MVC`                  | Applied MVC architecture across core systems.            |
| `Feature-5-State-Machine`        | Integrated Generic State Machines.                       |
| `Feature-6-Projectile-Setup`     | Setup projectile logic and pooling.                      |
| `Feature-7-Object-Pool`          | Generic pooling for enemies and projectiles.             |
| `Feature-8-Enemy-Setup`          | Enemy AI with NavMesh and states.                        |
| `Feature-9-Wave-Level-Setup`     | Wave system and entity spawning.                         |
| `Feature-10-UI-Setup`            | HUD, menus, and dynamic UI updates.                      |
| `Feature-11-Observer-Pattern`    | Implemented event system for decoupling.                 |
| `Feature-12-Sound-Setup`         | Integrated SFX and music using `SoundService`.           |
| `Feature-13-Polish`              | Visual/audio polish and game balance tuning.             |
| `Feature-14-Documentation`       | Created documentation and final project README.          |

---

## Events

| **Event Name**                 | **Description**                                      |
|--------------------------------|------------------------------------------------------|
| `OnPlayerHealthUIUpdateEvent`  | Updates the player health display in the UI.         |
| `OnPlayerAmmoUIUpdateEvent`    | Updates the ammo count in the UI (current / total).  |
| `OnEnemyCountUIUpdateEvent`    | Updates the remaining enemy count in the HUD.        |
| `OnWaveUIUpdateEvent`          | Updates the wave type in the HUD.                    |
| `OnLoadTextUIUpdateEvent`      | Toggles load wave message in the UI.                 |
| `OnPlaySoundEffectEvent`       | Plays a sound effect based on `SoundType`.           |

---

## Script and Asset Hierarchy

1. **Scripts**:
   - **Main**: Core game mechanics and state machine for main game flow.
   - **Player**: Player movement, aiming, shooting, IK setup, animation, and combat state machines.
   - **Enemy**: Enemy AI behavior, pooling, animations, and modular state machine for each behavior type.
   - **Weapon**: Weapon handling, firing logic, configurations, and weapon switching.
   - **Projectile**: Modular projectile system with pooling, controller logic, and different fire types.
   - **Wave**: Governs wave transitions, enemy count tracking, and horde progression.
   - **Spawn**: Spawning logic and entity registration system.
   - **UI**: HUD updates, menu screens, and event-based UI transitions.
   - **Sound**: Manages all sound effect playback and background music using events.
   - **Input**: Decoupled input processing using Unity's new Input System.
   - **Vision**: Camera follow system and configuration logic.
   - **Event**: Event-based communication between systems using Observer Pattern.
   - **Utility**: Shared systems like Generic Object Pooling and Generic State Machines.

2. **Assets**:
   - **Character Models & Animations**: Player and enemy models and animations from Mixamo.
   - **Art & Other Models**: Unity Asset Store.
   - **Prefabs**: Made from models.
   - **Sounds**: Some from Unity Asset Store and others are Royalty-free sound effects.

---

## Setting Up the Project

1. Clone the repository:
   ```bash
   git clone https://github.com/123rishiag/Rot-Zone.git
   ```
2. Open the project in Unity.

---

## Video Demo

[Watch the Gameplay Demo](https://www.loom.com/share/6b9599ffb9b843f9ab881a10d626d5dc?sid=15938040-660c-4a67-9318-2f87f5c4ea48)  
[Watch the Architecture Explanation](https://www.loom.com/share/cadac715ed3844d68c7608c74f70ce1e?sid=25da374c-d0fe-44d6-90bf-7649e0271e4d)

---

## Play Link

[Play the Game](https://123rishiag.github.io/Rot-Zone/)

---