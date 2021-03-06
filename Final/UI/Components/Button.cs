﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Edge.Hyperion.UI.Implementation.Popups;
using Edge.Hyperion.Backing;

namespace Edge.Hyperion.UI.Components {
    public class Button : UIComponent {
        bool _hovering;
        Screen _screen;
        Vector2 _textLocation;
        internal Rectangle _location;
        readonly Style _style;
        readonly ButtonAction _action;
        readonly string _text;
        readonly Vector2 _measurements;

        public delegate void ButtonAction();

        public Button(Game game, Screen screen, Rectangle location, Style style, string text, ButtonAction action) : base(game) {
            _location = location;
            _action = action;
            _style = style;
            _text = text;
            _screen = screen;
            _measurements = _style.Font.MeasureString(_text) / 2;
            that.viewMatrix = Matrix.Identity;
        }

        public override void Update(GameTime gameTime) {
            _hovering = _location.Contains(AssetStore.mouse.Location);
            if (_hovering && AssetStore.mouse.IsButtonToggledUp(Mouse.MouseButtons.Left) && _screen._isActive)
                _action();
            // I know this is Bad but i dont want to think of how to make it better just took act
            _textLocation = new Vector2(_location.Width / 2f - _measurements.X / 2f + _location.X, _location.Height / 2f - _measurements.Y / 2f + _location.Y);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime) {
            if (_screen._isActive) {
                that.batch.End();
                that.batch.Begin();
                that.batch.Draw(_hovering ? _style.Hover : _style.Base, _location, _hovering && _screen._isActive ? _style.HoverColour : _style.BaseColour);
                that.batch.DrawString(_style.Font, _text, _textLocation, _style.TextColour, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
                that.batch.End();
            }
            base.Draw(gameTime);
        }

        public class Style {
            public enum Type : byte {
                basic,
                disabled
            }

            public Texture2D Base, Hover;
            public Color BaseColour, HoverColour, TextColour;
            public SpriteFont Font;

            public Style(Texture2D texture, Texture2D hover, Color? baseColour, Color? hoverColour, Color? textColour) {
                Base = texture;
                Hover = hover;
                BaseColour = baseColour ?? Color.LightGray;
                HoverColour = hoverColour ?? Color.White;
                TextColour = textColour ?? Color.Black;
                Font = Edge.Hyperion.Backing.AssetStore.FontMain;
            }

            public Style(Texture2D texture, Color? baseColour, Color? hoverColour, Color? textColour) {
                Base = texture;
                Hover = texture;
                BaseColour = baseColour ?? Color.LightGray;
                HoverColour = hoverColour ?? Color.White;
                TextColour = textColour ?? Color.Black;
                Font = Edge.Hyperion.Backing.AssetStore.FontMain;
            }

            public Style(Texture2D texture, Texture2D hover, SpriteFont font, Color? baseColour, Color? hoverColour, Color? textColour) {
                Base = texture;
                Hover = hover;
                BaseColour = baseColour ?? Color.LightGray;
                HoverColour = hoverColour ?? Color.White;
                TextColour = textColour ?? Color.Black;
                Font = font;
            }

            public Style(Texture2D texture, SpriteFont font, Color? baseColour, Color? hoverColour, Color? textColour) {
                Base = texture;
                Hover = texture;
                BaseColour = baseColour ?? Color.LightGray;
                HoverColour = hoverColour ?? Color.White;
                TextColour = textColour ?? Color.Black;
                Font = font;
            }
        }
    }
}

