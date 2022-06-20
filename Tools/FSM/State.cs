using UnityEngine;

namespace Status92.Tools.FSM
{
    public abstract class State<TActor, TState, TMachine> : IState<TActor, TState, TMachine>
        where TActor : IActor<TActor, TState, TMachine>
        where TState : State<TActor, TState, TMachine>
        where TMachine : Machine<TActor, TState, TMachine>
    {
        public virtual TActor Actor { get; }
        public virtual bool CanEnter => true;
        public virtual string Name { get; }
        public virtual TMachine Machine { get; }

        public virtual float ActiveTime =>
            Time.time - StartTime;

        protected float StartTime;
        protected bool IsAnimationFinished;
        protected bool IsExiting;

        public virtual void Enter()
        {
            StartTime = Time.time;
            IsExiting = false;
            IsAnimationFinished = false;
        }

        public virtual void Exit()
        {
        }

        public virtual void Tick(float deltaTime)
        {
        }

        public virtual void Update(float deltaTime)
        {
        }

        public virtual void FixedUpdate(float deltaTime)
        {
        }

        public virtual void AnimationTrigger()
        {
        }

        public void AnimationFinishedTrigger()
        {
            IsAnimationFinished = true;
        }

        public virtual T _<T>() where T : TState
        {
            return Machine.Get<T>();
        }

        protected State(TActor actor, TMachine machine, string name)
        {
            (Actor, Machine, Name) = (actor, machine, name);
        }
    }
}