using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPI;

namespace RVIP_5
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var env = new MPI.Environment(ref args))
            {
                if (MPI.Communicator.world.Rank == 0)
                {
                    Console.WriteLine("Найти максимальный элемент матрицы.");
                    Console.WriteLine("Исходный массив: ");
                    int[,] matrix = new int[5, 5];

                    Random rand = new Random(); // рандомная матрица
                    for (int i = 0; i < 5; i++)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            matrix[i, j] = rand.Next(1, 20);
                            Console.Write(matrix[i, j] + " ");
                        }
                        Console.WriteLine();
                    } 
                    List<int> list = new List<int>(); 
                    int currentmax = 0;

                    List<int> results = new List<int>();
                    for (int i = 0; i < 5; i++)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            list.Add(matrix[i, j]);
                        }

                        MPI.Communicator.world.Send(list, Communicator.world.Rank + i + 1, 0);

                        Console.WriteLine("Передана "+ (i+1) +"-я часть матрицы");
                        results.Add(MPI.Communicator.world.Receive<int>(Communicator.world.Rank + i + 1, 0)); // запись результата
                    }
                    foreach (int j in results)
                        {
                            if (j > currentmax)
                            {
                                currentmax = j;
                            }
                        }
                    Console.WriteLine("Наибольший элемент:  "+currentmax);
                }
                for(int i =1; i < 6; i++)
                {
                    if (MPI.Communicator.world.Rank == i)
                    {
                        Console.WriteLine();
                        int max = 0;
                        List<int> multiplication = MPI.Communicator.world.Receive<List<int>>(Communicator.world.Rank - i, 0);
                        foreach(int j in multiplication)
                        {
                            if (j > max)
                            {
                                max = j;
                            }
                        }
                        multiplication.Clear();
                        Communicator.world.Send(max, 0, 0);
                    }
                }
                
            }

        }
        
    }
}