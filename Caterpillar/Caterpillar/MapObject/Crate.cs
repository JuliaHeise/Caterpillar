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

        public Crate(Vector3 pos)
        {
            _position = pos;
            _modelBody = Global.ContentManager.Load<Model>("Tail1");
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
                    effect.World = Matrix.CreateWorld(_position, Vector3.Forward, Vector3.Up); ;
                    effect.Projection = projectionMatrix;
                }
                mesh.Draw();
            }

        }
    }
}
