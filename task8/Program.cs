using System;
using System.Collections.Generic;

namespace task8
{
    class Program
    {
        public static Random random = new Random();
        static void Main()
        {
            Console.WriteLine("Задание 8:\n" +
                "Граф задан матрицей инциденций. Найти все его мосты.\n\n" +
                "----------");
            Console.Write("Задайте кол-во вершин: ");
            int vertex = int.Parse(Console.ReadLine());
            //Console.Write("Задайте кол-во ребер: ");
            //int edge = int.Parse(Console.ReadLine());
            int[,] Matrix = CreateMatrix(vertex, vertex);
            PrintMatrix(Matrix);

            Graph graph = new Graph(vertex);
            graph.AddEdges(Matrix);
            graph.bridge();

            Console.ReadKey();
        }
        //ребра         //вершины
        public static int[,] CreateMatrix(int graphEdges, int graphVertexes)
        {
            int[,] newMatrix = new int[graphEdges, graphVertexes];
            //заполнить матицу нулями
            for (int i = 0; i < graphEdges; i++)
            {
                for (int j = 0; j < graphVertexes; j++)
                {
                    newMatrix[i, j] = 0;
                }
            }
            //добавить для каждого ребра две вершины
            for (int i = 0; i < graphEdges; i++)
            {
                int count = 2;
                do
                {
                    int vertex = random.Next(0, graphVertexes);
                    if (newMatrix[vertex, i] != 1)
                    {
                        newMatrix[vertex, i] = 1;
                        count--;
                    }
                } while (count > 0);
            }
            return newMatrix;
        }
        public static void PrintMatrix(int[,] Matrix)
        {
            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < Matrix.GetLength(1); j++)
                    if (Matrix[i, j] == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(String.Format("{0,3}", Matrix[i, j]));
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(String.Format("{0,3}", Matrix[i, j]));
                    }
                Console.WriteLine();
            }
        }
        public class Graph
        {
            private int V; 

           
            private List<int>[] adjacency;
            int time = 0;
            static readonly int NIL = -1;

           
            public Graph(int v)
            {
                V = v;
                adjacency = new List<int>[v];
                for (int i = 0; i < v; ++i)
                    adjacency[i] = new List<int>();
            }

            
            public void AddEdges(int[,] matrix)
            {
                int fistNode = -1;
                int secondNode = -1;
                //перебор столбцов
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    //перебор строк
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (matrix[j, i] == 1)
                        {
                            if (fistNode == -1)
                                fistNode = j;
                            else
                                secondNode = j;
                        }
                    }
                    adjacency[fistNode].Add(secondNode);
                    try
                    {
                        adjacency[secondNode].Add(fistNode);
                    }
                    catch (System.IndexOutOfRangeException)
                    {
                        Console.WriteLine("Произошла ошибка при создании матрицы. Попробуйте ввод еще раз.");
                        Main();
                    }
                    fistNode = -1;
                    secondNode = -1;
                }
            }
            void bridgeUtil(int u, bool[] visited, int[] disc,
                            int[] low, int[] parent)
            {

               
                visited[u] = true;

               
                disc[u] = low[u] = ++time;

               
                foreach (int i in adjacency[u])
                {
                    int v = i; 
                    if (!visited[v])
                    {
                        parent[v] = u;
                        bridgeUtil(v, visited, disc, low, parent);

                        
                        low[u] = Math.Min(low[u], low[v]);

                        
                        if (low[v] > disc[u])
                            Console.WriteLine(u + " " + v);
                    }

                  
                    else if (v != parent[u])
                        low[u] = Math.Min(low[u], disc[v]);
                }
            }

            public void bridge()
            {

                bool[] visited = new bool[V];
                int[] disc = new int[V];
                int[] low = new int[V];
                int[] parent = new int[V];

                for (int i = 0; i < V; i++)
                {
                    parent[i] = NIL;
                    visited[i] = false;
                } 
                for (int i = 0; i < V; i++)
                    if (visited[i] == false)
                        bridgeUtil(i, visited, disc, low, parent);
            }
        }
    }
}
