using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;  


namespace _1001823_XNA_MiniProject6
{
    class SpaceShipManager
    {
        private int mNUM_SHIPS = 3;         ///defaults to 3 ships 
        private SpaceShip mCurrentShip = null;
        private List<IFireAble> firedItems = new List<IFireAble>();
        private Missile missile = new Missile();

        public static readonly Vector3 BOTTOM = new Vector3(0, -4.20f, 0);
        public static readonly Vector3 TOP = new Vector3(0, 4.20f, 0); 

        public SpaceShipManager()
        {
            //NextShip();
        }

        public SpaceShipManager(int num_ships)
        {
            mNUM_SHIPS = num_ships;  
        }

        public void Load(ContentManager theContentManager, GraphicsDevice graphicsDevice,
                                    string theAssetName, Vector3 initPosition)
        {    
            mCurrentShip = new SpaceShip();
            mCurrentShip.Load(theContentManager, graphicsDevice, theAssetName);
            mCurrentShip.Position = initPosition;

                if (initPosition == SpaceShipManager.TOP)
                {
                    mCurrentShip.RotationAngle = 183f;
                    mCurrentShip.World = Matrix.CreateRotationZ(MathHelper.ToRadians(183)) *
                                Matrix.CreateTranslation(SpaceShipManager.TOP);
                }
                else
                {
                    mCurrentShip.World = Matrix.CreateTranslation(SpaceShipManager.BOTTOM);
                }
        }

        public void LoadArms(ContentManager theContentManager, string theAssetName)
        {
            missile.LoadContent(theContentManager, theAssetName);
            missile.SpaceShip = this.mCurrentShip; 
        }

        public int NUM_SHIPS
        {
            get { return mNUM_SHIPS; }
            set { mNUM_SHIPS = value; }
        }

        public SpaceShip CurrentShip
        {
            get { return mCurrentShip; }
            //set { mCurrentShip = value; }
        }

        public void NextShip()
        {
            if (mNUM_SHIPS > 0)
            {
                mNUM_SHIPS -= 1;
            }
            else
                mCurrentShip = null; 
        }


        public List<IFireAble> FiredItems
        {
            get { return firedItems; } 
        }


        public void Fire(Vector3 direction)
        {
            firedItems.Add(mCurrentShip.Fire(missile.ShallowCopy(), direction));  
        }

        public void Update(GameTime gTime)
        {
            for (int num = 0; num < firedItems.Count; num++ ) 
            { 
                firedItems[num].Update(gTime);
                if (firedItems[num].Position.Y < SpaceShip.maxHeight.Y ||
                    firedItems[num].Position.Y > SpaceShip.maxDepth.Y)
                {
                    firedItems.RemoveAt(num); 
                }
            } 
        }

        public void Draw(SpriteBatch theSpriteBatch)
        { 
            foreach (IFireAble firedItem in firedItems)
            {
                firedItem.Draw(theSpriteBatch);
            }
        }

        public bool CheckCollision(IEnumerable<IFireAble> shells)
        {
            List<IFireAble> shellsList = (List<IFireAble>)shells; 

            for (int num = 0; num < shellsList.Count; num++)
            {
                if (mCurrentShip.CheckCollision(shellsList[num].Sphere))
                {
                    shellsList.RemoveAt(num);
                    return true; 
                }
            }
            return false; 
        }

        public void RemoveItem(int index)
        {
            try
            {
                firedItems.RemoveAt(index);
            }
            catch (ArgumentOutOfRangeException e)
            {
                //do nothing...
            }
        }
    }
}