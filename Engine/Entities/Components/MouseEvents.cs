using AdAstra.Engine.Managers.Global;
using Microsoft.Xna.Framework;

namespace AdAstra.Engine.Entities.Components
{
    internal class MouseEvents : Component
    {
        public delegate void MouseEventHandler();

        public event MouseEventHandler OnMouseEnter;
        public event MouseEventHandler OnMouseStayOnce;
        public event MouseEventHandler OnMouseStayRepeatDelay;
        public event MouseEventHandler OnMouseStayConstant;
        public event MouseEventHandler OnMouseStayConstantDelay;
        public event MouseEventHandler OnMouseLeave;
        public event MouseEventHandler OnMouseLeftClick;
        public event MouseEventHandler OnMouseRightClick;
        public event MouseEventHandler OnMouseMiddleClick;

        public float Delay { get; set; } = 300.0f;

        private bool _isMouseOver;
        private bool _isMouseStaying = false;
        private bool _hasMouseStayed = false;
        private float _elapsedTime = 0.0f;

        public override void Update()
        {
            base.Update();

            if (!Entity.HasComponent<ImageRenderer>()) return;

            ImageRenderer imageRenderer = Entity.GetComponent<ImageRenderer>();
            Vector2 mousePosition = SceneManager.Current.Camera.ScreenToWorld(InputManager.MousePosition);

            if (imageRenderer.IsVisible && imageRenderer.Bounds.Contains(mousePosition))
            {
                if (!_isMouseOver)
                {
                    _isMouseOver = true;
                    OnMouseEnter?.Invoke();
                    _elapsedTime = 0.0f;
                }
                else
                {
                    _elapsedTime += Time.DeltaMS;
                    if (_elapsedTime >= Delay)
                    {
                        if (!_hasMouseStayed) _hasMouseStayed = true;
                        if (_hasMouseStayed) OnMouseStayConstantDelay?.Invoke();
                        if (!_isMouseStaying)
                        {
                            _isMouseStaying = true;
                            OnMouseStayOnce?.Invoke();
                        }
                        else
                        {
                            _elapsedTime = 0.0f;
                        }
                        OnMouseStayRepeatDelay?.Invoke();
                    }
                    else
                    {
                        _isMouseStaying = false;
                        OnMouseStayConstant?.Invoke();
                    }

                    if (InputManager.IsMouseButtonPressed(MouseButton.Left)) OnMouseLeftClick?.Invoke();
                    if (InputManager.IsMouseButtonPressed(MouseButton.Right)) OnMouseRightClick?.Invoke();
                    if (InputManager.IsMouseButtonPressed(MouseButton.Middle)) OnMouseMiddleClick?.Invoke();
                }
            }
            else
            {
                if (_isMouseOver)
                {
                    _isMouseOver = false;
                    OnMouseLeave?.Invoke();
                    _elapsedTime = 0.0f;
                }
            }
        }
    }
}
