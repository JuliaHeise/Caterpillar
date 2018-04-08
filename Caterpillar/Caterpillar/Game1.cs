using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Caterpillar
{

    public class Game1 : Game
    {
        //Player
        Raupe.Raupe _player;

        //Konstanten
        int viewSizeWidth = 1600; //Breite des Spielfensters
        int viewSizeHeight = 900;

        public Game1()
        {
            Global.graphics = new GraphicsDeviceManager(this);
            //graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
            Global.ContentManager = Content;
            Global.graphics.PreferredBackBufferWidth = viewSizeWidth;  // Fenstermaße setzen
            Global.graphics.PreferredBackBufferHeight = viewSizeHeight;
            Global.graphics.ApplyChanges();
            //Global Camera init
            Global.Camera = new Camera.Camera();
            //Player
            _player = new Raupe.Raupe();

        }

        protected override void Initialize()
        {
            base.Initialize();        
        }

        protected override void LoadContent()
        {
            Global.spriteBatch = new SpriteBatch(GraphicsDevice);
            _player.Load();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            //Camera Drehen Test, später anderst implementieren
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
                ButtonState.Pressed || Keyboard.GetState().IsKeyDown(
                Keys.Escape))
                Exit();
            Global.Camera.Update();

            _player.Update(gameTime);

            // worldMatrix = Matrix.CreateWorld(camTarget, Vector3.Forward, RotAroundYAxes);
            base.Update(gameTime);


        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.ForestGreen);
            _player.Draw(Global.Camera.viewMatrix, Global.Camera.projectionMatrix);
            base.Draw(gameTime);
        }
    }
}