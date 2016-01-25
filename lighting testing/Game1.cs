using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using HardShadows;
using lighting_testing.HardShadows;
using Microsoft.Xna.Framework.Input.Touch;

namespace lighting_testing {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ConvexHull[] objects;
        LightSource[] lights;

        RenderTarget2D lightMap;
        Texture2D alphaClearTexture;
        Texture2D playerTexture;
        Vector2 playerPosition;

        Texture2D tileTexture;

        private void BuildObjectList() {

            ConvexHull.InitializeStaticMembers(GraphicsDevice);
            objects = new ConvexHull[37];
            Color wallColor = Color.Black;

            Vector2[] points = new Vector2[4];
            points[0] = new Vector2(0, 0);
            points[1] = new Vector2(0, 20);
            points[2] = new Vector2(800, 20);
            points[3] = new Vector2(800, 0);


            objects[0] = new ConvexHull(this, points, wallColor, Vector2.Zero);
            objects[1] = new ConvexHull(this, points, wallColor, new Vector2(0, 580));

            points[0] = new Vector2(0, 20);
            points[1] = new Vector2(0, 580);
            points[2] = new Vector2(20, 580);
            points[3] = new Vector2(20, 20);


            objects[2] = new ConvexHull(this, points, wallColor, Vector2.Zero);
            objects[3] = new ConvexHull(this, points, wallColor, new Vector2(780, 0));


            points[0] = new Vector2(0, 0);
            points[1] = new Vector2(0, 20);
            points[2] = new Vector2(560, 20);
            points[3] = new Vector2(560, 0);


            objects[4] = new ConvexHull(this, points, wallColor, new Vector2(20, 140));
            objects[5] = new ConvexHull(this, points, wallColor, new Vector2(20, 440));

            points[0] = new Vector2(0, 0);
            points[1] = new Vector2(0, 140);
            points[2] = new Vector2(20, 140);
            points[3] = new Vector2(20, 0);


            objects[6] = new ConvexHull(this, points, wallColor, new Vector2(580, 140));
            objects[7] = new ConvexHull(this, points, wallColor, new Vector2(580, 320));


            points[0] = new Vector2(0, 0);
            points[1] = new Vector2(0, 20);
            points[2] = new Vector2(20, 20);
            points[3] = new Vector2(20, 0);


            objects[8] = new ConvexHull(this, points, wallColor, new Vector2(640, 140));
            objects[9] = new ConvexHull(this, points, wallColor, new Vector2(640, 240));
            objects[10] = new ConvexHull(this, points, wallColor, new Vector2(640, 340));
            objects[11] = new ConvexHull(this, points, wallColor, new Vector2(640, 440));

            objects[12] = new ConvexHull(this, points, wallColor, new Vector2(720, 140));
            objects[13] = new ConvexHull(this, points, wallColor, new Vector2(720, 240));
            objects[14] = new ConvexHull(this, points, wallColor, new Vector2(720, 340));
            objects[15] = new ConvexHull(this, points, wallColor, new Vector2(720, 440));


            points[0] = new Vector2(0, 0);
            points[1] = new Vector2(0, 80);
            points[2] = new Vector2(20, 80);
            points[3] = new Vector2(20, 0);


            objects[16] = new ConvexHull(this, points, wallColor, new Vector2(100, 500));

            objects[17] = new ConvexHull(this, points, wallColor, new Vector2(200, 460));

            objects[18] = new ConvexHull(this, points, wallColor, new Vector2(300, 500));
            objects[19] = new ConvexHull(this, points, wallColor, new Vector2(400, 460));

            objects[20] = new ConvexHull(this, points, wallColor, new Vector2(500, 500));
            objects[21] = new ConvexHull(this, points, wallColor, new Vector2(580, 460));


            points[0] = new Vector2(0, 0);
            points[1] = new Vector2(0, 40);
            points[2] = new Vector2(60, 40);
            points[3] = new Vector2(60, 0);


            objects[22] = new ConvexHull(this, points, wallColor, new Vector2(160, 280));
            objects[23] = new ConvexHull(this, points, wallColor, new Vector2(360, 280));

            points[0] = new Vector2(0, -20);
            points[1] = new Vector2(-20, 0);
            points[2] = new Vector2(0, 20);
            points[3] = new Vector2(20, 0);

            objects[24] = new ConvexHull(this, points, wallColor, new Vector2(460, 80));
            objects[25] = new ConvexHull(this, points, wallColor, new Vector2(560, 80));
            objects[26] = new ConvexHull(this, points, wallColor, new Vector2(160, 80));

            points = new Vector2[8];
            float angleSlice = MathHelper.TwoPi / 8.0f;

            for (int i = 0; i < 8; i++) {
                points[i] = new Vector2((float)Math.Sin(angleSlice * i), (float)Math.Cos(angleSlice * i)) * 20;
            }

            objects[27] = new ConvexHull(this, points, wallColor, new Vector2(140, 220));
            objects[28] = new ConvexHull(this, points, wallColor, new Vector2(240, 220));
            objects[29] = new ConvexHull(this, points, wallColor, new Vector2(340, 220));
            objects[30] = new ConvexHull(this, points, wallColor, new Vector2(440, 220));

            objects[31] = new ConvexHull(this, points, wallColor, new Vector2(140, 380));
            objects[32] = new ConvexHull(this, points, wallColor, new Vector2(240, 380));
            objects[33] = new ConvexHull(this, points, wallColor, new Vector2(340, 380));
            objects[34] = new ConvexHull(this, points, wallColor, new Vector2(440, 380));

            objects[35] = new ConvexHull(this, points, wallColor, new Vector2(80, 300));
            objects[36] = new ConvexHull(this, points, wallColor, new Vector2(500, 300));
        }

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            BuildObjectList();
            playerTexture = Content.Load<Texture2D>("WizardSquare");
            playerPosition = new Vector2(100, 100);

            tileTexture = Content.Load<Texture2D>("tile");

            Texture2D lightTexture = Content.Load<Texture2D>("light");
            lights = new LightSource[6];
            lights[0] = new LightSource(lightTexture, Color.White, 150, playerPosition);
            lights[1] = new LightSource(lightTexture, Color.Crimson, 250, new Vector2(40, 400));
            lights[2] = new LightSource(lightTexture, Color.CornflowerBlue, 250, new Vector2(40, 200));
            lights[3] = new LightSource(lightTexture, Color.Gold, 200, Vector2.Zero);
            lights[4] = new LightSource(lightTexture, Color.Red, 150, new Vector2(510, 30));
            lights[5] = new LightSource(lightTexture, Color.ForestGreen, 150, new Vector2(50, 540));

            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            lightMap = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false,
                                   pp.BackBufferFormat, pp.DepthStencilFormat, pp.MultiSampleCount,
                                   RenderTargetUsage.DiscardContents);
            alphaClearTexture = Content.Load<Texture2D>("AlphaOne");
            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            TouchCollection touches = TouchPanel.GetState();
            if (touches.Count > 0) {
                if (touches[0].State == TouchLocationState.Moved || touches[0].State == TouchLocationState.Pressed) {
                    playerPosition = touches[0].Position;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W))
                playerPosition.Y--;
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                playerPosition.X--;
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                playerPosition.Y++;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                playerPosition.X++;

            lights[0].Position = playerPosition;

            double time = gameTime.TotalGameTime.TotalSeconds / 4.0f;
            lights[3].Position = new Vector2(700, 300 + (float)Math.Sin(time) * 200);


            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //build lightmap
            DrawLightmap();

            graphics.GraphicsDevice.Clear(Color.White);

            //draw ground
            DrawGround();

            //draw objects
            foreach (ConvexHull hull in objects) {
                hull.Draw(gameTime);
            }

            //multiply scene with lightmap
            spriteBatch.Begin(SpriteSortMode.Immediate, CustomBlendStates.Multiplicative);
            spriteBatch.Draw(lightMap, Vector2.Zero, Color.White);
            spriteBatch.End();

            //draw player, fully lit
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            Vector2 origin = new Vector2(playerTexture.Width, playerTexture.Height) / 2.0f;
            spriteBatch.Draw(playerTexture, playerPosition, null, Color.White, 0, origin, 1.0f, SpriteEffects.None, 0);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        private void DrawGround() {
            //draw the tile texture tiles across the screen
            Rectangle source = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            spriteBatch.Draw(tileTexture, Vector2.Zero, source, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            spriteBatch.End();

        }

        private void DrawLightmap() {
            GraphicsDevice.SetRenderTarget(lightMap);

            //clear to some small ambient light
            GraphicsDevice.Clear(new Color(50, 50, 50, 255));


            foreach (LightSource light in lights) {
                //clear alpha to 1
                ClearAlphaToOne();

                //draw all shadows
                //write only to the alpha channel, which sets alpha to 0
                GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                GraphicsDevice.BlendState = CustomBlendStates.WriteToAlpha;

                foreach (ConvexHull ch in objects) {
                    //draw shadow
                    ch.DrawShadows(light);
                }

                //draw the light shape
                //where Alpha is 0, nothing will be written
                spriteBatch.Begin(SpriteSortMode.Immediate, CustomBlendStates.MultiplyWithAlpha);
                light.Draw(spriteBatch);
                spriteBatch.End();
            }
            //clear alpha, to avoid messing stuff up later
            ClearAlphaToOne();
            GraphicsDevice.SetRenderTarget(null);
        }

        private void ClearAlphaToOne() {
            spriteBatch.Begin(SpriteSortMode.Immediate, CustomBlendStates.WriteToAlpha);
            spriteBatch.Draw(alphaClearTexture, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.End();
        }
    }
}
