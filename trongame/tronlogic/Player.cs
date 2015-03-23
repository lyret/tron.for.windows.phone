using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TronLogic
{
    /*
     * CLASS: Player
     * Abstract class for both the user and enemies
     * contains common attributes and methods
     *
     * 
     */
    public abstract class Player
    {
        // Enumeration of directions
        public enum Direction { Up, Right, Down, Left };

        // Enumeration of colors
        public enum Color { Blue, Yellow };

		// Enumeration of speed
		public enum Speed { Slow=4, Normal=2, Fast=1 };
        
        // Common variables for all players
        public Direction direction;
        public Color color;
        public Speed speed;
        public int x;
        public int y;
		public int updateTimer = 0;
        public bool alive = true;

        // Requried methods
        
        /*
         * SetDirection
         * the grid will sends the current walls and the users direction to each player
         */
        public abstract void setDirection(Wall[,] matrix, Player.Direction userDirection);
        
        // Common methods
        
		/*
         * Move
         * transports the player in it's direction
         */
		public void move()
        {
			if (!alive) return;

            switch (direction)
            {
                case Direction.Up:
                    y--;
                    break;
                case Direction.Right:
                    x++;
                    break;
                case Direction.Down:
                    y++;
                    break;
                case Direction.Left:
                    x--;
                    break;
            }

        }
    }
}
