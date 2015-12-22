using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Edge.Hyperion.UI.Components;
using AssetStore = Edge.Hyperion.Backing.AssetStore;

namespace Edge.Hyperion.UI.Implementation.Popups.Panel {
    public class MenuStrip : Popup {
        Screen Screen;
        Single buttonScale = .61f;
        private List<Button> btnList;

        public MenuStrip(Game game, Screen screen, Vector2 location, List<Button> BtnList) : base(game, location) {
            Screen = screen;
            btnList = BtnList;
            this.Width = (int)(261 * buttonScale);
            this.Height = (int)(100 * buttonScale);
            btnList.Insert(0, new Button(that, this, new Rectangle(), AssetStore.ButtonTypes[Button.ButtonStyle.ButtonStyles.basic], "", () => { }));
            //while (this.Width * btnList.Count < that.GraphicsDevice.Viewport.Width) {
            //    
            //    btnList.Insert(btnList.Count, new Button(that, this, new Rectangle(), AssetStore.ButtonTypes[Button.ButtonStyle.ButtonStyles.basic], "", () => { }));
            //}
            var mid = that.GraphicsDevice.Viewport.Width / 2;
            for (var i = 0; i < btnList.Count; i++) {
                if (i == 0) {
                    btnList[i]._location = new Rectangle(mid - ((btnList.Count / 2) * this.Width + (this.Width/(btnList.Count % 2 == 0 ? 2 : 1))), 10, this.Width, this.Height);
                    continue;
                }
                btnList[i]._location = btnList[i - 1]._location;
                btnList[i]._location.X = btnList[i - 1]._location.X + this.Width;
                that.Components.Add(btnList[i]);
            }
		}

        public override void Initialize() {
            base.Initialize();
        }
    }
}
