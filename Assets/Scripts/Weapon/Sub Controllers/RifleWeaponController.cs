using Game.Event;
using Game.Projectile;
using Game.Sound;
using UnityEngine;

namespace Game.Weapon
{
    public class RifleWeaponController : WeaponController
    {
        public RifleWeaponController(WeaponData _weaponData, Transform _parentPanel,
            EventService _eventService, ProjectileService _projectileService)
            : base(_weaponData, _parentPanel, _eventService, _projectileService)
        {
        }
        public override void PlayFireSound()
        {
            eventService.OnPlaySoundEffectEvent.Invoke(SoundType.FIRE_RIFLE);
        }
    }
}