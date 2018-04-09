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


        //Player
        Raupe.Raupe _player;

        //Konstanten
        int viewSizeWidth = 1600; //Breite des Spielfensters
        int viewSizeHeight = 900;
        float _CrateSize = 0.5f;

        //Kistenspawnfunktion
        public static int _maxCrateNum = 8;
        public static MapObject.Crate[] _crateArray;


        int CountNullEntries(MapObject.Crate[] array)
        {
            int _count = 0;
            for (int j = 0; j < array.Length; j++)
            {
                if (array[j] == null)
                {
                    _count++;
                }
            }
            return _count;
        }

        void SpawnCrates(int n)
        {
            _rnd = new Random();

            int _xPos = 0;
            int _yPos = 0;

            if (n+_crateArray.Length- CountNullEntries(_crateArray) > _maxCrateNum)
            {
                n = CountNullEntries(_crateArray);
            }
            
            for(int i = 0; i<n;i++)
            {
               // _rnd = new Random();
                _xPos = 5 - _rnd.Next(0,11); // 5 - eine Zahl zwisches 0 und 10 --> Zahl von -5 bis 5

                for (int j = 0; j<_maxCrateNum; j++)
                {
                   // _rnd = new Random();
                    _yPos = 5 - _rnd.Next(0, 11); 


                    if (_crateArray[j] == null)
                    {
                        
                        _crateArray[j] = new MapObject.Crate(new Vector3(_xPos, _yPos, 0));
                        break;
                    }
                }
            }
        }


        //Kollisionsfunktion
        double VectorDistance(Vector3 v1, Vector3 v2) // Ignoriert Z da "höhe"
        {
            return (Math.Sqrt(Math.Pow(v2.X - v1.X,2) + Math.Pow(v2.Y - v1.Y, 2)));
        }

        void CheckPlayerCollision(Raupe.Raupe _Raupe, MapObject.Crate[] _CArray)
        {
            for(int i = 0; i<_CArray.Length; i++)
            {
                if (_CArray[i] != null)
                {
                    if (VectorDistance(_Raupe.getPosition(), _CArray[i]._position) < _CrateSize)
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
            Global.graphics.PreferredBackBufferWidth = viewSizeWidth;  // Fenstermaße setzen
            Global.graphics.PreferredBackBufferHeight = viewSizeHeight;
            Global.graphics.ApplyChanges();
            //Global Camera init
            Global.Camera = new Camera.Camera();
            //Player
            _player = new Raupe.Raupe();


            _crateArray = new MapObject.Crate[_maxCrateNum];

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

            //Kistenspawnen
            if (CountNullEntries(_crateArray) == _maxCrateNum)
            {
                 SpawnCrates(3);
            }

            _player.Update(gameTime);

            CheckPlayerCollision(_player, _crateArray);

            base.Update(gameTime);


        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.ForestGreen);
            _player.Draw(Global.Camera.viewMatrix, Global.Camera.projectionMatrix);

            for (int j = 0; j < _maxCrateNum; j++)
            {
                if (_crateArray[j] != null)
                {
                    _crateArray[j].Draw(Global.Camera.viewMatrix, Global.Camera.projectionMatrix);
                }
            }

            base.Draw(gameTime);
        }
    }
}