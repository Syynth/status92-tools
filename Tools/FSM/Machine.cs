using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Status92.Tools.FSM
{
    public abstract class Machine<TActor, TState, TMachine> : IMachine<TActor, TState, TMachine>
        where TActor : IActor<TActor, TState, TMachine>
        where TState : State<TActor, TState, TMachine>
        where TMachine : Machine<TActor, TState, TMachine>
    {
        public TState Previous { get; private set; }
        public TState Current { get; private set; }
        public TState Next { get; private set; }

        private readonly Dictionary<Type, TState> States = new();
        private readonly Dictionary<Type, float> DisabledStates = new();

        public void ForEach(Action<TState> action)
        {
            foreach (var state in States.Values)
            {
                action(state);
            }
        }

        public void Init<T>() where T : TState
        {
            Init(Get<T>());
        }
        public void Init(TState initialState)
        {
            Previous = null;
            Current = initialState;
            Next = null;
        }

        public bool Has<T>() where T : TState
        {
            return States.ContainsKey(typeof(T));
        }

        public bool Is(TState state)
        {
            return Current == state;
        }

        public bool Is<T>() where T : TState
        {
            return Current == Get<T>();
        }

        public T Into<T>() where T : TState
        {
            Into(Get<T>());
            return Get<T>();
        }

        public TState Into(TState nextState)
        {
            if (nextState == Current || nextState is null)
            {
                return nextState;
            }
            if (DisabledStates.ContainsKey(nextState.GetType()))
            {
                return null;
            }
            Next = nextState;
            Debug.Log($"Transiting from {Current?.Name ?? "None"} -> {nextState.Name ?? "None"}");
            Current?.Exit();
            Previous = Current;
            Current = nextState;
            Next = null;
            Current.Enter();
            return nextState;
        }

        public bool TryInto<T>() where T : TState
        {
            return TryInto(Get<T>());
        }

        public bool TryInto(TState nextState)
        {
            if (nextState is null || !nextState.CanEnter)
            {
                return false;
            }

            Into(nextState);
            return true;
        }

        public bool TryRegister(IState<TActor, TState, TMachine> state)
        {
            if (States.ContainsKey(state.GetType())) return false;
            Register(state);
            return true;
        }

        public void Register(IState<TActor, TState, TMachine> state)
        {
            States[state.GetType()] = state as TState;
        }

        public T Get<T>() where T : TState
        {
            return States[typeof(T)] as T;
        }

        public void Update(float deltaTime)
        {
            Current.Update(deltaTime);
            foreach (var state in States.Values)
            {
                state.Tick(deltaTime);
            }

            var copy = new Dictionary<Type, float>();
            foreach (var kvp in DisabledStates)
            {
                copy[kvp.Key] = kvp.Value;
            }

            foreach (var kvp in copy)
            {
                var time = DisabledStates[kvp.Key];
                time -= deltaTime;
                if (time < 0f)
                {
                    DisabledStates.Remove(kvp.Key);
                }
                else
                {
                    DisabledStates[kvp.Key] = time;
                }
            }
        }

        public void FixedUpdate(float deltaTime)
        {
            Current.FixedUpdate(deltaTime);
        }

        public void Disable<T>(float delay = 0.1f) where T : TState
        {
            if (!Has<T>()) return;
            DisabledStates[typeof(T)] = delay;
        }
    }
}