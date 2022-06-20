using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Status92.Tools.Input
{
    public class S92XAxis : S92SingleAxis<S92XAxis.State>
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
            public virtual Vector3 Vector3() => new(Value, 0f);
        }

        protected override Type GetValueType() => typeof(Vector2);

        protected override float GetValue(InputAction.CallbackContext context) =>
            context.ReadValue<Vector2>().x;

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

        public static S92XAxis Create(InputAction action, S92AxisType axisType, float pressedWindow, float deadZone)
        {
            var axis = new S92XAxis(pressedWindow)
            {
                AxisType = axisType,
                DeadZone = deadZone
            };
            axis.Bind(action);
            return axis;
        }

        private S92XAxis(float pressedWindow) : base(pressedWindow)
        {
        }
    }
}