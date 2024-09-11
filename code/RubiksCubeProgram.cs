using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace RubiksCube
{

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class RubiksCubeProgram : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Model model;
        SpriteFont font, bigFont;
        Texture2D menuBox, highlightedMenuBox;
        VisualCube myVisualCube;
        Menu menu = new Menu();
        Menu subMenu = new Menu();
        Timer timer = new Timer();

        KeyboardState previousKey, currentKey;
        MouseState mouseState;

        Vector3 cameraPosition = new Vector3(0, 0, 20);
        Vector3 cameraLookAtVector = Vector3.Zero;
        Vector3 cameraUpVector = Vector3.UnitY;

        string timerMessage = "";
        float nearClipPlane = 1;
        float farClipPlane = 200;
        float fieldOfView = MathHelper.PiOver4;


        public RubiksCubeProgram()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        
        protected override void Initialize()
        {
            //keeps mouse visible on screen
            this.IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            menuBox = Content.Load<Texture2D>("menuBox");
            highlightedMenuBox = Content.Load<Texture2D>("highlightedMenuBox");

            menu.box = menuBox;
            menu.highligtedBox = highlightedMenuBox;

            menu.menuItemList.Add(new MenuItem(10, 10, 120, 40, "cube"));
            menu.menuItemList.Add(new MenuItem(10, 50, 120, 40, "timer"));
            menu.menuItemList.Add(new MenuItem(10, 90, 120, 40, "exit"));
            menu.menuItemList[0].selected = true;

            subMenu.box = menuBox;
            subMenu.highligtedBox = highlightedMenuBox;

            subMenu.menuItemList.Add(new MenuItem(10, 140, 120, 40, "scramble"));
            subMenu.menuItemList.Add(new MenuItem(10, 180, 120, 40, "solve"));
            subMenu.menuItemList.Add(new MenuItem(10, 220, 120, 40, "reset"));


            model = Content.Load<Model>("TexturedCube");  //loads Textured cube FBX
            font = Content.Load<SpriteFont>("onScreenText");
            bigFont = Content.Load<SpriteFont>("timerText");  //Loads spritefont text file

            myVisualCube = new VisualCube(subMenu);
            myVisualCube.createQBs(model);
            myVisualCube.createRotations();
            
            foreach (var mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.View = Matrix.CreateLookAt(cameraPosition, cameraLookAtVector, cameraUpVector);
                    float aspectRatio = graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClipPlane, farClipPlane);
                }
            }
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) || menu.menuItemList[2].selected == true)
                Exit();

            previousKey = currentKey;
            currentKey = Keyboard.GetState();
            mouseState = Mouse.GetState();

            if (menu.menuItemList[0].selected == true)
            {
                myVisualCube.Update(previousKey, currentKey, mouseState);
            }
            if (menu.menuItemList[1].selected == true)
            {
                timerMessage = timer.update(currentKey, previousKey);
            }
            else
            {
                timerMessage = "";
            }
            menu.update(mouseState);

            base.Update(gameTime); 
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.PaleVioletRed);

            spriteBatch.Begin();

            spriteBatch.DrawString(bigFont, timerMessage, new Vector2(245, 195), Color.White);
            menu.draw(spriteBatch, font);

            spriteBatch.End();

            //Stops the cube being see through 
            DepthStencilState dss = new DepthStencilState();
            dss.DepthBufferEnable = true;
            GraphicsDevice.DepthStencilState = dss;


            if (menu.menuItemList[0].selected == true)
            {
                myVisualCube.Draw(graphics, spriteBatch, font);
            }
            
            base.Draw(gameTime);
        }

        
    }
}