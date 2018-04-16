﻿using Microsoft.Xna.Framework;
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

        //initialize Game
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

        //Additional functions
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
        void CheckPlayerCollision(Raupe.Raupe _Raupe, MapObject.Crate[] _CArray)
        {
            for(int i = 0; i<_CArray.Length; i++)
            {
                if (_CArray[i] != null)
                {
                    if (Global.VectorDistance(_Raupe.getPosition(), _CArray[i]._position) < _CrateSize)
                    {
                        _CArray[i] = null;
                        _Raupe.AddToLength(1);
                    }
                }
            }
        }
        void DrawCrates()
        {
            for (int j = 0; j < _maxCrateNum; j++)
            {
                if (_crateArray[j] != null)
                {
                    _crateArray[j].Draw();
                }
            }
        }
        void DrawSprites()
        {
            Global.spriteBatch.Begin();
            Global.spriteBatch.Draw(_textureCursor, new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y));
            Global.spriteBatch.End();
            GraphicsDevice.DepthStencilState = DepthStencilState.Default; //muss sein da spritebatch.begin den Stencil auf none setzt
        }
        void UpdateCamera()
        {
            //Kamera
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && _mouseClickSkipCounter > 10)
            {
                Global._freeCam = !Global._freeCam;
                _mouseClickSkipCounter = 0;
            }

            if (Global._freeCam)
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

        }
        void RotateCrates(GameTime gameTime)
        {
            //Kisten drehen
            for (int h = 0; h < _crateArray.Length; h++)
            {
                if (_crateArray[h] != null)
                {
                    _crateArray[h].Update(gameTime);
                }
            }
        }
        void CheckMouseClick()
        {
            _mouseClickSkipCounter++;
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && _mouseClickSkipCounter > 10) //Spiel starten/pausieren durch linksclick
            {
                if (!_player._isAlive)
                {
                    _player.Respawn();
                    _crateArray = new MapObject.Crate[_maxCrateNum];
                    Global._minCameraZoom = -18;
                    Global._maxCameraZoom = -2;
                }
                else
                {
                    Global._gameActive = !Global._gameActive;
                }
                _mouseClickSkipCounter = 0;
            }

        }
        void UpdateWorld(GameTime gameTime)
        {

            //Kistenspawnen
            if (Global.CountNullEntries(_crateArray) == _maxCrateNum)
            {
                SpawnCrates(3);
            }

            _player.Update(gameTime);

            CheckPlayerCollision(_player, _crateArray);

        }

        //Draw
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.ForestGreen);

            //3D Zeug
            _player.Draw();
            DrawCrates();

            //2D Zeug
            DrawSprites();

            base.Draw(gameTime);
        }

        //Update
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            UpdateCamera();
            CheckMouseClick();

            if (Global._gameActive) //läuft gerade eine Runde
            {
                UpdateWorld(gameTime);
            }

            RotateCrates(gameTime);

            base.Update(gameTime);


        }

        /* unused functions
        //Unload the Content 
        protected override void UnloadContent()
        {
        }        
        */
    }
}