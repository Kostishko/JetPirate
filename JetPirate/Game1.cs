using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.DirectWrite;
using System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace JetPirate
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //Game statement
        public enum GameState
        {
            pause,
            game,            
            menu
        }
        public GameState currentGameState;



        //Ship - player
        public JetShip jetShip;
        
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
        public EnemyManager enemyManager;


        

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
            Content.RootDirectory = "Content";

            _graphics.PreferredBackBufferHeight = 720;
            _graphics.PreferredBackBufferWidth = 1280;
            currentGameState = GameState.menu;
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
            uiManager = new UIManager(cam, jetShip, Content, this);
            uiManager.menuPanel = UIManager.MenuPanel.Control;

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


            currState = GamePad.GetState(PlayerIndex.One);
            switch (currentGameState)
            {
                case GameState.game:
                    GameUpdate(gameTime);
                    break;
                case GameState.pause:
                    break;                                           
            }



            //ui update
            uiManager.UpdateMe(prevState, currState);


            //physic
            prevState = currState;

            base.Update(gameTime);


            
        }

       

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, cam.GetCam());
            //Background - draw it in any state
            background.DrawMe(_spriteBatch);

            switch (currentGameState)
            {
                case GameState.game:
                    GameDraw();
                    break;
                case GameState.pause:
                    GameDraw();
                    break;
                
                             
            }

            

            // DebugManager.DebugString("cam pos:" + cam.position, new Vector2(jetShip.GetPosition().X, jetShip.GetPosition().Y));

            //UI draw - send there the current state
            uiManager.DrawMe(_spriteBatch);

            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

      

        #region Game

        private void GameUpdate(GameTime gameTime)
        {
            PhysicManager.UpdateMe();           

            jetShip.UpdateMe(currState, prevState, gameTime);
            cam.UpdateMe(jetShip);

            background.UpdateMe();
            water.UpdateMe();            
            enemyManager.UpdateMe();

            //Pause case
            if(currState.Buttons.Back==ButtonState.Released&&prevState.Buttons.Back==ButtonState.Pressed)
            {
                currentGameState = GameState.pause;
                uiManager.menuPanel = UIManager.MenuPanel.Pause;

            }

            if(enemyManager.enemyCounter>=10)
            {
                uiManager.gameMajor.jetShip.Restore();
                uiManager.gameMajor.enemyManager.ResetMe();
                uiManager.gameMajor.currentGameState = GameState.pause;
                uiManager.menuPanel = UIManager.MenuPanel.Win;
            }

            if(jetShip.GetHealth()<=0)
            {
                uiManager.gameMajor.jetShip.Restore();
                uiManager.gameMajor.enemyManager.ResetMe();
                uiManager.gameMajor.currentGameState = GameState.pause;
                uiManager.menuPanel = UIManager.MenuPanel.Lose;
            }

        }

        private void GameDraw()
        {
            //water - bottom border
            water.DrawMe(_spriteBatch);

            //Enemies
            enemyManager.DrawMe(_spriteBatch);

            //  _particleSystem.DrawMe(_spriteBatch);
            jetShip.DrawMe(_spriteBatch);
        }

        #endregion


    }
}