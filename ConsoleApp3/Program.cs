using System;

namespace GemHuntersGame
{
    class Position
    {
        public int X { get; }
        public int Y { get; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    class Player
    {
        public string Name { get; }
        public Position Position { get; set; }
        public int GemCount { get; private set; }

        public Player(string name, int x, int y)
        {
            Name = name;
            Position = new Position(x, y);
            GemCount = 0;
        }

        public void Move(char direction)
        {
            switch (direction)
            {
                case 'U':
                    Position = new Position(Position.X, Position.Y - 1);
                    break;
                case 'D':
                    Position = new Position(Position.X, Position.Y + 1);
                    break;
                case 'L':
                    Position = new Position(Position.X - 1, Position.Y);
                    break;
                case 'R':
                    Position = new Position(Position.X + 1, Position.Y);
                    break;
            }
        }

        public void CollectGem()
        {
            GemCount++;
        }
    }

    class Board
    {
        private char[,] _grid;

        public Board()
        {
            _grid = new char[6, 6];
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            // Initialize with empty cells
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    _grid[i, j] = '-';
                }
            }

            // Place players
            _grid[0, 0] = 'P';
            _grid[5, 5] = 'P';

            // Place gems (randomly)
            Random random = new Random();
            for (int i = 0; i < 5; i++)
            {
                int x = random.Next(6);
                int y = random.Next(6);
                if (_grid[y, x] == '-')
                    _grid[y, x] = 'G';
            }

            // Place obstacles (randomly)
            for (int i = 0; i < 5; i++)
            {
                int x = random.Next(6);
                int y = random.Next(6);
                if (_grid[y, x] == '-')
                    _grid[y, x] = 'O';
            }
        }

        public void Display()
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    Console.Write(_grid[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        public bool IsValidMove(Player player, char direction)
        {
            int newX = player.Position.X;
            int newY = player.Position.Y;

            switch (direction)
            {
                case 'U':
                    newY--;
                    break;
                case 'D':
                    newY++;
                    break; 
                case 'L':
                    newX--;
                    break;
                case 'R':
                    newX++;
                    break;
            }

            if (newX >= 0 && newX < 6 && newY >= 0 && newY < 6)
            {
                return _grid[newY, newX] != 'O';
            }
            return false;
        }

        public void CollectGem(Player player)
        {
            if (_grid[player.Position.Y, player.Position.X] == 'G')
            {
                player.CollectGem();
                _grid[player.Position.Y, player.Position.X] = '-';
            }
        }
    }

    class Game
    {
        private Player _player1;
        private Player _player2;
        private Board _board;
        private Player _currentPlayer;
        private int _totalTurns;

        public Game()
        {
            _player1 = new Player("P1", 0, 0);
            _player2 = new Player("P2", 5, 5);
            _board = new Board();
            _currentPlayer = _player1;
            _totalTurns = 0;
        }

        public void Start()
        {
            while (_totalTurns <=15)
            {
                _board.Display();
                Console.WriteLine($"It's {_currentPlayer.Name}'s turn.");
                Console.Write("Enter direction (U/D/L/R): ");
                char direction = Char.ToUpper(Console.ReadKey().KeyChar);
                Console.WriteLine();

                if (_board.IsValidMove(_currentPlayer, direction))
                {
                    _currentPlayer.Move(direction);
                    _board.CollectGem(_currentPlayer);
                    SwitchTurn();
                    _totalTurns++;
                }
                else
                {
                    Console.WriteLine("Invalid move. Try again.");
                }
            }

            AnnounceWinner();
        }

        private void SwitchTurn()
        {
            _currentPlayer = _currentPlayer == _player1 ? _player2 : _player1;
        }

        private void AnnounceWinner()
        {
            Console.WriteLine("Game over!");
            if (_player1.GemCount > _player2.GemCount)
            {
                Console.WriteLine($"Player 1 wins with {_player1.GemCount} gems!");
            }
            else if (_player1.GemCount < _player2.GemCount)
            {
                Console.WriteLine($"Player 2 wins with {_player2.GemCount} gems!");
            }
            else
            {
                Console.WriteLine("It's a tie!");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
        }
    }
}
