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
        public Vector3 _camTarget;
        public Vector3 _camPosition;
        public Matrix _projectionMatrix;
        public Matrix _viewMatrix;
        public Matrix _worldMatrix;
        private int _prevMouseWheelValue;

        //Initialize
        public Camera()
        {
            Init();
        }
        public void Init()
        {
            _prevMouseWheelValue = Mouse.GetState().ScrollWheelValue;
            _camTarget = new Vector3(0f, 0f, 0f);
            _camPosition = new Vector3(0f, 0f, -9);
            _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                               MathHelper.ToRadians(45f),
                               Global.graphics.GraphicsDevice.Viewport.AspectRatio,
                               1f, 1000f);
            _viewMatrix = Matrix.CreateLookAt(_camPosition, _camTarget,
                         new Vector3(0f, 1f, 0f));
            _worldMatrix = Matrix.CreateWorld(_camTarget, Vector3.Forward, Vector3.Up);

        }

        //Getter und Setter
  
       
        //Additional Functions
        private void ZoomIn()
        {
            if (_camPosition.Z < -2)
                _camPosition.Z += 0.5f;
            _prevMouseWheelValue = Mouse.GetState().ScrollWheelValue;

        }
        private void ZoomOut()
        {
            if (_camPosition.Z > -18)
                _camPosition.Z -= 0.5f;
            _prevMouseWheelValue = Mouse.GetState().ScrollWheelValue;

        }


        //Use Additional Functions to Compute Update()
        public void Update()
        {
            //Zoomen
            if (Mouse.GetState().ScrollWheelValue > _prevMouseWheelValue)
            {
                ZoomIn();
 
            }
            if (Mouse.GetState().ScrollWheelValue < _prevMouseWheelValue)
            {
                ZoomOut();
            }

            _viewMatrix = Matrix.CreateLookAt(_camPosition, _camTarget,
                         Vector3.Up);
        }

    }
}
