using System;

namespace ServiceLocator.Event
{
    public class EventService
    {
        // Action to Update Player Health UI - UIController
        public EventController<Action<int>> OnPlayerHealthUIUpdateEvent { get; private set; }

        // Action to Update Player Ammo UI - UIController
        public EventController<Action<int, int>> OnPlayerAmmoUIUpdateEvent { get; private set; }

        // Action to Update Enemy Count UI - UIController
        public EventController<Action<int>> OnEnemyCountUIUpdateEvent { get; private set; }

        // Action to Update Wave Count UI - UIController
        public EventController<Action<string>> OnWaveCountUIUpdateEvent { get; private set; }

        // Action to Update Message UI - UIController
        public EventController<Action<string>> OnMessageUIUpdateEvent { get; private set; }

        public EventService()
        {
            OnPlayerHealthUIUpdateEvent = new EventController<Action<int>>();
            OnPlayerAmmoUIUpdateEvent = new EventController<Action<int, int>>();
            OnEnemyCountUIUpdateEvent = new EventController<Action<int>>();
            OnWaveCountUIUpdateEvent = new EventController<Action<string>>();
            OnMessageUIUpdateEvent = new EventController<Action<string>>();
        }
    }
}