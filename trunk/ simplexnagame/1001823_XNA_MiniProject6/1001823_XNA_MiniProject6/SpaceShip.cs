using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework; 

namespace _1001823_XNA_MiniProject6
{
    class SpaceShip
    {
        private Model model; 
        private float aspectRatio; 

        public Vector3 mPosition = new Vector3(0,0,0);
        private float mRotationAngle = MathHelper.ToRadians(0); 

        private Matrix world; 
        private Matrix view; 
        private Matrix projection;

        public static readonly Vector3 LEFT  = new Vector3(-0.05f, 0, 0);  
        public static readonly Vector3 RIGHT = new Vector3(0.05f, 0, 0);
        public static readonly Vector3 UP    = new Vector3(0, -0.05f, 0);
        public static readonly Vector3 DOWN  = new Vector3(0, 0.05f, 0);  

        public float maxLeft = -7.0f, maxRight = 7.0f;
        ///the max and min distance a bullet from this ship can travel 
        public static Vector3 maxHeight, maxDepth; 

        GraphicsDevice mDevice;

        private BoundingSphere sphere = new BoundingSphere(); 

        public SpaceShip()
        {
        }

        public SpaceShip Load(ContentManager theContentManager, GraphicsDevice graphicsDevice, 
                                    string theAssetName)
        {
            mDevice = graphicsDevice; 
            model = theContentManager.Load<Model>(theAssetName); 
            aspectRatio = (float)graphicsDevice.Viewport.Width / (float)graphicsDevice.Viewport.Height;

            world = Matrix.CreateRotationZ(mRotationAngle) * 
                        Matrix.CreateTranslation(new Vector3(0, 0, 0)); 
            view = Matrix.CreateLookAt(new Vector3(0, 0, 12),
                                                    new Vector3(0, 0, 0), Vector3.UnitY);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45),
                                                                aspectRatio, 0.1f, 100f);

            maxHeight = mDevice.Viewport.Project(Vector3.Zero, projection, view,       ///////////////////
                                    Matrix.CreateTranslation(new Vector3(0,6f,0)));
            maxDepth = mDevice.Viewport.Project(Vector3.Zero, projection, view,
                                    Matrix.CreateTranslation(new Vector3(0, -6f, 0)));  ///////////////////

            GetBoundingSphere(); 
            return this; 
        }


        public Vector3 Position
        {
            get { return mPosition; }
            set 
            { 
                mPosition = value;
                sphere.Center = mPosition;  
            } 
        }


        public Matrix World
        {
            get { return world; }
            set 
            { 
                world = value; 
                sphere.Transform(world);  
            } 
        }

        public Matrix View
        {
            get { return view; }
            set { view = value; } 
        }

        public Matrix Projection
        {
            get { return projection; }
            set { projection = value; } 
        }

        

        public void Move(Vector3 Direction)
        {
            mPosition += Direction; 

            if (mPosition.X > this.maxRight) 
                mPosition.X = this.maxRight; 
            else if (Position.X < this.maxLeft) 
                mPosition.X = this.maxLeft; 

            world = Matrix.CreateRotationZ(mRotationAngle) * Matrix.CreateTranslation(mPosition);

            sphere.Transform(world); 
        } 

        public float RotationAngle
        {
            get { return mRotationAngle; }
            set { mRotationAngle = MathHelper.ToRadians(value); } 
        }

        public void Draw()
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = world;
                    effect.View = view; 
                    effect.Projection = projection;
                }

                mesh.Draw();
            }
        }


        public BoundingSphere Sphere
        {
            get { return this.sphere; } 
        }

        public void GetBoundingSphere()
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                if (sphere.Radius == 0)
                    sphere = mesh.BoundingSphere;
                else
                    sphere = BoundingSphere.CreateMerged(sphere, mesh.BoundingSphere);
            }

            sphere.Center = Position; 

            //sphere.Radius *= scale;
        }  

        public bool CheckCollision(BoundingBox box)
        {
            if (sphere.Intersects(box))
                return true;
            else
                return false; 
        }

        public bool CheckCollision(BoundingSphere s)
        {
            if (sphere.Intersects(s))
            {
                return true;
            }
            else
                return false;
        }

        public IFireAble Fire(IFireAble missile, Vector3 direction)
        {
            /*
            missile.FireFrom = mDevice.Viewport.Project(Vector3.Zero, projection, view, 
                                    Matrix.CreateTranslation(this.Position));  */
            missile.FireFrom = this.mPosition; 
            missile.Direction = direction;
            missile.CreateBoundingSphere();  ///bad design, I know 
            return missile; 
        }

        public void Fire(IEnumerable<IFireAble> items)
        {
            foreach (IFireAble item in items)
            {
                item.FireFrom = new Vector3(Position.X, Position.Y, 0);  
            }
        }

        public void Destroy()
        {
        } 
    }
}
