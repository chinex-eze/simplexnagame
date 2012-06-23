using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using Microsoft.Xna.Framework.Storage;  

namespace _1001823_XNA_MiniProject6
{
    [Serializable()] 
    public class PlayerData
    {
        public PlayerData()
        {
            //the default values  
            Scores = 0;
            Lives = 3;
            Level = 1; 
        }

        public int Scores
        {
            get;
            set;
        }

        public int Lives
        {
            get;
            set; 
        }

        public int Level
        {
            get;
            set; 
        }
    }
}
