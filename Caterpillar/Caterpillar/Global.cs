﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Caterpillar
{
    public class Global : Game
    {
        public static Random _rndCratePhase = new Random();
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static ContentManager ContentManager;
        public static Camera.Camera GameCamera;
        public static float _minCameraZoom = -18;
        public static float _maxCameraZoom = -2;
        public static bool _freeCam = false;

        public static bool _gameActive = false;

        public static int viewSizeWidth = 1600; //Breite des Spielfensters
        public static int viewSizeHeight = 900;

        public static double VectorDistance(Vector3 v1, Vector3 v2) // Ignoriert Z da "höhe"
        {
            return (Math.Sqrt(Math.Pow(v2.X - v1.X, 2) + Math.Pow(v2.Y - v1.Y, 2)));
        }

        public static double VectorAngle(Vector3 v1, Vector3 v2) // Ignoriert Z da "höhe"
        {
            return (Math.Acos( (v1.X*v2.X + v1.Y*v2.Y)/(v1.Length() * v2.Length()) )) * 360 / (2 * Math.PI);
        }

        public static int CountNullEntries(MapObject.Crate[] array)
        {
            int _count = 0;
            for (int j = 0; j < array.Length; j++)
            {
                if (array[j] == null)
                {
                    _count++;
                }
            }
            return _count;
        }

    }
}
