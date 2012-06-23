using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace _1001823_XNA_MiniProject6
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteBG background;
        HUDisplay display; 
        SpaceShipManager playerManager;
        AIPlayer enemy; 
        KeyboardState prevKeyboardState;  
        MouseState prevState;

        SoundFX2 BGsound;
        BasicSoundFX missileSound;

        public static GraphicsDevice GDevice; 

        int hits = 0; //for testing purposes 
        PlayerData pData;
        double tmpTime = 0; 

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content"; 
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here 
            background = new SpriteBG();
            display = new HUDisplay();
            playerManager = new SpaceShipManager(); 
            enemy = new AIPlayer();
            enemy.ShipManager = new SpaceShipManager();
            pData = new PlayerData(); 

            //prevKeyboardState = Keyboard.GetState(); 
            prevState = Mouse.GetState(); 
            this.IsMouseVisible = true;

            base.Initialize(); GDevice = GraphicsDevice; 
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here 
            background.Load(GraphicsDevice, this.Content, "Images/StarNight");
            display.Load(this.Content, GraphicsDevice, "Fonts/SpriteFont1"); 
            playerManager.Load(this.Content, GraphicsDevice, "Models/Ship", SpaceShipManager.BOTTOM);
            playerManager.LoadArms(this.Content, "Images/missile");

            enemy.ShipManager.Load(this.Content, GraphicsDevice, "Models/Ship", SpaceShipManager.TOP); 
            enemy.ShipManager.LoadArms(this.Content, "Images/missile1");
            enemy.InitShip();

            BGsound = new SoundFX2(this.Content, "Sounds/game_GB_sound");
            BGsound.Loop = true; 
            BGsound.Play();

            missileSound = new BasicSoundFX(this.Content, "Sounds/missile_sound"); 
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here 
            background.Update(gameTime); 

            playerManager.Update(gameTime); 
            enemy.Update(gameTime);

            //if(enemy.ShipManager.CheckCollision(playerManager.FiredItems))
            //if (playerManager.CheckCollision(enemy.ShipManager.FiredItems )) 
            if(enemy.ShipManager.CurrentShip.CheckCollision(playerManager.CurrentShip.Sphere)) 
            {
                hits += 1;
            }
            display.mScore = hits + ""; 

            HandleMouse(Mouse.GetState());

            handleKeyboard(Keyboard.GetState()); 

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here 
            spriteBatch.Begin(); 
                background.Draw(spriteBatch);
                display.Draw(spriteBatch);
                playerManager.Draw(spriteBatch);  
                enemy.Draw(spriteBatch);  
            spriteBatch.End();

            ///these has to be drawn outside cos they can't be drawn inside a spritebatch.begin() 
            playerManager.CurrentShip.Draw(); 
            enemy.ShipManager.CurrentShip.Draw(); 

            base.Draw(gameTime);
        }


        private void handleKeyboard(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.Left) == true)
            {
                playerManager.CurrentShip.Move(SpaceShip.LEFT);

                if (prevKeyboardState.IsKeyDown(Keys.Left) == false)
                {

                }
            }
            else if (keyboardState.IsKeyDown(Keys.Right) == true)
            {
                playerManager.CurrentShip.Move(SpaceShip.RIGHT);

                if (prevKeyboardState.IsKeyDown(Keys.Right) == false)
                {

                }
            }
            else if (keyboardState.IsKeyDown(Keys.Up) == true)
            {
                playerManager.CurrentShip.Move(SpaceShip.DOWN); 
            }
            else if (keyboardState.IsKeyDown(Keys.Down) == true)
            {
                playerManager.CurrentShip.Move(SpaceShip.UP); 
            }
            else if (keyboardState.IsKeyDown(Keys.F) == true)
            {
                if (prevKeyboardState.IsKeyDown(Keys.F) == false)
                {
                    playerManager.Fire(SpaceShip.UP); 
                    missileSound.Play();
                }
            }
            else if (keyboardState.IsKeyDown(Keys.S) == true)
            {
                if (prevKeyboardState.IsKeyDown(Keys.S) == false)
                {
                    FileManager.GetFileManager.SaveData(pData, "GameData", "PlayerData.xml"); 
                }
            }
            else if (keyboardState.IsKeyDown(Keys.L) == true)
            {
                if (prevKeyboardState.IsKeyDown(Keys.L) == false)
                {
                    pData = FileManager.GetFileManager.ReadFromFile("GameData", "PlayerData.xml");
                }
            }
            else if (keyboardState.IsKeyDown(Keys.U) == true)
            {
                if (prevKeyboardState.IsKeyDown(Keys.U) == false)
                {
                    BGsound.Volume(SoundFX2.VOLUMEUP);
                }
            }
            else if (keyboardState.IsKeyDown(Keys.D) == true)
            {
                if (prevKeyboardState.IsKeyDown(Keys.D) == false)
                {
                    BGsound.Volume(SoundFX2.VOLUMEDOWN);
                }
            }
            else if (keyboardState.IsKeyDown(Keys.Space) == true)
            {
                if (prevKeyboardState.IsKeyDown(Keys.Space) == false)
                {
                    if (BGsound.IsPlaying())
                    {
                        BGsound.Stop();  
                    }
                    else
                    {
                        BGsound.Play();  
                    }
                }
            }
            else if (keyboardState.IsKeyDown(Keys.Escape) == true)
            {
                FileManager.GetFileManager.SaveData(pData, "GameData", "PlayerData.xml");  
                this.Exit(); 
            }
            

            prevKeyboardState = keyboardState; 
        }


        public void HandleMouse(MouseState curState)
        {
            if (curState.LeftButton == ButtonState.Pressed &&
                    prevState.LeftButton == ButtonState.Released)
            {
                
            }

            prevState = curState;
        }
    }
}
