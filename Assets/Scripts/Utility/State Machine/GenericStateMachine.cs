using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Utility
{
    public abstract class GenericStateMachine<U, T> where U : Enum where T : class
    {
        private T Owner;
        private IState<T> CurrentState;
        private Dictionary<U, IState<T>> States = new Dictionary<U, IState<T>>();

        public GenericStateMachine(T _owner)
        {
            Owner = _owner;
            CreateStates();
            SetOwner();
        }

        public void Update() => CurrentState.Update();
        public void FixedUpdate() => CurrentState?.FixedUpdate();
        public void LateUpdate() => CurrentState?.LateUpdate();

        private void ChangeState(IState<T> _newState)
        {
            CurrentState?.OnStateExit();
            CurrentState = _newState;
            CurrentState?.OnStateEnter();
        }

        public void ChangeState(U _newStateEnum)
        {
            if (States.ContainsKey(_newStateEnum))
            {
                ChangeState(States[_newStateEnum]);
            }
        }

        protected abstract void CreateStates();

        protected void AddState(U _stateEnum, IState<T> _state)
        {
            States.Add(_stateEnum, _state);
        }

        private void SetOwner()
        {
            foreach (IState<T> _state in States.Values)
            {
                _state.Owner = Owner;
            }
        }

        public U GetCurrentState() => States.Keys.FirstOrDefault(key => States[key] == CurrentState);
    }
}