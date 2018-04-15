using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using ArrayList = System.Collections.ArrayList;

namespace Caterpillar.Raupe
{
    class Raupe
    {
        float _tailSize = 0.5f;
        int _score;
        public bool _isAlive;
        Head _head;
        ArrayList _bodyPartArray;
        int _maxTailLength = 50;

        //Initialize "Raupe" 
        public Raupe()
        {
            _head = new Head();
            _bodyPartArray = new ArrayList();
            Init();
        }
        public void Load()
        {
            _head.Load();

            for (int i = 0; i < _bodyPartArray.Count; i++)
            {
                ((Body)_bodyPartArray[i]).Load();
            }

        }
        public void Init()
        {
            _score = 0;
            _head.Init();
            _bodyPartArray.Clear();
            _isAlive = true;

        }
        public void Respawn()
        {
            Init();
        }

        //Getter and Setter
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

        //Draw
        public void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            _head.Draw(viewMatrix, projectionMatrix);

            for (int i = 0; i < _bodyPartArray.Count; i++)
            {
                ((Body)_bodyPartArray[i]).Draw(viewMatrix, projectionMatrix);
            }
        }

        //Additional functions 
        public bool CheckPulse()
        {
            for (int j = 2; j < _bodyPartArray.Count; j++) //0 und 1 überspringen
            {
                if (Global.VectorDistance(getPosition(), ((Body)_bodyPartArray[j]).GetPos()) < _tailSize)
                {
                    return false;
                }
            }
            return true;
        }
        public void AddToLength(int n)
        {
            if (_score == 0)
            {
                _bodyPartArray.Add(new Body(_head.GetPos(), _head.GetPos()));
            }
            else
            {
                _bodyPartArray.Add(new Body(((Body)_bodyPartArray[_bodyPartArray.Count - 1]).GetPos(),
                    ((Body)_bodyPartArray[_bodyPartArray.Count - 1]).GetPos()));
            }

            ((Body)_bodyPartArray[_score]).Load();
            _score += n;
        }
        
        //use Additional functions to calculate the Update()
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

            for (int i = 0; i < _bodyPartArray.Count; i++)
            {
                if (_bodyPartArray[i] == null)
                    break;
                else
                {
                    Body _bodyPart = (Body)_bodyPartArray[i];

                    if (i == 0)
                        _bodyPart.SetAim(_head.GetPos()); //vorderster Part folgt Head
                    else
                        _bodyPart.SetAim(((Body)_bodyPartArray[i - 1]).GetPos()); //andere Parts folgen Part vor ihnen 

                    _bodyPart.Update(gameTime);
                }

            }

        }

    }
}
