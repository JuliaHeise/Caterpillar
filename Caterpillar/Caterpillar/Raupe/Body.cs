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
    class Body
    {
        //Attribute
        Model _modelBody;
        private Vector3 _position;
        private Vector3 _direction;
        private Vector3 _aim;
        private float _inimodelSize = 0.4f;
        private float _modelSize = 0.4f;
        private float _initspeed = 0.05f; //= 0.048f;
        private float _speed = 0.05f; //= 0.048f;
        private float _scale;

        //constructor
        public Body(Vector3 aimPos, Vector3 pos)
        {
            _position = pos;
            _aim = aimPos;
            Init();

        }
        public void Init()
        {
            _direction = _aim - _position;
            _scale = 1f;
        }
        public void Load()
        {
            _modelBody = Global.ContentManager.Load<Model>("Tail1");
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
        public float GetSpeed()
        {
            return _speed;
        }
        public Vector3 GetAim()
        {
            return _aim;
        }
        public void SetAim(Vector3 pos)
        {
            _aim = pos;
        }

        //Draw
        public void Draw()
        {

            foreach (ModelMesh mesh in _modelBody.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.AmbientLightColor = new Vector3(0, 0.2f, 0);
                    effect.View = Global.GameCamera._viewMatrix;
                    effect.World = Matrix.CreateScale(_scale) * Matrix.CreateWorld(_position, Vector3.Forward, _direction); ;
                    effect.Projection = Global.GameCamera._projectionMatrix;
                }
                mesh.Draw();
            }

        }

        //Additional Functions

        //Use Additional Functions to compute Update()
        public void Update(GameTime gameTime, float _playerScale)
        {
            _scale = _playerScale;
            _modelSize = _inimodelSize * _scale;
            _speed = _initspeed * _scale;

            _direction = _aim - _position;
            _direction.Normalize();

            if (Global.VectorDistance(_position, _aim) > _modelSize)
            {
                _position += _direction * _speed;
            }

        }


    }
}
