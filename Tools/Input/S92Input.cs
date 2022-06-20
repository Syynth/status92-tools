using Sirenix.Utilities;
using Status92.Tools.FSM;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Status92.Tools.Input
{
    public interface IS92InputState<TInput, TInputState>
        where TInput : IS92Input<TInput, TInputState>
    {
    }

    public interface IS92Input<TInput, TInputState>
        where TInput : IS92Input<TInput, TInputState>
    {
        void Update(float deltaTime);
        void Bind(InputAction action = null);
        void Unbind();
    }

    public abstract class S92Input<TState> : Actor<S92Input<TState>>,
        IS92Input<S92Input<TState>, S92Input<TState>.State>
    {
        protected S92Input(float pressedWindow)
        {
            PressedWindow = pressedWindow;
            FSM = new RecordMachine<S92Input<TState>>();
            FSM.Register(new Up(this, FSM));
            FSM.Register(new Pressed(this, FSM));
            FSM.Register(new Held(this, FSM));
            FSM.Register(new Released(this, FSM));
            FSM.Init(FSM.Get<Up>());
        }

        private float PressedWindow { get; }
        protected float HeldTime { get; set; }

        protected InputAction Action { get; set; }
        protected bool Bound;

        public abstract TState GetState();

        public virtual void Update(float deltaTime)
        {
            HeldTime += deltaTime;

            if (IsPressed && HeldTime > PressedWindow)
            {
                FSM.Into<Held>();
            }

            if (IsReleased && HeldTime > PressedWindow)
            {
                FSM.Into<Up>();
            }
        }

        protected virtual void Press()
        {
            if (!IsAnyUp || IsHeld) return;
            FSM.Into<Pressed>();
            HeldTime = 0f;
        }

        protected virtual void Release()
        {
            if (!IsHeld) return;
            FSM.Into<Released>();
            HeldTime = 0f;
        }

        public virtual void Bind(InputAction action = null)
        {
            if (Action is not null) Unbind();
            Action ??= action;
            if (Action is null) return;
            Action.started += HandleAction;
            Action.performed += HandleAction;
            Action.canceled += HandleAction;
            Bound = true;
        }

        public virtual void Unbind()
        {
            if (Action is null) return;
            Action.started -= HandleAction;
            Action.performed -= HandleAction;
            Action.canceled -= HandleAction;
            Bound = false;
        }

        protected abstract void HandleAction(InputAction.CallbackContext context);

        protected bool IsUp => FSM.Is<Up>();
        protected bool IsAnyUp => FSM.Is<Up>() || FSM.Is<Released>();
        protected bool IsPressed => FSM.Is<Pressed>();
        public bool IsHeld => FSM.Is<Held>();
        protected bool IsReleased => FSM.Is<Released>();
        protected bool IsDown => FSM.Is<Pressed>() || FSM.Is<Held>();

        private record Up(S92Input<TState> Actor, RecordMachine<S92Input<TState>> Machine)
            : RecordState<S92Input<TState>>(Actor, Machine, "Up");

        private record Pressed(S92Input<TState> Actor, RecordMachine<S92Input<TState>> Machine)
            : RecordState<S92Input<TState>>(Actor, Machine, "Pressed");

        private record Held(S92Input<TState> Actor, RecordMachine<S92Input<TState>> Machine)
            : RecordState<S92Input<TState>>(Actor, Machine, "Held");

        private record Released(S92Input<TState> Actor, RecordMachine<S92Input<TState>> Machine)
            : RecordState<S92Input<TState>>(Actor, Machine, "Released");

        public record State(
            float HeldTime,
            bool IsUp,
            bool IsReleased,
            bool IsPressed,
            bool IsHeld,
            bool IsDown,
            bool IsAnyUp
        ) : IS92InputState<S92Input<TState>, State>;
    }
}