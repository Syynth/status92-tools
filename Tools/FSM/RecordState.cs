using UnityEngine;

namespace Status92.Tools.FSM
{
    public record RecordState<TActor>(TActor Actor, RecordMachine<TActor> Machine, string Name)
        : IState<TActor, RecordState<TActor>, RecordMachine<TActor>>
        where TActor : IActor<TActor, RecordState<TActor>, RecordMachine<TActor>>
    {
        
        public virtual float ActiveTime =>
            Time.time - StartTime;

        private float StartTime;
        protected bool IsAnimationFinished;
        protected bool IsExiting;

        public virtual bool CanEnter => true;
        public TActor Actor { get; } = Actor;
        public RecordMachine<TActor> Machine { get; } = Machine;
        public string Name { get; } = Name;

        public T _<T>() where T : RecordState<TActor>
        {
            return Machine.Get<T>();
        }

        public void Enter()
        {
            StartTime = Time.time;
            IsExiting = false;
            IsAnimationFinished = false;
        }

        public void Exit()
        {
        }

        public void Tick(float deltaTime)
        {
        }

        public void Update(float deltaTime)
        {
        }

        public void FixedUpdate(float deltaTime)
        {
        }

        public void AnimationTrigger()
        {
        }

        public void AnimationFinishedTrigger()
        {
            IsAnimationFinished = true;
        }
    }
}