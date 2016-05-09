using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Edge.Atlas {
    public partial class Atlas {

        Item item;

        void ItemUpdate(Item item) {
            //update
            this.item = item;
            float dt = (currentTime - lastUpdates) / (float)TimeSpan.TicksPerSecond;

            if (item.life <= 0) {
                removeItems.Add(item);
            }
            item.life -= dt;
        }
    }
}
