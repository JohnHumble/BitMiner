using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitMiner
{
    class PickupManager
    {
        public List<PickUp> pickUps { get; protected set; }

        public int Count { get { return pickUps.Count; } }

        public PickupManager()
        {
            pickUps = new List<PickUp>();
        }
        
        public void Add(PickUp pickUp)
        {
            pickUps.Add(pickUp);
        }

        public PickUp Index(int i)
        {
            return pickUps[i];
        }

        public Item CollectAt(int index)
        {
            Item item = pickUps[index].GetItem();
            pickUps.RemoveAt(index);
            return item;
        }

        public void Update()
        {
            foreach(var pickup in pickUps)
            {
                pickup.Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D fill)
        {
            foreach (var pickup in pickUps)
            {
                pickup.Draw(spriteBatch, fill);
            }
        }
    }
}
