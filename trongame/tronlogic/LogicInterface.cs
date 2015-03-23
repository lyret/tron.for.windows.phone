using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace TronLogic
{
    /*
     * CLASS: LogicInterface
     * Allows the logical part of the game to be accessed from externaly,
     * contains all neccessary methods to control the logic.
     */
    public class LogicInterface
    {
        private GameGrid grid;

        // About enumurations: 
		// Se The Player class for possible values on colors, speed and directions


        /*
         * TronGame's logical constructor
         * Creates a new game with set options:
         * gridSize, the number of cells in the game grid
         * gamePace, an arbitary number that controlls how slowly the logic will update,
         *   should be compitable with the set player speeds.
         */
        public LogicInterface(int gridSize = 27, int gamePace = 16)
        {
            grid = new GameGrid(gridSize, gamePace);
        }

        /*
         * update
         * updates the game grid, adjusting the users speed and possibly turning him in a new direction
         */
		public void update(bool turnLeft = false, bool turnRight = false, bool speedUp = false, bool speedDown = false)
        {
            if (turnLeft) {
                grid.user.turnLeft = true;
                grid.user.turnRight = false;
            }
                
            if (turnRight) {
                grid.user.turnRight = true;
                grid.user.turnLeft = false;
            }
            
			if (speedDown)
				grid.user.speed = Player.Speed.Slow;
			else if (speedUp)
				grid.user.speed = Player.Speed.Fast;
			else
				grid.user.speed = Player.Speed.Normal;

            grid.update();
        }

        /*
        * getGamePace
        * returns the maximum pace of the game (fastes speed of any player)
        */
        public int getMaxPace()
        {
            return (int)Player.Speed.Slow;
        }

        /*
         * getlevel
         * returns the level of the game grid
         */
        public int getLevel() {
            return grid.level;
        }
        
        /* 
         * getWalls
         * gets the wall matrix from the GameGrid
         */
        public Wall[,] getWalls()
        {
            return grid.matrix;
        }


        /*
         * getPlayers
         * gets a list of all LIVING players
         */
        public List<Player> getPlayers()
        {
            return grid.livingPlayers;
        }
    }
}
