using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;


namespace Caterpillar
{

    public class Caterpillar : Game
    {
        Random _rnd;
        Texture2D _textureCursor;
        int _mouseClickSkipCounter=0;


        //Player
        Raupe.Raupe _player;

        //Konstanten

        float _CrateSize = 0.5f;

        //Kistenspawnfunktion
        public static int _maxCrateNum = 8;
        public static MapObject.Crate[] _crateArray;


        void SpawnCrates(int n)
        {
            _rnd = new Random();

            int _xPos = 0;
            int _yPos = 0;

            if (n+_crateArray.Length- Global.CountNullEntries(_crateArray) > _maxCrateNum)
            {
                n = Global.CountNullEntries(_crateArray);
            }
            
            for(int i = 0; i<n;i++)
            {
               // _rnd = new Random();
                _xPos = 4 - _rnd.Next(0,8); 

                for (int j = 0; j<_maxCrateNum; j++)
                {
                   // _rnd = new Random();
                    _yPos = 4 - _rnd.Next(0, 8); 


                    if (_crateArray[j] == null)
                    {
                        
                        _crateArray[j] = new MapObject.Crate(new Vector3(_xPos, _yPos, 0));
                        break;
                    }
                }
            }
        }


        //Kollisionsfunktion


        void CheckPlayerCollision(Raupe.Raupe _Raupe, MapObject.Crate[] _CArray)
        {
            for(int i = 0; i<_CArray.Length; i++)
            {
                if (_CArray[i] != null)
                {
                    if (Global.VectorDistance(_Raupe.getPosition(), _CArray[i]._position) < _CrateSize)
                    {
                        _CArray[i] = null;
                        _Raupe.addToLength(1);
                    }
                }
            }
        }





        public Caterpillar()
        {
            Global.graphics = new GraphicsDeviceManager(this);
            //graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
            Global.ContentManager = Content;
            Global.graphics.PreferredBackBufferWidth = Global.viewSizeWidth;  // Fenstermaße setzen
            Global.graphics.PreferredBackBufferHeight = Global.viewSizeHeight;
            Global.graphics.ApplyChanges();
            //Global Camera init
            Global.GameCamera = new Camera.Camera();
            //Player
            _player = new Raupe.Raupe();


            _crateArray = new MapObject.Crate[_maxCrateNum];

        }

        protected override void Initialize()
        {
            //this.IsMouseVisible = true;
            base.Initialize();        
        }

        protected override void LoadContent()
        {
            Global.spriteBatch = new SpriteBatch(GraphicsDevice);
            _textureCursor = this.Content.Load<Texture2D>("Black");
            _player.Load();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
                ButtonState.Pressed || Keyboard.GetState().IsKeyDown(
                Keys.Escape))
                Exit();

            _mouseClickSkipCounter++;
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && _mouseClickSkipCounter>10) //Spiel starten/pausieren durch linksclick
            {
                Global._gameActive = !Global._gameActive;
                _mouseClickSkipCounter = 0;
            }


            Global.GameCamera.Update();

            if (Global._gameActive) //läuft gerade eine Runde
            {

                //Kistenspawnen
                if (Global.CountNullEntries(_crateArray) == _maxCrateNum)
                {
                    SpawnCrates(3);
                }

                _player.Update(gameTime, Global.GameCamera);

                CheckPlayerCollision(_player, _crateArray);
            }



            base.Update(gameTime);


        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.ForestGreen);

            //3D Zeug
            _player.Draw(Global.GameCamera.viewMatrix, Global.GameCamera.projectionMatrix);

            for (int j = 0; j < _maxCrateNum; j++)
            {
                if (_crateArray[j] != null)
                {
                    _crateArray[j].Draw(Global.GameCamera.viewMatrix, Global.GameCamera.projectionMatrix);
                }
            }

            //2D Zeug
            Global.spriteBatch.Begin();
            Global.spriteBatch.Draw(_textureCursor, new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y));
            Global.spriteBatch.End();
            GraphicsDevice.DepthStencilState = DepthStencilState.Default; //muss sein da spritebatch.begin den Stencil auf none setzt

            base.Draw(gameTime);
        }
    }
}