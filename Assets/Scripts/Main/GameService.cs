using Game.Enemy;
using Game.Player;
using Game.Projectile;
using Game.Sound;
using Game.UI;
using Game.Utility;
using Game.Vision;
using Game.Wave;
using Game.Weapon;
using Unity.Cinemachine;
using UnityEngine;

namespace Game.Main
{
    public class GameService : MonoBehaviour
    {
        [Header("Camera Variables")]
        public CameraConfig cameraConfig;
        public CinemachineStateDrivenCamera cmCamera;

        [Header("Sound Variables")]
        public SoundConfig soundConfig;
        public AudioSource bgmSource;
        public AudioSource sfxSource;

        [Header("UI Variables")]
        public UIView uiCanvas;

        [Header("Other Variables")]
        public MiscService miscPanel;

        [Header("Pool Panels")]
        public Transform projectilePoolPanel;
        public Transform enemyPoolPanel;

        [Header("Spawn Transform")]
        public Transform spawnTransformPanel;

        [Header("Game Variables")]
        public ProjectileConfig projectileConfig;
        public WeaponConfig weaponConfig;
        public PlayerConfig playerConfig;
        public EnemyConfig enemyConfig;
        public WaveConfig waveConfig;

        // Private Variables
        private GameController gameController;

        private void Start() => gameController = new GameController(this);

        private void OnDestroy() => gameController.Destroy();

        private void Update() => gameController.Update();

        private void LateUpdate() => gameController.LateUpdate();
    }
}