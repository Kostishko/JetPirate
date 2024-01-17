using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace JetPirate
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //Ship - player
        private JetShip jetShip;
        
        //Camera
        private Camera cam;

        //Background
        private Background background;
        private Water water;

        //UI
        private UIManager uiManager;

        //Controller
        private GamePadState  currState;
        private GamePadState prevState;

        //Enemy
        private EnemyManager enemyManager;


        

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
            Content.RootDirectory = "Content";

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




            //camera initialising
            cam = new Camera(Vector2.Zero, new Vector2(-2560, -2880+720), new Vector2(2560, -370), new Vector2(1280, 720));


            // jetShip = new JetShipTest2(Content.Load<Texture2D>("jetship01"), new Vector2(400, 100));
            jetShip = new JetShip(new Vector2(0, -1000), 0f, Content, cam);

            //tested particles
            //_particleSystem = new ParticleSystem(new Vector2(200, 200), -MathHelper.Pi, Content.Load<Texture2D>("fire"), 2f,10f,1f);

            //Enemy
            enemyManager = new EnemyManager(Content, cam, jetShip);

            //UI
            uiManager = new UIManager(cam, jetShip, Content);

            //Background
            background = new Background(Vector2.Zero, 0f, Content, jetShip);
            water = new Water(new Vector2(0,-450), 0f, jetShip, Content);


            //Debugger
            DebugManager.debugFont = Content.Load<SpriteFont>("debugFont");
            DebugManager.spriteBatch = _spriteBatch;
            DebugManager.debugTexture = Content.Load<Texture2D>("DebugBounds");
            DebugManager.isWorking = true;
        }

        protected override void Update(GameTime gameTime)
        {
            //physic
            PhysicManager.UpdateMe();

            currState = GamePad.GetState(PlayerIndex.One);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            jetShip.UpdateMe(currState, prevState, gameTime);
            cam.UpdateMe(jetShip);

            background.UpdateMe();
            water.UpdateMe();
            uiManager.UpdateMe();
            enemyManager.UpdateMe();

            prevState = currState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, cam.GetCam());

            //Background
            background.DrawMe(_spriteBatch);
            water.DrawMe(_spriteBatch);

            //Enemies
            enemyManager.DrawMe(_spriteBatch);

            //  _particleSystem.DrawMe(_spriteBatch);
            jetShip.DrawMe(_spriteBatch);

            DebugManager.DebugString("cam pos:" + cam.position, new Vector2(jetShip.GetPosition().X, jetShip.GetPosition().Y));

            //tested enemy
            //enemy.DrawMe(_spriteBatch);



            //UI drawning
            uiManager.DrawMe(_spriteBatch);

            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}