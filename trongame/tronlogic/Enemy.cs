using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TronLogic
{
   /*
    * CLASS: Enemy
    * Represents a computer controlled enemy
    */
    public class Enemy : Player
    {
        // Variable controlling each enemies sight radius
        private const int threshold = 3;

        /*
         * Enemy Constructor
         * Creates a enemy with a position and direction
         */
        public Enemy(int x, int y, Direction dir)
        {
            this.color = Color.Yellow;
            this.x = x;
            this.y = y;
            this.direction = dir;
            this.alive = true;
			this.speed = Speed.Normal;
        }


        /*
         * SetDirection
         * the ai player is given access to the gamegrid to calculate it's next direction, they are also
         * given the users current direction so they can try to match it.
         */
        public override void setDirection(Wall[,] matrix, Player.Direction userDirection)
        {
            // Set the users direction as the initially best one
            Player.Direction bestDirection = userDirection;
            int bestDistance = getDistance(userDirection, matrix);
            

            //Change direction only when the best direction is bad
            if (bestDistance <= threshold)
            {
                // Test all directions and set the best one
                Player.Direction nextDirection = Player.Direction.Up;
                int nextDistance = getDistance(nextDirection, matrix);
                if (nextDistance > bestDistance)
                    bestDirection = nextDirection;

                nextDirection = Player.Direction.Right;
                nextDistance = getDistance(nextDirection, matrix);
                if (nextDistance > bestDistance)
                    bestDirection = nextDirection;

                nextDirection = Player.Direction.Down;
                nextDistance = getDistance(nextDirection, matrix);
                if (nextDistance > bestDistance)
                    bestDirection = nextDirection;

                nextDirection = Player.Direction.Left;
                nextDistance = getDistance(nextDirection, matrix);
                if (nextDistance > bestDistance)
                    bestDirection = nextDirection;
            }
            // Change direction to the best one if close to hitting a wall
            if (getDistance(this.direction, matrix) <= threshold)
                this.direction = bestDirection;
        }

        // Gets the distance from this players location to the nearest wall in a driection
        private int getDistance(Player.Direction dir, Wall[,] matrix)
        {
            int distance = 0;

            if (dir == Player.Direction.Left)
                for (int i = this.x - 1; i >= 0; i--)
                {
                    if (matrix[i, this.y] == null)
                        distance++;
                    else
                        return distance;
                }
            else if (dir == Player.Direction.Right)
                for (int i = this.x + 1; i < matrix.GetLength(0); i++)
                {
                    if (matrix[i, this.y] == null)
                        distance++;
                    else
                        return distance;
                }
            else if (dir == Player.Direction.Up)
                for (int i = this.y - 1; i >= 0; i--)
                {
                    if (matrix[this.x, i] == null)
                        distance++;
                    else
                        return distance;
                }
            else if (dir == Player.Direction.Down)
                for (int i = this.y + 1; i < matrix.GetLength(0); i++)
                {
                    if (matrix[this.x, i] == null)
                        distance++;
                    else
                        return distance;
                }

            return distance;
        }
    }
}
