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
        private float _modelSize = 0.4f;
        private float _speed = 0.05f; //= 0.048f;

        //constructor
        public Body(Vector3 aimPos, Vector3 pos)
        {
            _position = pos;
            _aim = aimPos;
            _direction =_aim - _position;
        }

        //Getter und Setter
        public Vector3 GetDir()
        {
            return this._direction;
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

        public void Load()
        {
            _modelBody = Global.ContentManager.Load<Model>("Tail1");
        }

        public void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {

            foreach (ModelMesh mesh in _modelBody.Meshes)
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

        //Update
        public void Update(GameTime gameTime)
        {
            _direction = _aim - _position;
            _direction.Normalize();

            if (Global.VectorDistance(_position, _aim) > _modelSize)
            {
                _position += _direction * _speed;
            }
           /* else
                _position -= _direction * _speed;*/

        }


    }
}
