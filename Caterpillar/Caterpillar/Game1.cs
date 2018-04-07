using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Caterpillar
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //3D Camera
        Vector3 camTarget;
        Vector3 camPosition;
        Matrix projectionMatrix;
        Matrix viewMatrix;
        Matrix worldMatrix;

        //Momentanes Modell zum 3D zeug testen, Löschen wenn wir eine Player Klasse haben
        Model deleteMe;
        Model deleteMeTail;
        Vector3 RotAroundYAxes = Vector3.Up;
        //Matrix RotationMatrix;
        //Quaternion rotationQuaternion = Quaternion.Identity;
        Vector3 deleteMePlayerPosition = new Vector3(0f, 0f, 0f);
        Vector3 deleteMePlayerTailPosition = new Vector3(0f, -0.45f, 0f);

        //Konstanten
        int viewSizeWidth = 1600; //Breite des Spielfensters
        int viewSizeHeight = 900;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            //graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            //Setup Camera
            camTarget = new Vector3(0f, 0f, 0f);
            camPosition = new Vector3(0f, 0f, -5);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                               MathHelper.ToRadians(45f), 
                               graphics.GraphicsDevice.Viewport.AspectRatio, 
                               1f, 1000f);
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget,
                         new Vector3(0f, 1f, 0f));
            worldMatrix = Matrix.CreateWorld(camTarget, Vector3.Forward, Vector3.Up);

            graphics.PreferredBackBufferWidth = viewSizeWidth;  // Fenstermaße setzen
            graphics.PreferredBackBufferHeight = viewSizeHeight;   
            graphics.ApplyChanges();

            deleteMe = Content.Load<Model>("HeadV1");
            deleteMeTail = Content.Load<Model>("Tail1");
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {


            //Camera Drehen Test, später anderst implementieren
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
                ButtonState.Pressed || Keyboard.GetState().IsKeyDown(
                Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                camPosition.X -= 0.1f;
                camTarget.X -= 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                camPosition.X += 0.1f;
                camTarget.X += 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                camPosition.Y -= 0.1f;
                camTarget.Y -= 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                camPosition.Y += 0.1f;
                camTarget.Y += 0.1f;
            }

            //Zoomen
            if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
            {
                camPosition.Z += 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
            {
                camPosition.Z -= 0.1f;
            }

            //Model Rotations Test
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                //RotationMatrix *= Matrix.CreateFromAxisAngle(RotationMatrix.Up, MathHelper.ToRadians(1.0f));
                //rotationQuaternion.Y = MathHelper.ToRadians(-90);
                RotAroundYAxes = Vector3.Up;
                //worldMatrix = Matrix.CreateWorld(camTarget, Vector3.Forward, RotAroundYAxes);
                deleteMePlayerPosition.Y += 0.1f;
                deleteMePlayerTailPosition.Y += 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                RotAroundYAxes = Vector3.Right;
                // worldMatrix = Matrix.CreateWorld(camTarget, camPosition, RotAroundYAxes);
                deleteMePlayerPosition.X += 0.1f;
                deleteMePlayerTailPosition.X += 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                RotAroundYAxes = Vector3.Down;
                // worldMatrix = Matrix.CreateWorld(camTarget, Vector3.Forward, RotAroundYAxes);
                deleteMePlayerPosition.Y -= 0.1f;
                deleteMePlayerTailPosition.Y -= 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                RotAroundYAxes = Vector3.Left;
                //  worldMatrix = Matrix.CreateWorld(camTarget, Vector3.Forward, RotAroundYAxes);
                deleteMePlayerPosition.X -= 0.1f;
                deleteMePlayerTailPosition.X -= 0.1f;
            }

            


            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget,
                         Vector3.Up);

           // worldMatrix = Matrix.CreateWorld(camTarget, Vector3.Forward, RotAroundYAxes);
            base.Update(gameTime);


        }

        protected override void Draw(GameTime gameTime)
        {


            GraphicsDevice.Clear(Color.ForestGreen);
           

            foreach (ModelMesh mesh in deleteMe.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //effect.EnableDefaultLighting();
                    effect.AmbientLightColor = new Vector3(1f, 0, 0);
                    effect.View = viewMatrix;
                    effect.World = Matrix.CreateWorld(deleteMePlayerPosition, Vector3.Forward, RotAroundYAxes); ;
                        /*transforms[mesh.ParentBone.Index] 
         Matrix.CreateScale(1, 1, 1) *
         Matrix.CreateFromQuaternion(rotationQuaternion) *
         Matrix.CreateTranslation(camTarget);*/
                    effect.Projection = projectionMatrix;
                }
                mesh.Draw();
            }
            foreach (ModelMesh mesh in deleteMeTail.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //effect.EnableDefaultLighting();
                    effect.AmbientLightColor = new Vector3(1f, 0, 0);
                    effect.View = viewMatrix;
                    effect.World = Matrix.CreateWorld(deleteMePlayerTailPosition, Vector3.Forward, Vector3.Up);
                    effect.Projection = projectionMatrix;
                }
                mesh.Draw();
            }
            base.Draw(gameTime);
        }
    }
}