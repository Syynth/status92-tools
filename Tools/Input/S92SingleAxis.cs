using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Status92.Tools.Input
{
    public enum S92AxisType
    {
        Positive,
        Negative,
        Full,
    }

    public abstract class S92SingleAxis<TState> : S92Input<TState> where TState : S92Input<TState>.State
    {
        public new record State(
            float HeldTime,
            bool IsUp,
            bool IsReleased,
            bool IsPressed,
            bool IsHeld,
            bool IsDown,
            bool IsAnyUp,
            float Value,
            int Sign,
            int FullTiltSign,
            bool HalfTilt
        ) : S92Input<TState>.State(
            HeldTime,
            IsUp,
            IsReleased,
            IsPressed,
            IsHeld,
            IsDown,
            IsAnyUp
        );

        [ShowInInspector] public float Value { get; protected set; }
        protected float DeadZone { get; init; }
        protected S92AxisType AxisType { get; init; }

        public override void Bind(InputAction action = null)
        {
            if (Action is not null) Unbind();
            Action ??= action;
            if (Action is null) return;
            Action.performed += HandleAction;
            Bound = true;
        }

        public override void Unbind()
        {
            if (Action is null) return;
            Action.performed -= HandleAction;
            Bound = false;
        }

        protected abstract Type GetValueType();
        protected abstract float GetValue(InputAction.CallbackContext context);

        protected override void HandleAction(InputAction.CallbackContext context)
        {
            if (context.valueType != GetValueType()) return;

            Value = AxisType switch
            {
                S92AxisType.Positive => Mathf.Max(0f, GetValue(context)),
                S92AxisType.Negative => Mathf.Min(0f, GetValue(context)),
                S92AxisType.Full => GetValue(context),
                _ => throw new ArgumentOutOfRangeException()
            };

            if (Mathf.Abs(Value) > DeadZone)
            {
                Press();
            }
            else
            {
                Release();
            }
        }

        protected S92SingleAxis(float pressedWindow) : base(pressedWindow)
        {
        }
    }
}