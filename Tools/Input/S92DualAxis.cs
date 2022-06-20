using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Status92.Tools.Input
{
    public class S92DualAxis : S92Input<S92DualAxis.State>
    {
        public new record State(
            float HeldTime,
            bool IsUp,
            bool IsReleased,
            bool IsPressed,
            bool IsHeld,
            bool IsDown,
            bool IsAnyUp,
            Vector2 Value,
            bool HalfTilt
        ) : S92Input<State>.State(
            HeldTime,
            IsUp,
            IsReleased,
            IsPressed,
            IsHeld,
            IsDown,
            IsAnyUp
        );

        public static S92DualAxis Create(InputAction action, float pressedWindow, float deadZone)
        {
            var axis = new S92DualAxis(pressedWindow)
            {
                DeadZone = deadZone
            };
            axis.Bind(action);
            return axis;
        }

        [ShowInInspector] public Vector2 Value { get; private set; }
        private float DeadZone { get; init; }

        public override State GetState() => new(
            HeldTime,
            IsUp,
            IsReleased,
            IsPressed,
            IsHeld,
            IsDown,
            IsAnyUp,
            Value,
            Value.magnitude < 0.5
        );

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

        protected override void HandleAction(InputAction.CallbackContext context)
        {
            if (context.valueType != typeof(Vector2)) return;

            Value = context.ReadValue<Vector2>();
            if (Value.magnitude > DeadZone)
            {
                Press();
            }
            else
            {
                Release();
            }
        }

        private S92DualAxis(float pressedWindow) : base(pressedWindow)
        {
        }
    }
}