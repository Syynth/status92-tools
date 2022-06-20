using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Status92.Tools.Input
{
    public class S92Button : S92Input<S92Button.State>
    {
        public new record State(
            float HeldTime,
            bool IsUp,
            bool IsReleased,
            bool IsPressed,
            bool IsHeld,
            bool IsDown,
            bool IsAnyUp
        ) : S92Input<State>.State(
            HeldTime,
            IsUp,
            IsReleased,
            IsPressed,
            IsHeld,
            IsDown,
            IsAnyUp
        );

        public static S92Button Create(InputAction action, float pressedWindow)
        {
            var btn = new S92Button(pressedWindow);
            btn.Bind(action);
            return btn;
        }

        private S92Button(float pressedWindow) : base(pressedWindow)
        {
        }

        protected override void HandleAction(InputAction.CallbackContext context)
        {
            if (context.control is not ButtonControl btn) return;

            if (btn.wasPressedThisFrame)
            {
                Press();
                return;
            }

            if (btn.wasReleasedThisFrame)
            {
                Release();
                return;
            }

            if (btn.isPressed && !IsDown)
            {
                Press();
            }
            else if (!btn.isPressed && IsDown)
            {
                Release();
            }
        }

        public override State GetState() => new(HeldTime, IsUp, IsReleased, IsPressed, IsHeld, IsDown, IsAnyUp);
    }
}