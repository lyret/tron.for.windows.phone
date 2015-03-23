using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TronLogic
{
    /*
     * CLASS: Wall
     * Represents a 'Light Wall' blocking the path for players on the grid
     */
    public class Wall
    {
        public Player.Color color;

        /*
         * Wall Constructor
         * Creates a user with a player color
         */
        public Wall(Player.Color c)
        {
            color = c;
        }
    }
}
