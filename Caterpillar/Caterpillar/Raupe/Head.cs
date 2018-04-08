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
        private float _speed = 0.02f;

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
                    effect.AmbientLightColor = new Vector3(0, 0, 0);
                    effect.View = viewMatrix;
                    effect.World = Matrix.CreateWorld(_position, Vector3.Forward, _direction); ;
                     /*transforms[mesh.ParentBone.Index] 
         Matrix.CreateScale(1, 1, 1) *
         Matrix.CreateFromQuaternion(rotationQuaternion) *
         Matrix.CreateTranslation(camTarget);*/
                    effect.Projection = projectionMatrix;
                }
                mesh.Draw();
            }

        }

        //Update
        public void Update(GameTime gameTime)
        {
            //_direction = new Vector3(0.0f, 0.0f, 0.0f);
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                _direction = new Vector3(0.0f, 1.0f, 0.0f);
                _position += new Vector3(0.0f, _speed, 0.0f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                _direction = new Vector3(1.0f, 0.0f, 0.0f);
                _position += new Vector3(_speed, 0.0f, 0.0f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                _direction = new Vector3(0.0f, -1.0f, 0.0f);
                _position += new Vector3(0.0f, -_speed, 0.0f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                _direction = new Vector3(-1.0f, 0.0f, 0.0f);
                _position += new Vector3(-_speed, 0.0f, 0.0f);
            }
            //_position += _direction * _speed  ;
        }
    }
}
