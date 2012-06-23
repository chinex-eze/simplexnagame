using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 

using Microsoft.Xna.Framework; 
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace _1001823_XNA_MiniProject6
{
    class Missile : Sprite, IFireAble 
    {
        private Vector3 mFireFrom;
        private Vector3 mDirection;

        private int mSpeed = 50; //default missile speed  

        private BoundingSphere sphere; 
        private SpaceShip mSpaceShip; ///temp; this should be an interface

        public Missile()
        {
        }


        public Vector3 FireFrom
        {
            get { return mFireFrom; }
            set
            {
                /*
                 * Position = new Vector2(value.X, value.Y); 
                    mFireFrom = value; 
                 */
                mFireFrom = value;  
                Vector3 temp = Game1.GDevice.Viewport.Project(Vector3.Zero, 
                                mSpaceShip.Projection, mSpaceShip.View,
                                    Matrix.CreateTranslation(value));

                Position = new Vector2(temp.X, temp.Y); 
            } 
        }

        public Vector3 Direction
        {
            get { return mDirection; }
            set { mDirection = value * mSpeed; } 
        }


        public override void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            base.LoadContent(theContentManager, theAssetName);
            base.Scale = 0.1f; 
            CreateBoundingSphere(); 
        }

        public void LoadContent(ContentManager theContentManager, string theAssetName, 
                            SpaceShip sShip)
        {
            this.LoadContent(theContentManager, theAssetName);
            mSpaceShip = sShip; 
        }

        public int Speed
        {
            get { return mSpeed; }
            set { mSpeed = value; } 
        }

        public void Update(GameTime gtime)
        {
            base.Move(new Vector2(mDirection.X, mDirection.Y));

            /*mFireFrom += mDirection; 
            sphere.Transform(Matrix.CreateTranslation(mFireFrom));*/
            mFireFrom = Game1.GDevice.Viewport.Unproject(new Vector3(Position.X,Position.Y,0), 
                        mSpaceShip.Projection, mSpaceShip.View, Matrix.Identity); 
                        //mSpaceShip.Projection, mSpaceShip.View, mSpaceShip.World);  
            sphere.Transform(Matrix.CreateTranslation(mFireFrom));
        }

        public new void Draw(SpriteBatch theSpriteBatch)
        {
            base.Draw(theSpriteBatch); 
        }

        public Missile ShallowCopy()
        {
            return (Missile)this.MemberwiseClone(); 
        }

        public new Vector2 Position
        {
            get { return base.Position; }
            set { base.Position = value; } 
        }

        public BoundingSphere Sphere 
        {
            get { return sphere; }
            set { sphere = value; }
        }

        public SpaceShip SpaceShip
        {
            get { return this.mSpaceShip; }
            set { this.mSpaceShip = value; } 
        }

        public void CreateBoundingSphere() 
        {
            sphere = new BoundingSphere();
            sphere.Center = mFireFrom;       
            //sphere.Radius = Math.Max(mSpriteTexture.Height/2, mSpriteTexture.Width/2); 
            //sphere.Radius = Math.Max(base.Size.Height / 2, base.Size.Width / 2); 
            sphere.Radius = 7.9f; 
            //sphere.Radius *= base.Scale ;
            //sphere.Radius *= 0.01f; 
        } 
    }
}
