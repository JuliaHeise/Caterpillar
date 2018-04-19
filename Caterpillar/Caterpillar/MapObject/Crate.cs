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
        public float _size;
        private Vector3 _Color;

        //Initialize
        public Crate(Vector3 pos, float _size)
        {
            _Color = new Vector3(1f, 0.3f, 0);
            this._size = _size;
            _position = pos;
            Init();
            Load();
        }
        public void Load()
        {
            if(_size<=1)
            _modelBody = Global.ContentManager.Load<Model>("needlev3"); //Nadel
            else if (_size <= 2)
                _modelBody = Global.ContentManager.Load<Model>("BlankLeaf"); //Blatt // size 1 zu 2 : 10 score
            else if (_size <= 4)
                _modelBody = Global.ContentManager.Load<Model>("Branchv4"); //Ast size 2 zu 4 : 20 score --> 30
            else if (_size <= 5)
                _modelBody = Global.ContentManager.Load<Model>("Branchv4"); //Busch // noch kein Model
            else if (_size <= 6)
                _modelBody = Global.ContentManager.Load<Model>("pinev2"); //Baum
            else if (_size <= 10)
                _modelBody = Global.ContentManager.Load<Model>("Stonev1"); //Stein
        }
        public void Init()
        {
            _direction = new Vector3(Global._rndCratePhase.Next(0, 5)-2, Global._rndCratePhase.Next(0, 5) - 2, 0);
            while(_direction.X==0 && _direction.Y==0) //kein Nullvektor
                _direction = new Vector3(Global._rndCratePhase.Next(0, 5) - 2, Global._rndCratePhase.Next(0, 5) - 2, 0);

            _phase = (int)Global._rndCratePhase.Next(0, 4);
        }

        //Draw
        public void Draw()
        {

            foreach (ModelMesh mesh in _modelBody.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.AmbientLightColor = _Color;
                    effect.View = Global.GameCamera._viewMatrix;
                    effect.World = Matrix.CreateScale(_size) * Matrix.CreateWorld(_position, Vector3.Forward, _direction);
                    effect.Projection = Global.GameCamera._projectionMatrix;
                }
                mesh.Draw();
            }

        }

        //Getter and Setter

        //Additional Functions
        public void Rotate()
        {
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

        }

        public bool IsColliding(Vector3 _fPos)
        {
            if(_size!=4)
            {
                if (Global.VectorDistance(_fPos, _position)
                    < 0.5 * 0.6f * _size + 0.5 * 0.5f * Global._playerScale)
                    return true;
            }
            else if (_size == 4)
            {
                float _width = 2f;
                float _height = 5f;

                Vector3[] _corners = new Vector3[4];
                Vector3 _localForward = _direction;
                Vector3 _localRight;
             /*   if (_direction.X < _direction.Y)
                _localRight = new Vector3(-_direction.Y, _direction.X,0);
                else*/
                _localRight = new Vector3(_direction.Y, -_direction.X, 0);


                _localForward.Normalize();
                _localRight.Normalize();
                _corners[0] = _position + 0.5f * _height * _localForward - 0.5f * _width * _localRight; //forward left
                _corners[1] = _corners[0] -  _height * _localForward +  _width * _localRight; //bottom right
                _corners[2] = _corners[0] + _width * _localRight; //forward right
                _corners[3] = _corners[0] - _height * _localForward; //bottom left

                if (Global.PointIsRightOf(_fPos, _corners[0], _corners[2])
                    && Global.PointIsRightOf(_fPos, _corners[2], _corners[1])
                    && Global.PointIsRightOf(_fPos, _corners[1], _corners[3])
                    && Global.PointIsRightOf(_fPos, _corners[3], _corners[0]))
                {
                    return true;
                }


            }
            return false;
        }

 

        //Use Additional Functions to Compute Update
        public void Update(GameTime gameTime)
        {
            if (Global._gamePhase >= _size - 1)
            {
                _Color = new Vector3(0.2f, 2f, 0.5f);
                Rotate();
            }
        }        
    }
}
