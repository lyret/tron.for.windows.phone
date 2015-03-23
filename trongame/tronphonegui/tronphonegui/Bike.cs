using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TronPhoneGui
{
    /* 
     * Helper class for Main.cs that keeps track of a logical
     * player and draws it moving smoothly over the grid as long as it's alive
     */
    class Bike
    {
        private int x;
        private int y;
        private int targetX;
        private int targetY;
        private int pixelsPerUpdate;

        /*
         * Constructor
         */
        public Bike(TronLogic.Player player, int cellSize)
        {
            setPosition(player.x, player.y);
            setTarget(x, y);

            int speed = cellSize / (int)player.speed;
            setSpeed(speed);
        }

        // Set this bikes position
        public void setPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        // Set this bikes target position
        public void setTarget(int x, int y)
        {
            this.targetX = x;
            this.targetY = y;
        }

        // Set this bikes speed, expressed as pixel per update
        public void setSpeed(int pixelsPerUpdate)
        {
            this.pixelsPerUpdate = pixelsPerUpdate;
        }

        // Update this bikes position using the target values and given speed
        public void updatePosition()
        {
            // Adjust for incompitability between the distance and speed
            if ((x < targetX && pixelsPerUpdate < (targetX - x)) ||
                (x > targetX && pixelsPerUpdate < (x - targetX))) {
                x = targetX;
            }
            if ((y < targetY && pixelsPerUpdate < (targetY - y)) ||
                (y > targetY && pixelsPerUpdate < (y - targetY))) {
                y = targetY;
            }

            // "Move"
            if (x < targetX)
                x += pixelsPerUpdate;
            else if (x > targetX)
                x -= pixelsPerUpdate;

            if (y < targetY)
                y += pixelsPerUpdate;
            else if (y > targetY)
                y -= pixelsPerUpdate;
        }
    }
}
