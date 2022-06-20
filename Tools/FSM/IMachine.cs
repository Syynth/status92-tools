using System;
using UnityEngine;

namespace Status92.Tools.FSM
{
    public interface IMachine<in TActor, TState, in TMachine>
        where TActor : IActor<TActor, TState, TMachine>
        where TState : IState<TActor, TState, TMachine>
        where TMachine : IMachine<TActor, TState, TMachine>
    {
        public TState Previous { get; }
        public TState Current { get; }
        public TState Next { get; }
        public T Get<T>() where T : TState;
        public void Init<T>() where T : TState;
        public void Init(TState initialState);
        public bool Is(TState state);
        public bool Is<T>() where T : TState;
        public bool Has<T>() where T : TState;
        public TState Into(TState nextState);
        public T Into<T>() where T : TState;
        public bool TryInto(TState nextState);
        public bool TryInto<T>() where T : TState;
        public void ForEach(Action<TState> action);
        public void Register(IState<TActor, TState, TMachine> state);
        public bool TryRegister(IState<TActor, TState, TMachine> state);
        public void Disable<T>(float delay = 0.1f) where T : TState;
        public void Update(float deltaTime);
        public void FixedUpdate(float deltaTime);
    }
}