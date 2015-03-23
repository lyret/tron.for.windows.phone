using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TronLogic
{
    /*
     * CLASS: User
     * Represents a User-controlled player.
     */
    public class User : Player
    {
        public bool turnLeft;
        public bool turnRight;
        
        /*
         * User Constructor
         * Creates a user with a position and direction
         */
        public User(int x, int y, Direction dir)
        {
            this.color = Color.Blue;
            this.x = x;
            this.y = y;
            this.direction = dir;
			this.speed = Speed.Normal;
            
            this.turnLeft = false;
            this.turnRight = false;
        }
        
        /*
         * SetDirection
         * The user will ignore the information from the grid but use this method to change it's direction
         * Left has priority
         */
        public override void setDirection(Wall[,] matrix, Player.Direction userDirection)
        {
            if (turnLeft) {
                turnCounterClockwise();
            }
            else if (turnRight) {
                turnClockwise();
            }
            
            turnLeft = false;
            turnRight = false;
        }

        
        /* 
         * turnClockwise
         * Turns the player
         */
        private void turnClockwise()
        {
            int dir = (int) direction;
            dir = (dir + 1) % 4;

            direction = (Player.Direction)dir;
        }

        /* 
         * turnCounterClockwise
         * Turns the player
         */
        private void turnCounterClockwise()
        {
            int dir = (int) direction;

            if (dir == 0)
                dir = 3;
            else
                dir = dir - 1;

            direction = (Player.Direction)dir;
        }
    }
}
