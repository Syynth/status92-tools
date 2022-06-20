using UnityEngine;

namespace Status92.Tools.FSM
{
    public interface IState<out TActor, in TState, out TMachine>
        where TActor : IActor<TActor, TState, TMachine>
        where TState : IState<TActor, TState, TMachine>
        where TMachine : IMachine<TActor, TState, TMachine> 
    {
        public TActor Actor { get; }
        public bool CanEnter { get; }
        public string Name { get; }
        public TMachine Machine { get; }
        public float ActiveTime { get; }

        public T _<T>() where T : TState;

        public void Enter();
        public void Exit();
        public void Tick(float deltaTime);
        public void Update(float deltaTime);
        public void FixedUpdate(float deltaTime);
        public void AnimationTrigger();
        public void AnimationFinishedTrigger();
    }
}