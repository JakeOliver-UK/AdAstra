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
            string info = "Market Items:\n";
            foreach (var item in MarketItems)
            {
                info += $"{item.Commodity} - Quantity: {item.Quantity}, Price: {item.Price:C}\n";
            }
            return info;
        }

        private void GenerateMarketItems()
        {
            MarketItems.Clear();
            var random = new PRNG();
            int itemCount = random.Int(5, 11);
            for (int i = 0; i < itemCount; i++)
            {
                Commodity commodity = (Commodity)random.Int(1, Enum.GetValues(typeof(Commodity)).Length);
                int quantity = random.Int(1, 101);
                float price = random.Float(1.0f, 100.0f);
                
                MarketItems.Add(new MarketItem(commodity, quantity, price));
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

    internal struct MarketItem(Commodity commodity, int quantity, float price)
    {
        public Commodity Commodity { get; set; } = commodity;
        public int Quantity { get; set; } = quantity;
        public float Price { get; set; } = price;
    }
}
