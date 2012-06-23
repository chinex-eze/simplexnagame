using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics; 
using Microsoft.Xna.Framework.Content; 
using Microsoft.Xna.Framework;  

namespace _1001823_XNA_MiniProject6
{
    class HUDisplay
    {
        private SpriteFont mFont { get; set; }  
        private Texture2D mRectangleBG;

        public String mScore { get; set; }
        public String mLevel { get; set; }
        public String mLives { get; set; }  

        public HUDisplay()
        {
            mScore = "0";
            mLevel = "0";
            mLives = "0"; 
        }

        public void Load(ContentManager theContentManager, GraphicsDevice device, String theAssetName)
        {
            mRectangleBG = new Texture2D(device, 1, 1); 
            mRectangleBG.SetData(new[] { Color.White }); 
            mFont = theContentManager.Load<SpriteFont>(theAssetName);  
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(mRectangleBG, new Rectangle(10, 20, 200, 70), Color.LightGoldenrodYellow);

            theSpriteBatch.DrawString(mFont, "Score: " + mScore, new Vector2(12, 20), Color.Black);
            theSpriteBatch.DrawString(mFont, "Level: " + mLevel, new Vector2(12, 40), Color.Black);
            theSpriteBatch.DrawString(mFont, "Lives: " + mLives, new Vector2(12, 60), Color.Black);  
        }
    }
}
