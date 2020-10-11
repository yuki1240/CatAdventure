using System;

namespace ConsoleApplication1
{
    public static class MazeGenerator_Bar
    {
        // 通路・壁情報
        const int Path = 0;
        const int Wall = 1;

        // 棒倒し法による迷路生成
        public static int[,] GenerateMaze(int width, int height)
        {
            // 5未満のサイズでは生成できない
            if (height < 5 || width < 5) throw new ArgumentOutOfRangeException();
            if (width % 2 == 0) width++;
            if (height % 2 == 0) height++;

            // 指定サイズで生成し外周を壁にする
            var maze = new int[width, height];

            // ここでMapInfoに壁か通路の情報を入れる
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                        maze[x, y] = Wall; // 外周はすべて壁
                    else
                        maze[x, y] = Path;  // 外周以外は通路

            // 棒を立て、倒す
            var rnd = new Random();
            for (int x = 2; x < width - 1; x += 2)
            {
                for (int y = 2; y < height - 1; y += 2)
                {
                    maze[x, y] = Wall; // 棒を立てる

                    // 倒せるまで繰り返す
                    while (true)
                    {
                        // 1行目のみ上に倒せる
                        int direction;
                        if (y == 2)
                            direction = rnd.Next(4);
                        else
                            direction = rnd.Next(3);

                        // 棒を倒す方向を決める
                        int wallX = x;
                        int wallY = y;
                        switch (direction)
                        {
                            case 0: // 右
                                wallX++;
                                break;
                            case 1: // 下
                                wallY++;
                                break;
                            case 2: // 左
                                wallX--;
                                break;
                            case 3: // 上
                                wallY--;
                                break;
                        }
                        // 壁じゃない場合のみ倒して終了
                        if (maze[wallX, wallY] != Wall)
                        {
                            maze[wallX, wallY] = Wall;
                            break;
                        }
                    }
                }
            }
            return maze;
        }

        // デバッグ用メソッド
        public static void DebugPrint(int[,] maze)
        {
            Console.WriteLine($"Width: {maze.GetLength(0)}");
            Console.WriteLine($"Height: {maze.GetLength(1)}");
            for (int y = 0; y < maze.GetLength(1); y++)
            {
                for (int x = 0; x < maze.GetLength(0); x++)
                {
                    Console.Write(maze[x, y] == Wall ? "■" : "　");
                }
                Console.WriteLine();
            }
        }
    }
}