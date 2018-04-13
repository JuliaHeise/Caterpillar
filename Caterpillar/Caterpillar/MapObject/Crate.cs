using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Caterpillar.MapObject
{
    public class Crate
    {
        public Vector3 _position;
        Model _modelBody;
        public Vector3 _direction;
        int _phase;
        float _spinSpeed = 0.01f;

        public Crate(Vector3 pos)
        {

            _position = pos;
            _modelBody = Global.ContentManager.Load<Model>("Tail1");
            _direction = new Vector3(0, 1, 0);
            _phase = (int)Global._rndCratePhase.Next(0,3);
        }

        public void Update(GameTime gameTime)
        {/*
            if (_direction.X < 0.006)
                _direction.X = 0;

            if (_direction.X > 0.96)
                _direction.X = 1;
            if (_direction.Y > 0.96)
                _direction.Y = 1;*/

            Vector3 _directionNorm;

            _directionNorm = _direction;
            _directionNorm.Normalize();

            if (_phase == 0)
            {
                _direction.X += _spinSpeed;
                _direction.Y -= _spinSpeed;
                if (_direction.Y < _spinSpeed)
                {
                    _direction.Y = 0;
                    _direction.X = 1;
                    _phase = 1;
                }
            }
            else if (_phase == 1)
            {
                _direction.X -= _spinSpeed;
                _direction.Y -= _spinSpeed;
                if (_direction.X < _spinSpeed)
                {
                    _direction.Y = -1;
                    _direction.X = 0;
                    _phase = 2;
                }
            }
            else if (_phase == 2)
            {
                _direction.X -= _spinSpeed;
                _direction.Y += _spinSpeed;
                if (_direction.Y > -_spinSpeed)
                {
                    _direction.Y = 0;
                    _direction.X = -1;
                    _phase = 3;
                }
            }
            else
            {
                _direction.X += _spinSpeed;
                _direction.Y += _spinSpeed;
                if (_direction.X > -_spinSpeed)
                {
                    _direction.Y = 1;
                    _direction.X = 0;
                    _phase = 0;
                }
            }


            //_direction.Normalize();

        }

        public void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {

            foreach (ModelMesh mesh in _modelBody.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.AmbientLightColor = new Vector3(0, 0.5f, 0);
                    effect.View = viewMatrix;
                    effect.World = Matrix.CreateWorld(_position, Vector3.Forward, _direction); ;
                    effect.Projection = projectionMatrix;
                }
                mesh.Draw();
            }

        }
    }
}
