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
        private Vector3 _direction;
        private Vector3 _upwardsDirection;
        private float _iniSpeed = 0.05f;
        private float _speed = 0.05f;
        private float _scale;


        //Initialize
        public Head()
        {
            Init();
        }
        public void Load()
        {
            _modelHead = Global.ContentManager.Load<Model>("HeadV1");
        }
        public void Init()
        {
            _position = new Vector3(0.0f, 0.0f, 0.0f);
            _direction = new Vector3(0.0f, 1.0f, 0.0f);
            _upwardsDirection = Vector3.Forward;
            _scale = 1f;
        }

        //Getter und Setter
        public Vector3 GetDir()
        {
            return _direction;
        }
        public Vector3 GetPos()
        {
            return _position;
        }
        public void SetSpeed(float newSpeed)
        {
            _speed = newSpeed;
        }
        public void SpinAround()
        {
            if (_upwardsDirection == Vector3.Backward)
            {
                _upwardsDirection = Vector3.Forward;
                _position.Z = 0;
            }
            else if (_upwardsDirection == Vector3.Forward)
            {
                _upwardsDirection = Vector3.Backward;
                _position.Z = -1;
            }
        }

        //Draw
        public void Draw()
        {         

            foreach (ModelMesh mesh in _modelHead.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.AmbientLightColor = new Vector3(0, 0.2f, 0);
                    effect.View = Global.GameCamera._viewMatrix;
                    effect.World = Matrix.CreateScale(_scale) * Matrix.CreateWorld(_position, _upwardsDirection, _direction); ;
                    effect.Projection = Global.GameCamera._projectionMatrix;
                }
                mesh.Draw();
            }
        }

        //Additional functions
    
        //use Additional functions to compute Update()
        public void Update(GameTime gameTime, float _playerScale)
        {
            _scale = _playerScale;
            _speed = _iniSpeed * _scale;
            

            int _halfWidth = (int)(0.5 * Global.viewSizeWidth);
            int _halfHeight = (int)(0.5 * Global.viewSizeHeight);
            int _mouseX;
            int _mouseY;


            if (Global._freeCam)
            {
                int _camCorrX;

                if (Global.GameCamera._camPosition.X == 0)
                    _camCorrX = 0;
                else
                    _camCorrX = (int)(_halfWidth / 10.8);

                int _camCorrY;

                if (Global.GameCamera._camPosition.Y == 0)
                    _camCorrY = 0;
                else
                    _camCorrY = (int)(_halfWidth / 7.5);


                 _mouseX = -(int)((Mouse.GetState().Position.X - _halfWidth)) + (int)Global.GameCamera._camPosition.X * _camCorrX;
                 _mouseY = -(int)((Mouse.GetState().Position.Y - _halfHeight)) + (int)Global.GameCamera._camPosition.Y * _camCorrY;
                float _camScale = 87.5f / (-Global.GameCamera._camPosition.Z / 9); //ertestet, Maß zwischen Modell Koordinatensystem und Maus ist anders

                // Console.Out.WriteLine(Global.VectorAngle(_direction, new Vector3((_mouseX - _position.X * _scale), (_mouseY - _position.Y * _scale), 0.0f)));

                if (Global.VectorAngle(_direction, new Vector3((_mouseX - _position.X * _scale), (_mouseY - _position.Y * _camScale), 0.0f)) < 60)
                {
                    _direction += new Vector3((_mouseX - _position.X * _scale), (_mouseY - _position.Y * _camScale), 0.0f);
                }

            }
            else
            {
                _mouseX = -(int)((Mouse.GetState().Position.X - _halfWidth));
                _mouseY = -(int)((Mouse.GetState().Position.Y - _halfHeight));

                if (Global.VectorAngle(_direction, new Vector3((_mouseX), (_mouseY), 0.0f)) < 30)
                {
                    _direction += new Vector3((_mouseX), (_mouseY), 0.0f);
                }
                else if (Global.VectorAngle(_direction, new Vector3((_mouseX), (_mouseY), 0.0f)) > 30) //Raupe verliert Cursor nicht
                {
                    Vector3 _norm = _direction;
                    _norm.Normalize();
                    Mouse.SetPosition((int)(0.5*Global.viewSizeWidth - 50 * _norm.X), (int)(0.5 * Global.viewSizeHeight - 50 * _norm.Y));
                }
            }

            _direction.Normalize();
            _position += _direction * _speed;    
        }
    }
}
