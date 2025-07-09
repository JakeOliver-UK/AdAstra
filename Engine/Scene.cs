using AdAstra.Engine.Entities;
using AdAstra.Engine.Entities.Components;
using AdAstra.Engine.Managers.Instance;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AdAstra.Engine
{
    internal abstract class Scene : IDisposable
    {
        public Color BackgroundColor { get; set; } = Color.CornflowerBlue;
        public EntityManager WorldEntityManager => _worldEntityManager;
        public EntityManager OverlayEntityManager => _overlayEntityManager;
        public Camera Camera=> _camera;

        private EntityManager _worldEntityManager;
        private EntityManager _overlayEntityManager;
        private Camera _camera;

        public virtual void Initialize()
        {
            _worldEntityManager = new(false);
            _overlayEntityManager = new(true);
            Entity camera = _worldEntityManager.Create("Camera");
            _camera = camera.AddComponent<Camera>();
        }
        
        public virtual void Update()
        {
            _worldEntityManager.Update();
            _overlayEntityManager.Update();
        }
        
        public virtual void Draw()
        {
            App.Instance.GraphicsDevice.Clear(BackgroundColor);

            App.Instance.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null, _camera.TransformMatrix);
            DrawWorld();
            App.Instance.SpriteBatch.End();

            App.Instance.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null, null);
            DrawOverlay();
            App.Instance.SpriteBatch.End();
        }

        public virtual void DrawWorld()
        {
            _worldEntityManager.Draw();
        }

        public virtual void DrawOverlay()
        {
            _overlayEntityManager.Draw();
        }

        public virtual void Dispose()
        {
            _camera.Entity.Dispose();
            _camera = null;
            _worldEntityManager.Dispose();
            _worldEntityManager = null;
            _overlayEntityManager.Dispose();
            _overlayEntityManager = null;
        }
    }
}
