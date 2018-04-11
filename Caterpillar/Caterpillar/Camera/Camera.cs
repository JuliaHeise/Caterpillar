using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Caterpillar.Camera
{
    public class Camera
    {
        //3D Camera
        Vector3 camTarget;
        public Vector3 camPosition;
        public Matrix projectionMatrix;
        public Matrix viewMatrix;
        Matrix worldMatrix;
        private int _prevMouseWheelValue;

        public Camera()
        {
            _prevMouseWheelValue = Mouse.GetState().ScrollWheelValue;
            camTarget = new Vector3(0f, 0f, 0f);
            camPosition = new Vector3(0f, 0f, -9);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                               MathHelper.ToRadians(45f),
                               Global.graphics.GraphicsDevice.Viewport.AspectRatio,
                               1f, 1000f);
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget,
                         new Vector3(0f, 1f, 0f));
            worldMatrix = Matrix.CreateWorld(camTarget, Vector3.Forward, Vector3.Up);
        }

        public void Update()
        {

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                camPosition.X -= 0.1f;
                camTarget.X -= 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                camPosition.X += 0.1f;
                camTarget.X += 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                camPosition.Y -= 0.1f;
                camTarget.Y -= 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                camPosition.Y += 0.1f;
                camTarget.Y += 0.1f;
            }

            //Zoomen
            if (Mouse.GetState().ScrollWheelValue > _prevMouseWheelValue)
            {
                camPosition.Z += 0.5f;
                _prevMouseWheelValue = Mouse.GetState().ScrollWheelValue;
            }
            if (Mouse.GetState().ScrollWheelValue < _prevMouseWheelValue)
            {
                camPosition.Z -= 0.5f;
                _prevMouseWheelValue = Mouse.GetState().ScrollWheelValue;
            }

            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget,
                         Vector3.Up);
        }

    }
}
