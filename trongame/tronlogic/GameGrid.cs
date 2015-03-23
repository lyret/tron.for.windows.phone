using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace TronLogic
{
    /*
     * CLASS: GameGrid
     * Runs game after game after game until the
     * user quits the application
     */
    public class GameGrid
    {
        public int level;
        public List<Player> livingPlayers;
        public Wall[,] matrix;
        public User user;
        
        private int size;
        private int gamePace;
        private Enemy[] enemies;

        /*
         * GameGrid Constructor
         * The Grid keeps tracks of walls, players, time and game results
         */
        public GameGrid(int gridSize, int gamePace)
        {
            this.size = gridSize;
            this.gamePace = gamePace;
            level = 1;
            reset();
        }

        /*
         * Update
         */
        public void update()
        {
            // Update all players
            foreach (Player player in livingPlayers)
            {
                // Handle speed
                player.updateTimer += 1;

                if (player.updateTimer >= gamePace * (int)player.speed)
                {
                    player.updateTimer = 0;

                    // Move the player and place trailing walls
                    if (matrix[player.x, player.y] == null)
                        matrix[player.x, player.y] = new Wall(player.color);

                    player.move();
                    player.setDirection(matrix, user.direction);

                    // Check if the enemy has moved into a wall or of the edge of the map
                    if (player.x < 0 || player.x >= size || player.y < 0 || player.y >= size
                        || matrix[player.x, player.y] != null)
                    {
                        killPlayer(player);
                    }
                }
            }
        }


        /*
         * Victory
         * Increase the level and resets the game
         */
        private void victory()
        {
            level++;
            reset();
        }

        /*
         * Game Over
         * Reset the level to 1 and resets the game
         */
        private void gameOver()
        {
            level = 1;
            reset();
        }

        /*
         * Reset
         * restarts the Game Grid with user and enemies
         */
        private void reset()
        {
            // Reset the matrix
            matrix = new Wall[size, size];

            // Load a new blue player
            user = new User(size / 2, size-1 - 2, Player.Direction.Up);

            // Load enemies
            enemies = new Enemy[level];

            for(int i = 0; i < level; i++)
            {
                enemies[i] = new Enemy(size / 2 + level / 2 - i, 2, Player.Direction.Down);
            }
            
            // Set living players
            livingPlayers = getLivingPlayers();
        }
        
        /*
         * killPlayer
         * kills a player and recaculate currently alive players
         */
         private void killPlayer(Player player)
         {
             player.alive = false;
             livingPlayers = getLivingPlayers();
             
             // If the user has crashed
            if (player == user)
                gameOver();
             
            // Check user win-condition
            else if (livingPlayers.Count <= 1)
                victory();
         }
        
        /*
         * getLivingPlayers
         * creates and returns a list of all currently living players
         */
         private List<Player> getLivingPlayers()
         {
             
            List<Player> list = new List<Player>();

            foreach (Enemy enemy in enemies)
                if (enemy.alive)
                    list.Add(enemy);

            list.Add(user);

            return list;
         }
        
        
    }
}
