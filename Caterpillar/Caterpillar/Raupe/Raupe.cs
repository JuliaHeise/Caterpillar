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
        Head _head;
        Body _body;
        public Raupe()
        {
            _head = new Head();
            _body = new Body(_head.GetPos());
        }

        public void Update(GameTime gameTime)
        {
            _head.Update(gameTime);
            if ((_body.GetPos() - _body.GetAim()).Length() <  _body.GetSpeed())
            {
                _body.SetAim(_head.GetPos());
                
            }
            _body.Update(gameTime);
        }

        public void Load()
        {
            _head.Load();
            _body.Load();
        }

        public void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            _head.Draw(viewMatrix, projectionMatrix);
            _body.Draw(viewMatrix, projectionMatrix);
        }

    }
}
