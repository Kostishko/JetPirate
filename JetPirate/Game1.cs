﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JetPirate
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private JetShip jetShip;


        private ParticleSystem _particleSystem;


        private Camera cam;

        //Controll
        private GamePadState  currState;
        private GamePadState prevState;
        

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferHeight = 720;
            _graphics.PreferredBackBufferWidth = 1280;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // jetShip = new JetShipTest2(Content.Load<Texture2D>("jetship01"), new Vector2(400, 100));
            jetShip = new JetShip(new Vector2(200, 200), 0f, Content.Load<Texture2D>("jetship01"), Content.Load<Texture2D>("fire_left"));

            _particleSystem = new ParticleSystem(new Vector2(200, 200), -MathHelper.Pi, Content.Load<Texture2D>("fire"), 2f,10f,1f);

            //camera initialising
            cam = new Camera(Vector2.Zero, new Vector2(-2560, -1440), new Vector2(2560,1440), new Vector2(1280,720));

            //Debugger
            DebugManager.debugFont = Content.Load<SpriteFont>("debugFont");
            DebugManager.spriteBatch = _spriteBatch;
            DebugManager.debugTexture = Content.Load<Texture2D>("DebugBounds");
            DebugManager.isWorking = true;
        }

        protected override void Update(GameTime gameTime)
        {
            currState = GamePad.GetState(PlayerIndex.One);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            jetShip.UpdateMe(GamePad.GetState(PlayerIndex.One), gameTime);
            cam.UpdateMe(jetShip);

            if(prevState.Buttons.X==ButtonState.Pressed&&currState.Buttons.X==ButtonState.Released)
            {
                cam.StartShaking(10);
            }
            //  _particleSystem.UpdateMe(jetShip.position, -jetShip.RealRotate);
            //if(Keyboard.GetState().IsKeyDown(Keys.A)|| Keyboard.GetState().IsKeyDown(Keys.D))
            //{
            //    _particleSystem.Play();
            //}
            //else
            //{
            //    _particleSystem.Stop();
            //}
            prevState = currState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, cam.GetCam());

          //  _particleSystem.DrawMe(_spriteBatch);
            jetShip.DrawMe(_spriteBatch);

            DebugManager.DebugString("cam pos:" + cam.position, new Vector2(jetShip.GetPosition().X, jetShip.GetPosition().Y));

            
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}