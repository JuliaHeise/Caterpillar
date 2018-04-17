using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;
using System.IO;


namespace Caterpillar
{

    public class Caterpillar : Game
    {
        Random _rnd;
        Texture2D _textureCursor;
        Texture2D _eatingEffect;
        int _mouseClickSkipCounter=0;
        bool _deathSkipAClick = false;
        Model GameBackground;
        SoundEffect _eatingSound;


        //Player
        Raupe.Raupe _player;

        //Konstanten

        float _InitialCrateDistance = 0.5f;

        //Kistenspawnfunktion
        public static int _maxCrateNum = 100;
        public static MapObject.Crate[] _crateArray;

        void SpawnCrates(int n, float _cSize)
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
                _xPos = (int)(0.5* Global.gameSizeWidth) - _rnd.Next(0,Global.gameSizeWidth); 

                for (int j = 0; j<_maxCrateNum; j++)
                {
                   // _rnd = new Random();
                    _yPos = (int)(0.5 * Global.gameSizeHeight) - _rnd.Next(0, Global.gameSizeHeight);
                    while ( ((_xPos<4) && (_xPos > -4)) && ((_yPos < 4) && (_yPos > -4)) )
                        _yPos = (int)(0.5 * Global.gameSizeHeight) - _rnd.Next(0, Global.gameSizeHeight);


                    if (_crateArray[j] == null)
                    {
                        
                        _crateArray[j] = new MapObject.Crate(new Vector3(_xPos, _yPos, 0), _cSize);
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
                    // if (Global.VectorDistance(_Raupe.getPosition(), _CArray[i]._position) < 0.5*_InitialCrateDistance+ 0.5 * _InitialCrateDistance * _Raupe._scale)
                    if (Global.VectorDistance(_Raupe.getPosition(), _CArray[i]._position) < 0.5 * _InitialCrateDistance* _CArray[i]._size + 0.5 * _InitialCrateDistance * _Raupe._scale)
                    {
                        if (Global._gamePhase >= _CArray[i]._size - 1)
                        {
                            _CArray[i] = null;
                            _Raupe.AddToLength(1);
                            _eatingSound.Play();
                        }
                        else
                        {
                            _deathSkipAClick = true;
                            _Raupe._isAlive = false;
                            Global._gameActive = false;
                        }
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
            _eatingEffect = this.Content.Load<Texture2D>("EatingAnim1v2");
            GameBackground = Global.ContentManager.Load<Model>("Background");
            _eatingSound = this.Content.Load<SoundEffect>("EatingSound");
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


            //Kamera
            if(Keyboard.GetState().IsKeyDown(Keys.LeftShift) && _mouseClickSkipCounter > 10)
            {
                Global._freeCam = !Global._freeCam;
                _mouseClickSkipCounter = 0;
            }

            if(Global._freeCam)
            {
                Global.GameCamera.Update();
            }
            else
            {/*
                if (!Keyboard.GetState().IsKeyDown(Keys.W) && !Keyboard.GetState().IsKeyDown(Keys.A)
                    && !Keyboard.GetState().IsKeyDown(Keys.S) && !Keyboard.GetState().IsKeyDown(Keys.D))
                {*/
                    Global.GameCamera._camTarget = _player.getPosition();
                    Global.GameCamera._camPosition = new Vector3(_player.getPosition().X, _player.getPosition().Y, Global.GameCamera._camPosition.Z);
               // }
                Global.GameCamera.Update();
            }




            _mouseClickSkipCounter++;
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && _mouseClickSkipCounter>10) //Spiel starten durch linksclick
            {
                if (!_player._isAlive)
                {
                    _player.Respawn();
                    _crateArray = new MapObject.Crate[_maxCrateNum];
                    Global._minCameraZoom = -18;
                    Global._maxCameraZoom = -2;
                    Global.GameCamera._camPosition.Z = -9;

                    if (!_deathSkipAClick)
                    {
                        Global._gameActive = !Global._gameActive;
                    }
                    _deathSkipAClick = false;
                }
                _mouseClickSkipCounter = 0;
            }

            if (Mouse.GetState().RightButton == ButtonState.Pressed && _mouseClickSkipCounter > 10) //Spiel pausieren durch rechtsclick
            {
                Global._gameActive = !Global._gameActive;
                _mouseClickSkipCounter = 0;
            }


                if (Global._gameActive) //läuft gerade eine Runde
            {

                //Kistenspawnen
                if (Global.CountNullEntries(_crateArray) == _maxCrateNum)
                {
                    SpawnCrates(15, 1); //15:10
                    SpawnCrates(25, 2); //40:30
                    SpawnCrates(15, 4); //55:40
                    //SpawnCrates(10, 5); //65:50
                    SpawnCrates(5, 6);
                    SpawnCrates(2, 10);
                }

                _player.Update(gameTime);

                CheckPlayerCollision(_player, _crateArray);
            }

            //Kisten drehen
            for(int h = 0; h < _crateArray.Length; h++)
            {
                if(_crateArray[h]!=null)
                {
                    _crateArray[h].Update(gameTime);
                }
            }



            Global._gamePhase = (int)(_player._score / 10);

            base.Update(gameTime);


        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.ForestGreen);


            //Background
            foreach (ModelMesh mesh in GameBackground.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //effect.EnableDefaultLighting();
                    effect.AmbientLightColor = new Vector3(0, 0, 0);
                    effect.View = Global.GameCamera._viewMatrix;
                    effect.World = Matrix.CreateWorld(new Vector3(0, 0, 0), Vector3.Forward, Vector3.Up);
                    effect.Projection = Global.GameCamera._projectionMatrix;
                }
                mesh.Draw();
            }



            //2D Zeug unter 3D Map Zeug
            Global.spriteBatch.Begin();
            if (Global._isEatingAnimation>0)
            {
                Global.spriteBatch.Draw(_eatingEffect, new Vector2(0.5f*Global.viewSizeWidth- 0.5f * _eatingEffect.Width -_player.getDirection().X*75, 0.5f*Global.viewSizeHeight - 0.5f * _eatingEffect.Height - _player.getDirection().Y * 75));
                Global._isEatingAnimation--;
            }
            Global.spriteBatch.End();
            GraphicsDevice.DepthStencilState = DepthStencilState.Default; //muss sein da spritebatch.begin den Stencil auf none setzt



            //3D Map Zeug
            _player.Draw();

            for (int j = 0; j < _maxCrateNum; j++)
            {
                if (_crateArray[j] != null)
                {
                    _crateArray[j].Draw();
                }
            }

            //2D Zeug über 3D zeug
            Global.spriteBatch.Begin();
            Global.spriteBatch.Draw(_textureCursor, new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y));
            Global.spriteBatch.End();
            GraphicsDevice.DepthStencilState = DepthStencilState.Default; 

            base.Draw(gameTime);
        }
    }
}