using AdAstra.Engine.Maths;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AdAstra.Engine.Entities.Components
{
    internal class SpaceStation : Component
    {
        public string Name { get; set; } = "Space Station";
        public Color Color { get; set; } = Color.White;
        public List<MarketItem> MarketItems { get; set; } = [];

        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            GenerateMarketItems();
        }

        public string GetMarketItemInfo()
        {
            if (MarketItems.Count == 0) return "No items available in the market.";
            string info = "Market Items:/n";
            for (int i = 0; i < MarketItems.Count; i++)
            {
                MarketItem item = MarketItems[i];
                info += $"{item.Commodity} - Quantity: {item.Quantity} - Price: {item.Price:n2}/n";
            }
            return info;
        }

        private void GenerateMarketItems()
        {
            MarketItems.Clear();
            PRNG random = new();
            Commodity[] commodities = (Commodity[])Enum.GetValues(typeof(Commodity));
            for (int i = 0; i < commodities.Length; i++)
            {
                Commodity commodity = commodities[i];
                if (commodity == Commodity.None) continue;
                int quantity = random.Int(1, 100);
                double price = Math.Round(random.Double(100.0d, 500.0d), 2);
                MarketItems.Add(new MarketItem(commodity, quantity, price, this));
            }
        }
    }

    internal enum Commodity
    {
        None,
        Food,
        Water,
        Oxygen,
        Fuel,
        Medicine,
        Electronics,
        Machinery,
        Metals,
        RareMaterials
    }

    internal struct MarketItem(Commodity commodity, int quantity, double price, SpaceStation location)
    {
        public Commodity Commodity { get; set; } = commodity;
        public int Quantity { get; set; } = quantity;
        public double Price { get; set; } = price;
        public SpaceStation Location { get; set; } = location;
    }
}
