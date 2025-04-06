using Game.Sound;
using Game.Wave;
using System;

namespace Game.Event
{
    public class EventService
    {
        public EventService()
        {
            OnPlayerHealthUIUpdateEvent = new EventController<Action<int>>();
            OnPlayerAmmoUIUpdateEvent = new EventController<Action<int, int>>();
            OnEnemyCountUIUpdateEvent = new EventController<Action<int>>();
            OnWaveUIUpdateEvent = new EventController<Action<WaveType>>();
            OnLoadTextUIUpdateEvent = new EventController<Action<bool>>();
            OnPlaySoundEffectEvent = new EventController<Action<SoundType>>();
        }

        // Event to Update Player Health UI - UIController
        public EventController<Action<int>> OnPlayerHealthUIUpdateEvent { get; private set; }

        // Event to Update Player Ammo UI - UIController
        public EventController<Action<int, int>> OnPlayerAmmoUIUpdateEvent { get; private set; }

        // Event to Update Enemy Count UI - UIController
        public EventController<Action<int>> OnEnemyCountUIUpdateEvent { get; private set; }

        // Event to Update Wave Count UI - UIController
        public EventController<Action<WaveType>> OnWaveUIUpdateEvent { get; private set; }

        // Event to Update Load Message UI - UIController
        public EventController<Action<bool>> OnLoadTextUIUpdateEvent { get; private set; }

        // Event to Play Sound Effect - Sound Service
        public EventController<Action<SoundType>> OnPlaySoundEffectEvent { get; private set; }
    }
}