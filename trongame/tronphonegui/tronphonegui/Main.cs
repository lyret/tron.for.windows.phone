using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using TronLogic;

namespace TronPhoneGui
{
    public class Main : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        Texture2D wallpaperTexture;
        Rectangle wallpaperRectangle;
        Texture2D basicTexture;

        Texture2D leftButtonTexture;
        Texture2D rightButtonTexture;
        Texture2D plusButtonTexture;
        Texture2D minusButtonTexture;

        Rectangle leftButtonRectangle;
        Rectangle rightButtonRectangle;
        Rectangle plusButtonRectangle;
        Rectangle minusButtonRectangle;

        // Color options
        Color bluePlayerColor = new Color(142,213,225);
        Color blueWallColor = new Color(89,122,129);
        Color yellowPlayerColor = new Color(255,215,51);
        Color yellowWallColor = new Color(99,81,37);

        // Buttons options
        private const int buttonSize = 74;
        private const int rightButtonX = 577;
        private const int rightButtonY = 103;
        private const int leftButtonX  = 577;
        private const int leftButtonY  = 300;
        private const int plusButtonX = 480;
        private const int plusButtonY = 202;
        private const int minusButtonX = 675;
        private const int minusButtonY = 202;

        // Grid options
        private const int gridCells = 27;
        private const int cellSize = 16;
        private const int gridSize = 16 * gridCells;
        private const int gridOffset = 24;

        // Game Logic and Time
        LogicInterface logic;
        private bool leftTurnFlag;
        private bool rightTurnFlag;
        private bool speedUpFlag;
        private bool speedDownFlag;
        private int updateTimer;
        private int logicPace;
        private int logicTimer;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(666666);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
        }


        /*
         * Logic Initalization
         */
        protected override void Initialize()
        {
            base.Initialize();

            // Set drawing positions
            wallpaperRectangle = new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
            rightButtonRectangle = new Rectangle(rightButtonX, rightButtonY, buttonSize, buttonSize);
            minusButtonRectangle = new Rectangle(minusButtonX, minusButtonY, buttonSize, buttonSize);
            leftButtonRectangle = new Rectangle(leftButtonX, leftButtonY, buttonSize, buttonSize);
            plusButtonRectangle = new Rectangle(plusButtonX, plusButtonY, buttonSize, buttonSize);

            // Create and initialize the Game Grid and between time
            logic = new LogicInterface(gridCells, cellSize);
            leftTurnFlag = false;
            rightTurnFlag = false;
            speedUpFlag = false;
            speedDownFlag = false;
            updateTimer = 0;
            logicPace = cellSize / logic.getMaxPace();
            logicTimer = 0;
        }

        
        /*
         * Logic loop
         */
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Touch events
            TouchCollection touchCollection = TouchPanel.GetState();
            foreach (TouchLocation touch in touchCollection)
            {
                if (touch.State == TouchLocationState.Pressed || touch.State == TouchLocationState.Moved)
                {
                    // Turn left
                    if (buttonIsPressed(leftButtonRectangle, touch))
                        leftTurnFlag = true;
                    // Turn right
                    else if (buttonIsPressed(rightButtonRectangle, touch))
                        rightTurnFlag = true;
                    // Speed up
                    if (buttonIsPressed(plusButtonRectangle, touch))
                        speedUpFlag = true;
                    // Slow down
                    else if (buttonIsPressed(minusButtonRectangle, touch))
                        speedDownFlag = true;
                }
            }

            // Handle time and game speed
            updateTimer = (updateTimer + 1) % cellSize;
            logicTimer = 0;// updateTimer % logicPace;

            // This speeds up the feel of the game as it gets harder
            this.TargetElapsedTime = TimeSpan.FromTicks(Math.Max(100000, 300000 - 50000 * logic.getLevel()));

            // Update the logic
            if (logicTimer == 0)
            {
                logic.update(leftTurnFlag, rightTurnFlag, speedUpFlag, speedDownFlag);

                leftTurnFlag = false;
                rightTurnFlag = false;
                speedUpFlag = false;
                speedDownFlag = false;
            }
        }

        // Returns true if a touch event is within a rectangle
        private bool buttonIsPressed(Rectangle rectangle, TouchLocation l)
        {
            int x = (int) l.Position.X;
            int y = (int) l.Position.Y;
            
            return (x >= rectangle.X
                    && y >= rectangle.Y
                    && x <= (rectangle.X + rectangle.Width)
                    && y <= (rectangle.Y + rectangle.Height)
                    );
        }


        /* 
         * Draw Loop
         */
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // Clear the screen
            GraphicsDevice.Clear(Color.Black);

            // Draw the wallpaper
            spriteBatch.Begin();
            spriteBatch.Draw(wallpaperTexture, wallpaperRectangle, Color.White);
            spriteBatch.End();

            // Draw walls
            spriteBatch.Begin();
            TronLogic.Wall[,] matrix = logic.getWalls();
            int matrixSize = matrix.GetLength(0);

            for ( int i = 0; i < matrixSize; i++ )
                for (int j = 0; j < matrixSize; j++)
                {
                    if (matrix[i, j] == null) continue;

                    spriteBatch.Draw(basicTexture,
                    new Rectangle(
                        gridOffset + (i * cellSize),
                        gridOffset + (j * cellSize),
                        cellSize,
                        cellSize),
                    getColor(matrix[i,j]));
                }
            spriteBatch.End();

            // Draw players
            spriteBatch.Begin();
            foreach (TronLogic.Player player in logic.getPlayers()) {
                drawPlayer(player);
            }
            spriteBatch.End();

            // Draw buttons
            spriteBatch.Begin();
            spriteBatch.Draw(leftButtonTexture, leftButtonRectangle, Color.White);
            spriteBatch.Draw(rightButtonTexture, rightButtonRectangle, Color.White);
            spriteBatch.Draw(plusButtonTexture, plusButtonRectangle, Color.White);
            spriteBatch.Draw(minusButtonTexture, minusButtonRectangle, Color.White);
            spriteBatch.End();
        }

        // Draws a player's bike
        private void drawPlayer(TronLogic.Player player)
        {
            int offset = player.updateTimer / (int) player.speed;
            int x = player.x * cellSize;
            int y = player.y * cellSize;

            //Drawing offset depends on the players direction
            if (player.direction == Player.Direction.Up)
                y -= offset;
            else if (player.direction == Player.Direction.Down)
                y += offset;
            else if (player.direction == Player.Direction.Left)
                x -= offset;
            else if (player.direction == Player.Direction.Right)
                x += offset;

            spriteBatch.Draw(basicTexture,
                new Rectangle(
                    gridOffset + x,
                    gridOffset + y,
                    cellSize,
                    cellSize),
                getColor(player));
        }

        // Takes a Tron Player Color and returns the correct xna color
        private Color getColor(TronLogic.Player player)
        {
            if (player.color == TronLogic.Player.Color.Blue)
                return bluePlayerColor;
            if (player.color == TronLogic.Player.Color.Yellow)
                return yellowPlayerColor;

            return Color.White;
        }

        // Takes a wall and returns the correct xna color
        private Color getColor(TronLogic.Wall wall)
        {
            if (wall.color == TronLogic.Player.Color.Blue)
                return blueWallColor;
            if (wall.color == TronLogic.Player.Color.Yellow)
                return yellowWallColor;

            return Color.White;
        }


        /*
         * Loading of resources
         */
        protected override void LoadContent()
        {
            base.LoadContent();

            spriteBatch = new SpriteBatch(GraphicsDevice);
            basicTexture = new Texture2D(GraphicsDevice, 1, 1);
            basicTexture.SetData(new Color[] { Color.White });

            wallpaperTexture = Content.Load<Texture2D>("wallpaper");
            
            leftButtonTexture = Content.Load<Texture2D>("button_left");
            rightButtonTexture = Content.Load<Texture2D>("button_right");
            plusButtonTexture = Content.Load<Texture2D>("button_plus");
            minusButtonTexture = Content.Load<Texture2D>("button_minus");
        }


        /*
         * Unloading of resources (that are not loaded from content)
         */
        protected override void UnloadContent()
        {
            base.UnloadContent();

            spriteBatch.Dispose();
            basicTexture.Dispose();
        }
    }
}
