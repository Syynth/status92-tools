using System;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

namespace Status92.Tools.FSM
{
    
    [Serializable]
    public class RecordMachine<TActor> : IMachine<TActor, RecordState<TActor>, RecordMachine<TActor>>
        where TActor : IActor<TActor, RecordState<TActor>, RecordMachine<TActor>>
    {

        public string CurrentState = "";
        public RecordState<TActor> Previous { get; private set; }
        public RecordState<TActor> Current { get; private set; }
        public RecordState<TActor> Next { get; private set; }
        
        private readonly Dictionary<Type, dynamic> States = new();
        private readonly Dictionary<Type, float> DisabledStates = new();
        
        public T Get<T>() where T : RecordState<TActor>
        {
            return States[typeof(T)] as T;
        }

        public void Init<T>() where T : RecordState<TActor>
        {
            Init(Get<T>());
        }

        public void Init(RecordState<TActor> initialState)
        {
            Current = initialState;
            CurrentState = initialState.Name;
        }

        public bool Is(RecordState<TActor> state)
        {
            return Current == state;
        }

        public bool Is<T>() where T : RecordState<TActor>
        {
            return Current == Get<T>();
        }
        
        public bool Has<T>() where T : RecordState<TActor>
        {
            return States.ContainsKey(typeof(T));
        }

        public RecordState<TActor> Into(RecordState<TActor> nextState)
        {
            if (nextState == Current) return nextState;
            Next = nextState;
            Current?.Exit();
            Previous = Current;
            Current = nextState;
            Next = null;
            Current.Enter();
            CurrentState = Current.Name;
            return nextState;
        }

        public T Into<T>() where T : RecordState<TActor>
        {
            Into(Get<T>());
            return Get<T>();
        }

        public bool TryInto(RecordState<TActor> nextState)
        {
            if (!nextState.CanEnter) return false;
            
            Into(nextState);
            return true;
        }

        public bool TryInto<T>() where T : RecordState<TActor>
        {
            return TryInto(Get<T>());
        }

        public void ForEach(Action<RecordState<TActor>> action)
        {
            foreach (var state in States.Values)
            {
                action(state);
            }
        }

        public void Register(IState<TActor, RecordState<TActor>, RecordMachine<TActor>> state)
        {
            States[state.GetType()] = state;
        }

        public bool TryRegister(IState<TActor, RecordState<TActor>, RecordMachine<TActor>> state)
        {
            if (States.ContainsKey(state.GetType())) return false;
            Register(state);
            return true;
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

        public void Disable<T>(float delay = 0.1f) where T : RecordState<TActor>
        {
            if (!Has<T>()) return;
            DisabledStates[typeof(T)] = delay;
        }
    }
}