using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.Xna.Framework;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using Microsoft.Xna.Framework.Storage;
using System.Runtime.Serialization;


namespace _1001823_XNA_MiniProject6
{
    class FileManager
    {
        private static FileManager instance = null;
        private static object myLocker = new object();

        private StorageDevice storageDevice;  

        private FileManager()
        {
            init(); 
        }

        //just trying a new style...
        //from seppo
        public static FileManager GetFileManager
        {
            get
            {
                lock (myLocker)
                {
                    if (instance == null)
                    {
                        instance = new FileManager();
                    }
                    return instance;
                }
            }
        }

        public void init()
        {
            IAsyncResult IResult = null;

            IResult = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);
            storageDevice = StorageDevice.EndShowSelector(IResult); 
        }

        public String Location
        {
            get;
            set; 
        }


        public void SaveData(PlayerData data, String location, String filename)
        {
            if (location == null)
                location = this.Location; 

            IAsyncResult IResult = storageDevice.BeginOpenContainer(location, null, null);
            IResult.AsyncWaitHandle.WaitOne();
            StorageContainer container = storageDevice.EndOpenContainer(IResult);

            IResult.AsyncWaitHandle.Close();

            if (container.FileExists(filename))
                container.DeleteFile(filename);

            Stream stream = container.CreateFile(filename);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PlayerData)); 
            xmlSerializer.Serialize(stream, data); 

            stream.Close();

            container.Dispose(); 
        }


        public PlayerData ReadFromFile(String location, String filename)
        {
            PlayerData data = new PlayerData(); 
            IAsyncResult result = storageDevice.BeginOpenContainer(location, null, null);
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = storageDevice.EndOpenContainer(result);

            result.AsyncWaitHandle.Close();

            if (container.FileExists(filename))
                return null; 

            Stream stream = container.OpenFile(filename, FileMode.Open);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PlayerData));

            data = (PlayerData)xmlSerializer.Deserialize(stream);

            stream.Close();
            container.Dispose(); 
            
            return data;  
        }
    }
}
