using AdAstra.Engine.Extensions;
using AdAstra.Engine.Managers.Global;
using AdAstra.Engine.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdAstra.Engine.Entities.Components
{
    internal class Spaceship : Component
    {
        public string Name { get; set; } = "Spaceship";
        public Color Color { get; set; } = Color.White;
        public double Money { get; set; } = 1000.0d;
        public SpaceshipController Controller { get; set; } = SpaceshipController.None;
        public SpaceshipState State { get; set; } = SpaceshipState.Idle;
        public Entity DockedAt { get; set; }
        public float Speed { get; set; } = 100.0f;
        public float TurnSpeed { get; set; } = 1.0f;
        public float ArrivalThreshold { get; set; } = 5.0f;
        public Keys KeyboardForward { get; set; } = Keys.W;
        public Keys KeyboardLeft { get; set; } = Keys.A;
        public Keys KeyboardRight { get; set; } = Keys.D;
        public Vector2 Target { get; set; } = Vector2.Zero;
        public List<Vector2> NextTargets { get; set; } = [];
        public Entity TradeLocation { get; set; } = null;
        public Commodity TradeCommodity { get; set; } = Commodity.None;
        public int TradeAmount { get; set; } = 0;
        public bool DrawTargetLine { get; set; } = true;
        public float TargetLineOpacity { get; set; } = 0.65f;
        public float TargetLineThickness { get; set; } = 1.0f;
        public List<CommodityCargoItem> Cargo { get; set; } = [];

        private Vector2 _velocity = Vector2.Zero;
        private float _timer = 0.0f;
        private float _idleTime = 5.0f;

        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            
            SetupCargo();
            if (Controller == SpaceshipController.Player) Entity.Tags.Add("Player");
            else if (Controller == SpaceshipController.AI) Entity.Tags.Add("AI");
        }

        public override void Update()
        {
            base.Update();

            if (Controller == SpaceshipController.Player) HandlePlayer();
            else if (Controller == SpaceshipController.AI) HandleAI();

            HandleMovement();
        }

        private void SetupCargo()
        {
            Cargo.Clear();
            Commodity[] commodities = (Commodity[])Enum.GetValues(typeof(Commodity));
            for (int i = 0; i < commodities.Length; i++)
            {
                Commodity commodity = commodities[i];
                if (commodity == Commodity.None) continue;
                int amount = 0;
                Cargo.Add(new CommodityCargoItem(commodity, amount));
            }
        }

        public string GetCargoInfo()
        {
            if (Cargo.Count == 0) return "No cargo available.";
            string info = "Cargo:/n";
            for (int i = 0; i < Cargo.Count; i++)
            {
                CommodityCargoItem item = Cargo[i];
                info += $"{item.Commodity} - Amount: {item.Amount}/n";
            }
            return info;
        }

        private void HandlePlayer()
        {
            Vector2 mousePosition = SceneManager.Current.Camera.ScreenToWorld(InputManager.MousePosition);

            if (InputManager.IsKeyUp(Keys.LeftShift) && InputManager.IsMouseButtonPressed(MouseButton.Right))
            {
                NextTargets.Clear();
                if (InputManager.MousePosition.X < 0 || InputManager.MousePosition.Y < 0 || InputManager.MousePosition.X > App.Instance.GraphicsDevice.Viewport.Width || InputManager.MousePosition.Y > App.Instance.GraphicsDevice.Viewport.Height) return;
                if (Target != Vector2.Zero || _velocity != Vector2.Zero) _velocity *= 0.5f;
                Target = mousePosition;
            }
            else if (InputManager.IsKeyDown(Keys.LeftShift) && InputManager.IsMouseButtonPressed(MouseButton.Right))
            {
                if (InputManager.MousePosition.X < 0 || InputManager.MousePosition.Y < 0 || InputManager.MousePosition.X > App.Instance.GraphicsDevice.Viewport.Width || InputManager.MousePosition.Y > App.Instance.GraphicsDevice.Viewport.Height) return;
                if (Target == Vector2.Zero) Target = mousePosition;
                else NextTargets.Add(mousePosition);
            }
        }

        private void HandleAI()
        {
            switch (State)
            {
                case SpaceshipState.Idle:
                    _timer += Time.Delta;
                    if (_timer >= _idleTime)
                    {
                        _timer = 0.0f;
                        State = SpaceshipState.LookingToBuy;
                    }
                    break;
                case SpaceshipState.LookingToBuy:
                    if (LookToBuy()) State = SpaceshipState.MovingToBuy;
                    else State = SpaceshipState.Idle;
                    break;
                case SpaceshipState.MovingToBuy:
                    if (Target == Vector2.Zero)
                    {
                        State = SpaceshipState.CheckingBuy;
                        DockedAt = TradeLocation;
                        TradeLocation = null;
                    }
                    break;
                case SpaceshipState.CheckingBuy:
                    if (CheckToBuy()) State = SpaceshipState.Buying;
                    else State = SpaceshipState.LookingToBuy;
                    break;
                case SpaceshipState.Buying:
                    Buy();
                    State = SpaceshipState.Idle;
                    break;
                case SpaceshipState.LookingToSell:
                    break;
                case SpaceshipState.MovingToSell:
                    break;
                case SpaceshipState.CheckingSell:
                    break;
                case SpaceshipState.Selling:
                    break;
            }
        }

        private bool LookToBuy()
        {
            Entity[] spaceStations = Entity.Manager.GetWithComponent<SpaceStation>();
            List<MarketItem> marketItems = [];

            if (spaceStations != null && spaceStations.Length > 0)
            {
                for (int i = 0; i < spaceStations.Length; i++)
                {
                    marketItems.AddRange(spaceStations[i].GetComponent<SpaceStation>().MarketItems);
                }

                marketItems.RemoveAll(x => x.Quantity == 0);

                if (marketItems.Count > 0)
                {
                    MarketItem cheapest;

                    if (TradeCommodity == Commodity.None) cheapest = marketItems.MinBy(x => x.Price);
                    else cheapest = marketItems.FindAll(x => x.Equals(TradeCommodity)).MinBy(x => x.Price);

                    Target = cheapest.Location.Entity.Transform.Position;
                    TradeAmount = Random.Shared.Next(1, cheapest.Quantity);
                    TradeLocation = cheapest.Location.Entity;
                    TradeCommodity = cheapest.Commodity;
                    return true;
                }
            }

            return false;
        }

        private bool CheckToBuy()
        {
            SpaceStation dock = DockedAt.GetComponent<SpaceStation>();
            if (dock != null)
            {
                if (dock.MarketItems.Any(x => x.Commodity == TradeCommodity))
                {
                    MarketItem marketItem = dock.MarketItems.FirstOrDefault(x => x.Commodity == TradeCommodity);
                    if (marketItem.Quantity >= TradeAmount) return true;
                }
            }

            return false;
        }

        private void Buy()
        {
            SpaceStation dock = DockedAt.GetComponent<SpaceStation>();
            if (dock != null)
            {
                if (dock.MarketItems.Any(x => x.Commodity == TradeCommodity))
                {
                    MarketItem marketItem = dock.MarketItems.FirstOrDefault(x => x.Commodity == TradeCommodity);
                    marketItem.Quantity -= TradeAmount;
                    double price = TradeAmount * marketItem.Price;
                    Money -= price;
                    CommodityCargoItem cargo = Cargo.FirstOrDefault(x => x.Commodity == TradeCommodity);
                    cargo.Amount += TradeAmount;
                }
            }
        }

        private void HandleMovement()
        {
            if (Target != Vector2.Zero)
            {
                Vector2 pos = Entity.Transform.Position;
                Vector2 toTarget = Target - pos;
                float dist = toTarget.Length();

                if (dist <= ArrivalThreshold)
                {
                    if (NextTargets.Count > 0)
                    {
                        Target = NextTargets[0];
                        NextTargets.RemoveAt(0);
                        _velocity *= 0.5f;
                    }
                    else
                    {
                        Target = Vector2.Zero;
                        _velocity = Vector2.Zero;
                        return;
                    }
                }

                float desiredAngle = MathF.Atan2(toTarget.Y, toTarget.X);
                float current = Entity.Transform.Rotation;
                float delta = WrapAngle(desiredAngle - current);
                float maxDelta = TurnSpeed * Time.Delta;

                if (MathF.Abs(delta) < maxDelta) current = desiredAngle;
                else current += MathF.Sign(delta) * maxDelta;

                Entity.Transform.Rotation = current;

                if (MathF.Abs(delta) < 0.1f)
                {
                    float targetSpeed = Speed;
                    float decelerationRange = Speed / 2.0f;
                    if (dist < decelerationRange) targetSpeed *= dist / decelerationRange;

                    Vector2 forward = new(MathF.Cos(current), MathF.Sin(current));
                    Vector2 desiredVel = forward * targetSpeed;

                    float t = MathF.Min(1.0f, Speed * Time.Delta / targetSpeed);
                    _velocity = Vector2.Lerp(_velocity, desiredVel, t);
                }

                Entity.Transform.Position += _velocity * Time.Delta;
            }
        }

        private static float WrapAngle(float angle)
        {
            while (angle <= -MathF.PI) angle += MathHelper.TwoPi;
            while (angle > MathF.PI) angle -= MathHelper.TwoPi;
            return angle;
        }

        public override void Draw()
        {
            base.Draw();

            if (DrawTargetLine && Target != Vector2.Zero)
            {
                Color targetLineColor = Color * TargetLineOpacity;
                Vector2 position = Entity.Transform.Position;
                Vector2 directionToTarget = Target - position;
                float distanceToTarget = directionToTarget.Length();
                if (distanceToTarget > 0f)
                {
                    directionToTarget.Normalize();
                    Vector2 endPoint = position + directionToTarget * distanceToTarget;
                    App.Instance.SpriteBatch.DrawLine(position, endPoint, targetLineColor, TargetLineThickness, 10);
                }
                if (NextTargets.Count > 0)
                {
                    for (int i = 0; i < NextTargets.Count; i++)
                    {
                        targetLineColor = Color * (TargetLineOpacity * 0.75f);
                        Vector2 nextTarget = NextTargets[i];
                        if (i == 0)
                        {
                            App.Instance.SpriteBatch.DrawLine(Target, nextTarget, targetLineColor, TargetLineThickness, 10);
                        }
                        else if (nextTarget != Vector2.Zero)
                        {
                            App.Instance.SpriteBatch.DrawLine(NextTargets[i - 1], nextTarget, targetLineColor, TargetLineThickness, 10);
                        }
                    }
                }
            }
        }
    }

    internal enum SpaceshipController
    {
        None,
        Player,
        AI
    }

    internal enum SpaceshipState
    {
        Idle,
        LookingToBuy,
        MovingToBuy,
        CheckingBuy,
        Buying,
        LookingToSell,
        MovingToSell,
        CheckingSell,
        Selling
    }

    internal struct CommodityCargoItem(Commodity commodity, int amount)
    {
        public Commodity Commodity { get; set; } = commodity;
        public int Amount { get; set; } = amount;
    }
}
