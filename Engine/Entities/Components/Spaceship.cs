using AdAstra.Engine.Managers.Global;
using Microsoft.Xna.Framework.Input;

namespace AdAstra.Engine.Entities.Components
{
    internal class Spaceship : Component
    {
        public float Speed { get; set; } = 100.0f;
        public float TurnSpeed { get; set; } = 5.0f;
        public bool IsKeyboardControlled { get; set; } = true;
        public Keys KeyboardForward { get; set; } = Keys.W;
        public Keys KeyboardLeft { get; set; } = Keys.A;
        public Keys KeyboardRight { get; set; } = Keys.D;

        public override void Update()
        {
            base.Update();

            if (!Entity.HasComponent<Rigidbody>()) return;

            if (IsKeyboardControlled)
            {
                if (InputManager.IsKeyDown(KeyboardForward)) Entity.GetComponent<Rigidbody>().AddForce(Speed);
                if (InputManager.IsKeyDown(KeyboardLeft)) Entity.GetComponent<Rigidbody>().AddTorque(TurnSpeed);
                if (InputManager.IsKeyDown(KeyboardRight)) Entity.GetComponent<Rigidbody>().AddTorque(TurnSpeed);
            }
        }
    }
}
