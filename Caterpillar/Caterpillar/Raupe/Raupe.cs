using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Caterpillar.Raupe
{
    class Raupe
    {
        float _tailSize = 0.5f;
        int _score;
        public bool _isAlive;
        Head _head;
        Body[] _bodyPartArray;
        int _maxTailLength = 50;

        public Raupe()
        {
            _score = 0;
            _head = new Head();
            _bodyPartArray = new Body[_maxTailLength];
            _isAlive = true;
        }

        public bool CheckPulse()
        {
            for (int j = 2; j < _bodyPartArray.Length; j++) //0 und 1 überspringen
            {
                if (_bodyPartArray[j] != null)
                {
                    if (Global.VectorDistance(this.getPosition(), _bodyPartArray[j].GetPos()) < _tailSize)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void Respawn()
        {
            _score = 0;
            _head = new Head();
            _bodyPartArray = new Body[_maxTailLength];
            _isAlive = true;
            Load();
        }


        public  int CountEntries(Body[] array)
        {
            int _count = 0;
            for (int j = 0; j < array.Length; j++)
            {
                if (array[j] != null)
                {
                    _count++;
                }
            }
            return _count;
        }

        public void Update(GameTime gameTime)
        {

            _isAlive = CheckPulse();

            //Tod oder Sieg abfangen
            if (!_isAlive)
            {
                Global._gameActive = false;
            }
            else if (_score == _maxTailLength)
            {
                gameWon();
                _isAlive = false;
                Global._gameActive = false;
            }


            _head.Update(gameTime);

            for (int i = 0; i < CountEntries(_bodyPartArray); i++)
            {
                if (_bodyPartArray[i] == null)
                    break;
                else
                {
                    Body _bodyPart = _bodyPartArray[i];

                    if (i == 0)
                        _bodyPart.SetAim(_head.GetPos()); //vorderster Part folgt Head
                    else
                        _bodyPart.SetAim(_bodyPartArray[i - 1].GetPos()); //andere Parts folgen Part vor ihnen 

                    _bodyPart.Update(gameTime);
                }

            }

        }

        public Vector3 getPosition()
        {
            return _head.GetPos();
        }

        public void gameLost()
        {
            Global._gameActive = false;
        }

        public void gameWon()
        {
           
        }

        public void addToLength(int n)
        {
            if (_score == 0)
            {
                _bodyPartArray[0]= new Body(_head.GetPos(), _head.GetPos());
            }
            else
            {
                _bodyPartArray[_score] = new Body(_bodyPartArray[CountEntries(_bodyPartArray)-1].GetPos(), 
                    _bodyPartArray[CountEntries(_bodyPartArray)-1].GetPos());
            }

            _bodyPartArray[_score].Load();
            _score += n;
        }

        public void Load()
        {
            _head.Load();

            for (int i = 0; i < CountEntries(_bodyPartArray); i++)
            {
                _bodyPartArray[i].Load();
            }

        }

        public void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            _head.Draw(viewMatrix, projectionMatrix);

            for (int i = 0; i < CountEntries(_bodyPartArray); i++)
            {
                _bodyPartArray[i].Draw(viewMatrix, projectionMatrix);
            }
        }

    }
}
