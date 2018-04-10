using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;



namespace Caterpillar.Raupe
{
    class Head
    {
        //Attribute
        Model _modelHead;
        private Vector3 _position;
        public Vector3 _prevPos;
        public Vector3 _prevDir;
        private Vector3 _direction;
        private float _speed = 0.05f;

        //constructor
        public Head()
        {
            _position = new Vector3(0.0f, 0.0f, 0.0f);
            _direction = new Vector3(0.0f, 1.0f, 0.0f);
        }

        //Getter und Setter
        public Vector3 GetDir()
        {
            return this._direction;
        }

        public Vector3 GetPos()
        {
            return this._position;
        }

        public void SetSpeed(float newSpeed)
        {
            this._speed = newSpeed;
        }

        public void Load()
        {
            _modelHead = Global.ContentManager.Load<Model>("HeadV1");
        }

        public void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {         

            foreach (ModelMesh mesh in _modelHead.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.AmbientLightColor = new Vector3(0, 0.2f, 0);
                    effect.View = viewMatrix;
                    effect.World = Matrix.CreateWorld(_position, Vector3.Forward, _direction); ;
                    effect.Projection = projectionMatrix;
                }
                mesh.Draw();
            }
        }
        public void Previous()
        {
            _prevDir = _direction;
            _prevPos = _position;
        }

        //Update
        public void Update(GameTime gameTime, Camera.Camera cam)
        {
            int _halfWidth = (int)(0.5 * Global.viewSizeWidth);
            int _halfHeight = (int)(0.5 * Global.viewSizeHeight);

            int _camCorrX;

            if (cam.camPosition.X == 0)
                _camCorrX = 0;
            else
                _camCorrX = (int)(_halfWidth / 10.8);

            int _camCorrY;

            if (cam.camPosition.Y == 0)
                _camCorrY = 0;
            else
                _camCorrY = (int)(_halfWidth / 7.5);


            int _mouseX = -(int)((Mouse.GetState().Position.X- _halfWidth)) + (int)cam.camPosition.X*_camCorrX;
            int _mouseY = -(int)((Mouse.GetState().Position.Y- _halfHeight)) + (int)cam.camPosition.Y*_camCorrY;
            float _scale = 87.5f / (-cam.camPosition.Z / 9); //ertestet, Maß zwischen Modell Koordinatensystem und Maus ist anders

            //Console.Out.WriteLine("Cax "+ cam.camPosition.X); //10.8 = 1/2 width, 0 = 0 
            //Console.Out.WriteLine("Cax " + cam.camPosition.Y); //7.5 = 1/2 height, 0 = 0


            _direction += new Vector3((_mouseX - _position.X * _scale), (_mouseY - _position.Y * _scale), 0.0f);
           // Console.Out.WriteLine(_direction);


            /*
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                _direction += new Vector3(0.0f, 1.0f, 0.0f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                _direction += new Vector3(1.0f, 0.0f, 0.0f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                _direction  += new Vector3(0.0f, -1.0f, 0.0f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                _direction += new Vector3(-1.0f, 0.0f, 0.0f);
            }*/


            _direction.Normalize();
            _position += _direction * _speed;    
        }
    }
}
