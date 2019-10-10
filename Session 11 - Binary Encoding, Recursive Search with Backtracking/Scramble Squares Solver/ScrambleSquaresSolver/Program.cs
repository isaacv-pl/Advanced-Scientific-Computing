// Solves 3 x 3 "PUPPIES" Scramble Squares puzzle


using System;
using System.Diagnostics;

namespace ScrambleSquaresSolver
{

    class Tile
    {
        public int Id;
        public int Rotation;
        public bool Placed = false;
        public int[,] Bindings = new int[4, 4];

        public Tile(int id, int north, int east, int south, int west)
        {
            Id = id;

            // Rotation = 0, no turn = original orientation
            Bindings[0, 0] = north;
            Bindings[0, 1] = east;
            Bindings[0, 2] = south;
            Bindings[0, 3] = west;

            // Rotation = 1, one quarter turn counterclockwise
            Bindings[1, 0] = east;
            Bindings[1, 1] = south;
            Bindings[1, 2] = west;
            Bindings[1, 3] = north;

            // Rotation = 2, two quarter turns counterclockwise
            Bindings[2, 0] = south;
            Bindings[2, 1] = west;
            Bindings[2, 2] = north;
            Bindings[2, 3] = east;

            // Rotation = 3, three quarter turns counterclockwise
            Bindings[3, 0] = west;
            Bindings[3, 1] = north;
            Bindings[3, 2] = east;
            Bindings[3, 3] = south;
        }
    }

    class Board
    {
        Tile[] tiles = new Tile[9];
        Tile[] positions = new Tile[9];

        public Board()
        {
            tiles[0] = new Tile(0, 8, 2, 64, 32);
            tiles[1] = new Tile(1, 2, 128, 16, 4);
            tiles[2] = new Tile(2, 8, 128, 1, 32);
            tiles[3] = new Tile(3, 16, 2, 1, 8);
            tiles[4] = new Tile(4, 1, 4, 16, 128);
            tiles[5] = new Tile(5, 8, 2, 64, 16);
            tiles[6] = new Tile(6, 2, 64, 8, 128);
            tiles[7] = new Tile(7, 2, 8, 32, 64);
            tiles[8] = new Tile(8, 4, 64, 16, 32);
        }

        void Print()
        {
            for (int i = 0; i < 9; i += 3)
            {
                Console.WriteLine("({0} r {1})\t({2} r {3})\t({4} r {5})",
                    positions[i].Id, positions[i].Rotation,
                    positions[i + 1].Id, positions[i + 1].Rotation,
                    positions[i + 2].Id, positions[i + 2].Rotation);
            }
            Console.WriteLine();
        }

        bool IsMatch(Tile tileA, int positionB, int bindingSite)
        {
            Tile tileB = positions[positionB];
            if (tileB == null) return true;
            int sum = 0;
            switch (bindingSite)
            {
                case 0:  // North binding
                    sum = tileA.Bindings[tileA.Rotation, 0] + tileB.Bindings[tileB.Rotation, 2];
                    break;
                case 1: // East binding
                    sum = tileA.Bindings[tileA.Rotation, 1] + tileB.Bindings[tileB.Rotation, 3];
                    break;
                case 2: // South binding
                    sum = tileA.Bindings[tileA.Rotation, 2] + tileB.Bindings[tileB.Rotation, 0];
                    break;
                case 3: // West binding
                    sum = tileA.Bindings[tileA.Rotation, 3] + tileB.Bindings[tileB.Rotation, 1];
                    break;
            }
            return ((sum == 3) || (sum == 12) || (sum == 48) || (sum == 192));

        }

        bool CanPlaceTile(Tile tile, int position)
        {
            switch (position)
            {
                case 0:
                    if (!IsMatch(tile, 1, 1)) return false;
                    if (!IsMatch(tile, 3, 2)) return false;
                    break;
                case 1:
                    if (!IsMatch(tile, 2, 1)) return false;
                    if (!IsMatch(tile, 4, 2)) return false;
                    if (!IsMatch(tile, 0, 3)) return false;
                    break;
                case 2:
                    if (!IsMatch(tile, 5, 2)) return false;
                    if (!IsMatch(tile, 1, 3)) return false;
                    break;
                case 3:
                    if (!IsMatch(tile, 0, 0)) return false;
                    if (!IsMatch(tile, 4, 1)) return false;
                    if (!IsMatch(tile, 3, 2)) return false;
                    break;
                case 4:
                    if (!IsMatch(tile, 1, 0)) return false;
                    if (!IsMatch(tile, 5, 1)) return false;
                    if (!IsMatch(tile, 7, 2)) return false;
                    if (!IsMatch(tile, 3, 3)) return false;
                    break;
                case 5:
                    if (!IsMatch(tile, 2, 0)) return false;
                    if (!IsMatch(tile, 8, 2)) return false;
                    if (!IsMatch(tile, 4, 3)) return false;
                    break;
                case 6:
                    if (!IsMatch(tile, 3, 0)) return false;
                    if (!IsMatch(tile, 7, 1)) return false;
                    break;
                case 7:
                    if (!IsMatch(tile, 4, 0)) return false;
                    if (!IsMatch(tile, 8, 1)) return false;
                    if (!IsMatch(tile, 6, 3)) return false;
                    break;
                case 8:
                    if (!IsMatch(tile, 5, 0)) return false;
                    if (!IsMatch(tile, 7, 3)) return false;
                    break;
            }
            return true;
        }


        public void Solve(int position = 0)
        {
            for (int tileNum = 0; tileNum < 9; tileNum++)
            {
                Tile tile = tiles[tileNum];
                if (!tile.Placed)
                {
                    for (tile.Rotation = 0; tile.Rotation < 4; tile.Rotation++)
                    {
                        if (CanPlaceTile(tile, position))
                        {
                            tile.Placed = true;
                            positions[position] = tile;
                            if (position == 8)
                                Print();
                            else
                                Solve(position + 1);                            
                            positions[position] = null;
                            tile.Placed = false;
                        }
                    }
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board();
            board.Solve();

            if (Debugger.IsAttached)
            {
                Console.WriteLine("\nPress any key to continue . . .");
                Console.ReadKey();
            }
        }
    }
}
