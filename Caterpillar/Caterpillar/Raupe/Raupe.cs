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
        int _score;
        Head _head;
       // Body _body;
        //Stack<Body> _bodyPartStack;
        Body[] _bodyPartArray;

        public Raupe()
        {
            _score = 0;
            _head = new Head();
           // _body = new Body(_head.GetPos());
           // _bodyPartStack = new Stack<Body>();
            _bodyPartArray = new Body[40];

            //test
            // _bodyPartStack.Push(_body);
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
            _head.Update(gameTime);
            /*for (int i = 0; i < _bodyPartStack.Count; i++)
            {
                Body _bodyPart = _bodyPartStack.ElementAt(i);
                /*if ((_bodyPart.GetPos() - _bodyPart.GetAim()).Length() < _bodyPart.GetSpeed())
           //     {
            //        if (i == 0)
           //             _bodyPart.SetAim(_head.GetPos()); //vorderster Part folgt Head
           //         else
           //             _bodyPart.SetAim(_bodyPartStack.ElementAt(_bodyPartStack.Count - 1).GetPos()); //andere Parts folgen Part vor ihnen
          //      }

                if (i == 0)
                    _bodyPart.SetAim(_head.GetPos()); //vorderster Part folgt Head
                else
                    _bodyPart.SetAim(_bodyPartStack.ElementAt(0).GetPos()); //andere Parts folgen Part vor ihnen 
                    
                _bodyPart.Update(gameTime);*/

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

        public void addToLength(int n)
        {
            // Console.Out.WriteLine(_score);
            if (_score == 0)
            {
                _bodyPartArray[0]= new Body(_head.GetPos(), _head.GetPos());
            }
            else
            {
                _bodyPartArray[_score] = new Body(_bodyPartArray[CountEntries(_bodyPartArray)-1].GetPos(), 
                    _bodyPartArray[CountEntries(_bodyPartArray)-1].GetPos());
            }

            //Console.Out.WriteLine(_score);

            _bodyPartArray[_score].Load();
            _score += n;
        }

        public void Load()
        {
            _head.Load();
            //_body.Load();

            for (int i = 0; i < CountEntries(_bodyPartArray); i++)
            {
                _bodyPartArray[i].Load();
            }

        }

        public void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            _head.Draw(viewMatrix, projectionMatrix);
            //_body.Draw(viewMatrix, projectionMatrix);

            for (int i = 0; i < CountEntries(_bodyPartArray); i++)
            {
                _bodyPartArray[i].Draw(viewMatrix, projectionMatrix);
            }
        }

    }
}
