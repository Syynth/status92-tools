using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Status92.Tools.Input
{
    public class S92YAxis : S92SingleAxis<S92YAxis.State>
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
        ) : S92SingleAxis<State>.State(
            HeldTime,
            IsUp,
            IsReleased,
            IsPressed,
            IsHeld,
            IsDown,
            IsAnyUp,
            Value,
            Sign,
            FullTiltSign,
            HalfTilt
        )
        {
            public virtual Vector3 Vector3() => new(0f, Value);
        }

        protected override Type GetValueType() => typeof(Vector2);

        protected override float GetValue(InputAction.CallbackContext context) =>
            context.ReadValue<Vector2>().y;

        public override State GetState() => new(
            HeldTime: HeldTime,
            IsUp: IsUp,
            IsReleased: IsReleased,
            IsPressed: IsPressed,
            IsHeld: IsHeld,
            IsDown: IsDown,
            IsAnyUp: IsAnyUp,
            Value: Value,
            Sign: Math.Sign(Value),
            FullTiltSign: Mathf.Abs(Value) < 0.5f ? 0 : Math.Sign(Value),
            HalfTilt: Mathf.Abs(Value) < 0.5f
        );

        public static S92YAxis Create(InputAction action, S92AxisType axisType, float pressedWindow, float deadZone)
        {
            var axis = new S92YAxis(pressedWindow)
            {
                AxisType = axisType,
                DeadZone = deadZone
            };
            axis.Bind(action);
            return axis;
        }

        private S92YAxis(float pressedWindow) : base(pressedWindow)
        {
        }
    }
}