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
        //Screens
        Texture2D _TitleTexture;
        Texture2D _CreditsTexture;
        Texture2D _ControlsTexture;
        Texture2D _LoseTexture;
        Texture2D _WinTexture;

        int _stoneNum = 3;



        Random _rnd;
        Texture2D _textureCursor;
        Texture2D _eatingEffect;
        int _mouseClickSkipCounter=0;
        bool _deathSkipAClick = false;
        bool _cheatDeath = false;
        bool _afterGame = false;
        Model[] GameBackground;
        SoundEffect _eatingSound;
        SoundEffect _dyingSound;
        SoundEffect _forestSound;
        SoundEffectInstance _forestLoop;
        static bool _allObjectsSpawned = false;
        static bool _onlyDoOnce = true;


        //Player
        Raupe.Raupe _player;

        //Konstanten

        //Kistenspawnfunktion
        public static int _maxCrateNum = 200;
        public static MapObject.Crate[] _crateArray;
        public static int _emptySpacePostSpawn;
        public static int _borderElementNum = 0;

        public static void DrawBorder()
        {
            _emptySpacePostSpawn = Global.CountNullEntries(_crateArray);

            Vector3 _BorderPos = new Vector3(Global.gameSizeWidth+6, Global.gameSizeHeight+6, 0);
            for (int i = 0; i < _BorderPos.X/3; i++)
            {
                if (_allObjectsSpawned)
                {
                    int _emptySpace = Global.CountNullEntries(_crateArray);
                    _crateArray[_crateArray.Length - _emptySpace] = new MapObject.Crate(new Vector3(0.5f*_BorderPos.X-i*3, 0.5f * _BorderPos.Y, 0), 100);
                }
            }
            for (int i = 0; i < _BorderPos.X / 3; i++)
            {
                if (_allObjectsSpawned)
                {
                    int _emptySpace = Global.CountNullEntries(_crateArray);
                    _crateArray[_crateArray.Length - _emptySpace] = new MapObject.Crate(new Vector3(0.5f * _BorderPos.X - i * 3, 0.5f * -_BorderPos.Y, 0), 100);
                }
            }
            
            for (int i = 0; i < _BorderPos.Y / 3; i++)
            {
                if (_allObjectsSpawned)
                {
                    int _emptySpace = Global.CountNullEntries(_crateArray);
                    _crateArray[_crateArray.Length - _emptySpace] = new MapObject.Crate(new Vector3(0.5f * _BorderPos.X, 0.5f * _BorderPos.Y - i * 3, 0), 100);
                }
            }
            for (int i = 0; i < _BorderPos.Y / 3; i++)
            {
                if (_allObjectsSpawned)
                {
                    int _emptySpace = Global.CountNullEntries(_crateArray);
                    _crateArray[_crateArray.Length - _emptySpace] = new MapObject.Crate(new Vector3(0.5f * - _BorderPos.X, 0.5f * _BorderPos.Y - i * 3, 0), 100);
                }
            }
        }

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
                    for (int h = 0; h < _maxCrateNum; h++)
                    {
                        if (_crateArray[h] != null)
                        {
                            _yPos = (int)(0.5 * Global.gameSizeHeight) - _rnd.Next(0, Global.gameSizeHeight);
                            while (((_xPos < 4) && (_xPos > -4)) && ((_yPos < 4) && (_yPos > -4))
                                || (_crateArray[h].IsColliding(new Vector3(_xPos, _yPos, 0)))
                                || (new MapObject.Crate(new Vector3(_xPos, _yPos, 0), _cSize).IsColliding(_crateArray[h]._position)))
                                _yPos = (int)(0.5 * Global.gameSizeHeight) - _rnd.Next(0, Global.gameSizeHeight);
                        }
                    }



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
                    if(_CArray[i].IsColliding(_Raupe.getPosition()))
                    {
                        if (Global._gamePhase >= _CArray[i]._size - 1)
                        {
                            _CArray[i] = null;
                            _Raupe.AddToLength(1);
                            _eatingSound.Play();
                        }
                        else if (!_cheatDeath)
                        {
                            _deathSkipAClick = true;
                            _Raupe._isAlive = false;
                            _dyingSound.Play();
                            _Raupe.gameLost();
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
            //Screens
            _TitleTexture = Global.ContentManager.Load<Texture2D>("Titel2");
            _CreditsTexture = Global.ContentManager.Load<Texture2D>("Credits");
            _ControlsTexture = Global.ContentManager.Load<Texture2D>("Control");
            _LoseTexture = Global.ContentManager.Load<Texture2D>("GameOver");
            _WinTexture = Global.ContentManager.Load<Texture2D>("GameWon2");



            Global.spriteBatch = new SpriteBatch(GraphicsDevice);
            _textureCursor = Global.ContentManager.Load<Texture2D>("CursorMini");
            _eatingEffect = Global.ContentManager.Load<Texture2D>("EatingAnim1v4");

            //background
            GameBackground = new Model[9];
            for (int i = 0; i < 9; i++)
            {
                GameBackground[i] = Global.ContentManager.Load<Model>("Background2");
            }



            _eatingSound = Global.ContentManager.Load<SoundEffect>("EatingSound");
            _dyingSound = Global.ContentManager.Load<SoundEffect>("Crack");
            _forestSound = Global.ContentManager.Load<SoundEffect>("ForestSound2");
            _forestLoop = _forestSound.CreateInstance();
            _forestLoop.IsLooped = true;
            _forestLoop.Play();
            _player.Load();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            //Game won?
            if(Global.CountNullEntries(_crateArray) == _maxCrateNum - _borderElementNum - _stoneNum -5 && !_afterGame )
            {
                Global._isWinScreen = true;
            }

            if (_afterGame && Global.CountNullEntries(_crateArray) == _maxCrateNum - _borderElementNum - _stoneNum - 5)
            {
                SpawnCrates(10, 5);
                SpawnCrates(6, 6);
            }


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
                ButtonState.Pressed || Keyboard.GetState().IsKeyDown(
                Keys.Escape))
                Exit();

            _mouseClickSkipCounter++;
            if (Keyboard.GetState().IsKeyDown(Keys.F11) && _mouseClickSkipCounter > 10)
            {
                Global.graphics.IsFullScreen = !Global.graphics.IsFullScreen;
                Global.graphics.ApplyChanges();
                _mouseClickSkipCounter = 0;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F9) && _mouseClickSkipCounter > 10) //cheat
            {
                _cheatDeath = !_cheatDeath;
                _mouseClickSkipCounter = 0;
            }

            //Screens
            if (Global._isTitleScreen)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && _mouseClickSkipCounter > 10) //Spiel starten durch linksclick
                {
                    Global._isTitleScreen = false;
                    Global._isCreditScreen = true;
                    _mouseClickSkipCounter = 0;
                }
            }
            else if (Global._isCreditScreen)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && _mouseClickSkipCounter > 10) //Spiel starten durch linksclick
                {
                    Global._isCreditScreen = false;
                    Global._isControlsScreen = true;
                    _mouseClickSkipCounter = 0;
                }
            }
            else if (Global._isControlsScreen)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && _mouseClickSkipCounter > 10) //Spiel starten durch linksclick
                {
                    Global._isControlsScreen = false;
                    _mouseClickSkipCounter = 0;
                }
            }
            else if (Global._isLoseScreen)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && _mouseClickSkipCounter > 10) //Spiel starten durch linksclick
                {
                    _afterGame = false;
                    Global._isLoseScreen = false;
                    _mouseClickSkipCounter = 0;
                }
            }
            else if (Global._isWinScreen && !_afterGame)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && _mouseClickSkipCounter > 10) //Spiel starten durch linksclick
                {
                    _afterGame = true;
                    Global._isWinScreen = false;
                    _mouseClickSkipCounter = 0;
                }
                if (Mouse.GetState().RightButton == ButtonState.Pressed && _mouseClickSkipCounter > 10) //Spiel starten durch linksclick
                {
                    Global._isWinScreen = false;
                    _deathSkipAClick = true;
                    _player._isAlive = false;
                    _player.gameWon();
                    _mouseClickSkipCounter = 0;
                }
            }
            else
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





                if (Mouse.GetState().LeftButton == ButtonState.Pressed && _mouseClickSkipCounter > 10) //Spiel starten durch linksclick
                {
                    if (!_player._isAlive)
                    {
                        _player.Respawn();
                        //_crateArray = new MapObject.Crate[_maxCrateNum];
                        for (int j = 0; j < _maxCrateNum - _emptySpacePostSpawn; j++)
                        {
                            _crateArray[j] = null;
                        }
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


                if (Global._isDeadTime > 0)
                {
                    Global._isDeadTime--;
                }
                else if (Global._isDeadTime == 0)
                {
                    Global._gameActive = false;
                    Global._isDeadTime--;
                }

                if (Global._gameActive) //läuft gerade eine Runde
                {


                    //Kistenspawnen
                    if (Global.CountNullEntries(_crateArray) == _crateArray.Length
                        || Global.CountNullEntries(_crateArray) == _crateArray.Length - _borderElementNum)
                    {
                        SpawnCrates(15, 1); //15:10
                        SpawnCrates(25, 2); //40:30
                        SpawnCrates(15, 4); //55:40
                        SpawnCrates(10, 5); //65:50
                        SpawnCrates(6, 6);
                        SpawnCrates(_stoneNum, 10);
                        _allObjectsSpawned = true;
                    }

                    _player.Update(gameTime);
                    if (_onlyDoOnce)
                    {
                        DrawBorder();
                        _onlyDoOnce = !_onlyDoOnce;
                        _borderElementNum = _emptySpacePostSpawn - Global.CountNullEntries(_crateArray);
                    }

                    CheckPlayerCollision(_player, _crateArray);
                }


                //Kisten drehen
                for (int h = 0; h < _crateArray.Length; h++)
                {
                    if (_crateArray[h] != null)
                    {
                        if ((_crateArray[h]._position.X < _player.getPosition().X + 16 * _player._scale
                       && _crateArray[h]._position.X > _player.getPosition().X - 16 * _player._scale)
                       && (_crateArray[h]._position.Y < _player.getPosition().Y + 10 * _player._scale
                          && _crateArray[h]._position.Y > _player.getPosition().Y - 10 * _player._scale)) //Object near Player
                            _crateArray[h].Update(gameTime);
                    }
                }



                Global._gamePhase = (int)(_player._score / 10);

                base.Update(gameTime);

            }
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.ForestGreen);


            //Background
            for (int i = 0; i < 9; i++)
            {
                Vector3 _backPos = new Vector3(0, 0, 0);
                if(i ==1)
                    _backPos = new Vector3(Global.gameSizeWidth+5, 0, 0);
                if (i == 8)
                    _backPos = new Vector3(-Global.gameSizeWidth - 5, 0, 0);
                if (i == 2)
                    _backPos = new Vector3(Global.gameSizeWidth + 1.8f, Global.gameSizeHeight + 1.8f, 0);
                if (i == 3)
                    _backPos = new Vector3(Global.gameSizeWidth + 1.8f, -Global.gameSizeHeight - 1.8f, 0);
                if (i == 4)
                    _backPos = new Vector3(-Global.gameSizeWidth - 1.8f, -Global.gameSizeHeight - 1.8f, 0);
                if (i == 5)
                    _backPos = new Vector3(-Global.gameSizeWidth - 1.8f, Global.gameSizeHeight + 1.8f, 0);
                if (i == 6)
                    _backPos = new Vector3(0, Global.gameSizeHeight + 5, 0);
                if (i == 7)
                    _backPos = new Vector3(0, -Global.gameSizeHeight - 5, 0);


                foreach (ModelMesh mesh in GameBackground[i].Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        //effect.EnableDefaultLighting();
                        effect.AmbientLightColor = new Vector3(0, 0, 0);
                        effect.View = Global.GameCamera._viewMatrix;
                        effect.World = Matrix.CreateWorld(_backPos, Vector3.Forward, Vector3.Up);
                        effect.Projection = Global.GameCamera._projectionMatrix;
                    }
                    mesh.Draw();
                }
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
                    if((_crateArray[j]._position.X < _player.getPosition().X + 16 *_player._scale
                        && _crateArray[j]._position.X > _player.getPosition().X - 16 * _player._scale)
                        && (_crateArray[j]._position.Y < _player.getPosition().Y + 10 * _player._scale
                        && _crateArray[j]._position.Y > _player.getPosition().Y - 10 * _player._scale)) //Object near Player
                    _crateArray[j].Draw();
                }
            }

            //2D Zeug über 3D zeug
            Global.spriteBatch.Begin();
            Global.spriteBatch.Draw(_textureCursor, new Vector2(Mouse.GetState().Position.X - _textureCursor.Width, Mouse.GetState().Position.Y - _textureCursor.Height));

            //Screens
            if (Global._isTitleScreen)
                Global.spriteBatch.Draw(_TitleTexture, new Vector2(0, 0));
            else if (Global._isCreditScreen)
                Global.spriteBatch.Draw(_CreditsTexture, new Vector2(0, 0));
            else if (Global._isControlsScreen)
                Global.spriteBatch.Draw(_ControlsTexture, new Vector2(0, 0));
            else if (Global._isLoseScreen)
                Global.spriteBatch.Draw(_LoseTexture, new Vector2(0, 0));
            else if (Global._isWinScreen)
                Global.spriteBatch.Draw(_WinTexture, new Vector2(0, 0));


            Global.spriteBatch.End();
            GraphicsDevice.DepthStencilState = DepthStencilState.Default; 





            base.Draw(gameTime);
        }
    }
}