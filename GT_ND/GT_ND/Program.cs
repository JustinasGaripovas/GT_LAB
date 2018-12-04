using System;
using System.Collections.Generic;

namespace GT_ND
{
    class MainClass
    {
        public const int BORDER = 8;

        public static Node[,] board = new Node[BORDER + 1, BORDER + 1];

        public static List<Node> stack = new List<Node>();

        public class Path
        {
            public Node start;
            public Node end;

            public int stepCount;

            public ConsoleColor color;

            public Path(Node s)
            {
                start = s;
            }

            public Path(Node s, Node e)
            {
                start = s;
                end = e;

            }

            public Path(){ }
        }

        public class Knight
        {
            ConsoleColor color = new ConsoleColor();

            public Node startNode = new Node();

            public String name;

            public Path toKing;
            public Path toEnd;

            public Knight() { }

            public Knight(Node s, ConsoleColor c)
            {
                this.color = c;

                startNode = s;
            }

            public Knight(Node s, ConsoleColor c,String name)
            {
                this.color = c;
                this.name = name;
                startNode = s;
            }

            public void InitToKing(Node k){
                this.toKing = new Path();
                this.toKing = ShowMoves(this.startNode, k);
                Console.WriteLine("To king");
                printPath(this.toKing);
            }

            public void InitToEnd(Node e)
            {
                this.toEnd = new Path();
                this.toEnd = ShowMoves(this.startNode,e );
                Console.WriteLine("To End");
                printPath(this.toEnd);
            }

            public int getPathToKing()
            {
                return toKing.stepCount;
            }

            public int getPathFromStartToEnd()
            {
                return toEnd.stepCount;
            }
        }

        public class Node :IComparable<Node>
        {
            public int x;
            public int y;
            public Node parent;
            public bool open = true;
            public bool seen = false;
            public double score = 100;
            public double step = 100;

            public Node()
            {

            }

            public Node(int x1, int y1)
            {
                x = x1;
                y = y1;
            }

            public void setScore(int score)
            {
                this.score = score;
            }

            public int CompareTo(Node other)
            {
                if (this.score > other.score)
                    return 1;

                if (this.score < other.score)
                    return -1;

                if (this.score == other.score)
                    return 0;

                return 0;
            }
        }

        public static void InitBoard()
        {
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 8; j++)
                {
                    Node currentNode = new Node(j, i);
                    board[i, j] = currentNode;
                }
            }
        }

        public static int getInput()
        {
            Console.WriteLine("Input how many knights should be on the board");
            int x = Convert.ToInt32(Console.ReadLine());
            if (x < 0 && x > 10)
            {
                Console.WriteLine("Error number should be less than 8");
                return getInput();
            }
            else{
                return x;
            }
        }

        public static void Main(string[] args)
        {
            InitBoard();

            Node kingNode = board[4, 8];
            Node finishNode = board[0, 4];

            List<Knight> allKnights = new List<Knight>();

            int countOfKnights = getInput();

            Console.WriteLine(countOfKnights);

            Random rnd = new Random();

            for (int i = 1; i <= countOfKnights; i++)
            {
                allKnights.Add(new Knight(board[rnd.Next(0, i), rnd.Next(0, i)], ConsoleColor.Blue, i +" knight"));
                Console.WriteLine();
            }

            /*
              allKnights.Add(new Knight(board[0,8],ConsoleColor.Blue,"First knight"));
              allKnights.Add(new Knight(board[1, 8], ConsoleColor.Yellow, "Second knight"));
              allKnights.Add(new Knight(board[6, 4], ConsoleColor.Yellow, "Third knight"));
              allKnights.Add(new Knight(board[1, 1], ConsoleColor.Yellow, "Forth knight"));
            */

            if (countOfKnights == 0)
            {
                Console.WriteLine("Nera zirgu ");
            }
            else
            {
                calculations(allKnights, kingNode, finishNode);
            }
        }

        public static void calculations(List<Knight> allKnights, Node kingNode, Node finishNode)
        {
            Knight tempKnight = new Knight();

            int minimalSteps = 99999;

            foreach (Knight k in allKnights)
            {
                Console.WriteLine();
                Console.WriteLine(k.name);
                Console.WriteLine(k.startNode.x + "  " + k.startNode.y);
                Console.WriteLine();

                k.InitToKing(kingNode);

                if (k.getPathToKing() < minimalSteps)
                {
                    tempKnight = k;
                    minimalSteps = k.getPathToKing();
                }

                Console.WriteLine(k.getPathToKing());
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Black;

            Console.Write("Final path");
            Console.ResetColor();


            foreach (Knight k in allKnights)
            {
                Console.WriteLine();
                Console.WriteLine(k.name);
                Console.WriteLine("Starting node position  " + k.startNode.x + "  " + k.startNode.y);

                Console.WriteLine();

                if (k == tempKnight)
                {
                    k.InitToKing(kingNode);
                    printPath(ShowMoves(kingNode,finishNode));

                }
                else
                {
                    k.InitToEnd(finishNode);
                }
            }
        }

        public static void ResetBoard()
        {
            stack = new List<Node>();

            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 8; j++)
                {
                    Node currentNode = board[i, j];
                    currentNode.step = 100;
                    currentNode.score = 100;
                    currentNode.seen = false; 
                    currentNode.open = true;
                    currentNode.parent = null;
                }
            }
        }

        public static Node ReturnMin()
        {
            Node minNode = new Node(0,0);
            minNode.setScore(100000);

            foreach(Node node in stack)
            {
                if(node.score <= minNode.score)
                {
                    minNode = node;
                }
            }

            return minNode;
        }

        public static void printPath(Path n)
        {

            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 8; j++)
                {
                    board[i, j].step = -1;
                }
            }

            int temp_stepcount = n.stepCount;

            Node node = n.end;
            while (node.parent != null)
            {
                board[node.y, node.x].step = temp_stepcount--;
                node = node.parent;
            }

            for (int i = 0; i <= 8; i++)
            {
                Console.Write("{0,3:0}", i);
            }

            Console.WriteLine();

            for (int i = 0; i <= 8; i++)
            {
                Console.Write("{0,3:0}", "___");
            }

            Console.WriteLine();


            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 8; j++)
                {
                    if (n.end == board[i, j])
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.Write("{0,3:0}", n.stepCount);
                    }
                    else if (n.start == board[i, j])
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.Write("{0,3:0}", 0);
                    }
                    else if (board[i, j].step == -1)
                    {
                        Console.BackgroundColor =ConsoleColor.Black;
                        Console.Write("{0,3:0}", 0);
                    }else{
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        Console.Write("{0,3:0}", board[i, j].step);
                    }
            
                    Console.ResetColor();
                }

                Console.Write(" | {0}", i);

                Console.WriteLine();
            }

            Console.WriteLine(n.stepCount);
        }

        public static Node BorderPatrol(int first, int second, int y, int x)
        {
            int tempY = y + first;
            int tempX = x + second;
            
            if (tempX >= 0 && tempY <= BORDER && tempY >= 0 && tempX <= BORDER)
            {
                return(board[tempY, tempX]);
            }

            return null;
        }

        public static double heuristic(Node current,Node end)
        {
            double d1 = Math.Abs(end.x - current.x);
            double d2 = Math.Abs(end.y - current.y);

            return Math.Sqrt(d1 * d1 + d2 * d2)*3;
        }

        public static Path ShowMoves(Node start, Node end)
        {
            ResetBoard();

            Path currentPath = new Path(start);

            currentPath.start = start;
            currentPath.end = end;

            stack = new List<Node>();

            start.score = heuristic(start, end);
            stack.Add(start);

            while(stack.Count > 0)
            {
                Node currentNode = ReturnMin();

                if(currentNode.x == end.x && currentNode.y == end.y)
                {
                    break;
                }

                currentNode.open = false;
                currentNode.seen = true;

                stack.Remove(currentNode);

                List<Node> neighborsList = new List<Node>();

                neighborsList.Add(BorderPatrol(-2, 1, currentNode.y, currentNode.x));
                neighborsList.Add(BorderPatrol(-2, -1, currentNode.y, currentNode.x));
                neighborsList.Add(BorderPatrol(2, 1, currentNode.y, currentNode.x));
                neighborsList.Add(BorderPatrol(2, -1, currentNode.y, currentNode.x));
                neighborsList.Add(BorderPatrol(-1, -2, currentNode.y, currentNode.x));
                neighborsList.Add(BorderPatrol(1, -2, currentNode.y, currentNode.x));
                neighborsList.Add(BorderPatrol(-1, 2, currentNode.y, currentNode.x));
                neighborsList.Add(BorderPatrol(1, 2, currentNode.y, currentNode.x));

                foreach(Node neighbor in neighborsList)
                {
                    if (neighbor == null || !neighbor.open)
                    {
                        continue;
                    }

                    double temp_score = currentNode.step + heuristic(currentNode, neighbor);

                    if (neighbor.seen == false){
                        stack.Add(neighbor);
                    }
                    else if (temp_score >= neighbor.step)
                    {
                        continue;
                    }

                    neighbor.parent = currentNode;
                    neighbor.step = temp_score;
                    neighbor.seen = true;
                    neighbor.score = neighbor.step + heuristic(neighbor, end);
                }
            }
            
            Node node = end;
            while (node.parent != null)
            {
                board[node.y, node.x].step = -1;
                currentPath.stepCount++;
                node = node.parent;
            }

            return currentPath;

            //if (stack.Count > 0)
            //    return;
            /*
            BorderPatrol(-2, 1, y, x);

            BorderPatrol(-2, -1, y, x);

            BorderPatrol(2, 1, y, x);

            BorderPatrol(2, -1, y, x);

            BorderPatrol(-1, -2, y, x);

            BorderPatrol(1, -2, y, x);

            BorderPatrol(-1, 2, y, x);

            BorderPatrol(1, 2, y, x);*/

            //board[y, x].open = false;

        }
    }
}
/*

   Node kingNode = board[8, 8];
            Node finishNode = board[2, 2];
            
            Path secondPath = new Path(board[8, 0]);

            secondPath = ShowMoves(secondPath.start, kingNode);
            secondPath.color = ConsoleColor.Blue;
            printPath(secondPath);

            ResetBoard();

            secondPath = ShowMoves(kingNode, finishNode);
            secondPath.color = ConsoleColor.Blue;
            printPath(secondPath);

            Path firstPath = new Path(board[0, 8]);

            firstPath = ShowMoves(firstPath.start, kingNode);
            firstPath.color = ConsoleColor.Gray;
            printPath(firstPath);
            
            ResetBoard();

*/
