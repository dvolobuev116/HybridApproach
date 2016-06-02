using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HybridApproach
{
    public class Approach
    {
        public double[,] D { get; set; }
        public int N { get { return D.GetLength(0); } }

        public double Tau0 { get; set; } = 2;
        public double Rho { get; set; } = 0.5;

        public int M { get; set; } = 10;
        public double G0 { get; set; } = 3;
        public double X { get; set; } = 2;

        public int TMax { get; set; } = 10;
        private Random Rand { get; set; } = new Random();

        public double Alpha { get; set; } = 1;
        public double Beta { get; set; } = 3;
        public double Gamma { get; set; } = 1;

        public double PCrossover { get; set; } = 0.5;
        public double PMutation { get; set; } = 0.5;

        public Approach(double[,] d, int tMax)
        {
            D = d;
        }

        public void Start()
        {
            //<Инициализация>
            double[,] tau = InitArray(N, N, Tau0);
            double[,] eta = new double[N, N];
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    eta[i, j] = 1 / D[i, j];
            double[,] g = InitArray(N, N, G0);
            //</Инициализация/>
            //<Основной цикл алгоритма>
            double min = double.MaxValue;
            for (int t = 0; t < TMax; t++)
            {
                //<Размещение агентов в случайных городах>
                int[] MCurrent = new int[M];
                int[][] path = new int[M][];
                bool[][] J = new bool[M][];
                for (int k = 0; k < M; k++)
                {
                    int VIndex = Rand.Next(0, N);
                    MCurrent[k] = VIndex;
                    J[k] = Enumerable.Repeat(true, N).ToArray();
                    J[k][VIndex] = false;
                    path[k] = new int[N];
                    path[k][0] = VIndex;
                }
                //</Размещение агентов в случайных городах/>
                //<Обход всех вершин всеми агентами>
                for (int k = 0; k < M; k++)
                {
                    for (int l = 1; l < N; l++)
                    {
                        //<Выбор следующей вершины/>
                        double[] numerator = new double[N];
                        int i = MCurrent[k];
                        for (int j = 0; j < N; j++)
                            if (J[k][j])
                                numerator[j] = Math.Pow(tau[i, j], Alpha) * Math.Pow(eta[i, j], Beta) * Math.Pow(g[i, j], Gamma);
                        double denomerator = Enumerable.Sum(numerator);
                        int maxIndex = -1;
                        double maxP = 0;
                        for (int j = 0; j < N; j++)
                            if (numerator[j] > 0)
                            {
                                double p = numerator[j] / denomerator;
                                if (p > maxP)
                                {
                                    maxP = p;
                                    maxIndex = j;
                                }
                            }
                        MCurrent[k] = maxIndex;
                        J[k][maxIndex] = false;
                        path[k][l] = maxIndex;
                        //</Выбор следующей вершины/>
                    }
                }
                //</Обход всех вершин всеми агентами>
                //<Обновление феромона>
                double[][,] deltaTau = new double[M][,];
                double[] L = new double[M];
                for (int k = 0; k < M; k++)
                {
                    L[k] = CalculatePathLenght(path[k], D);
                    deltaTau[k] = new double[N, N];
                    for (int i = 1; i < path[k].Length; i++)
                        deltaTau[k][path[k][i - 1], path[k][i]] = 1 / L[k];
                }
                for (int i = 0; i < N; i++)
                    for (int j = 0; j < N; j++)
                        tau[i, j] = (1 - Rho) * tau[i, j] + SummarizeDelta(deltaTau, i, j);
                //</Обновление феромона>
                //<Оценка>
                double[] e = new double[M];
                for (int k = 0; k < M; k++)
                    e[k] = N / L[k];
                double eMax = Enumerable.Max(e);
                double eAvg = Enumerable.Average(e);
                //</Оценка>
                //<Селекция решений>
                double[] f = new double[M];
                for (int k = 0; k < M; k++)
                {
                    f[k] = (X * (e[k] - eAvg) + (eMax - e[k])) * eAvg / (eMax - eAvg);
                    if (f[k] < 0)
                        f[k] = Math.Pow(f[k], 2);
                }

                double[] pGA = new double[M];
                double fSum = Enumerable.Sum(f);
                for (int k = 0; k < M; k++)
                    pGA[k] = f[k] / fSum;

                int[][] selectedPath = new int[M][];
                for (int k = 0; k < M; k++)
                {
                    double p = Rand.NextDouble();
                    int sectorIndex = 0;
                    double sectorValue = 0;
                    while (sectorValue < p)
                    {
                        sectorValue += pGA[sectorIndex++];
                    }
                    selectedPath[k] = path[sectorIndex - 1];
                }
                path = selectedPath;
                //</Селекция решений>
                //<Кроссинговер>
                if (Rand.NextDouble() < PCrossover)
                {
                    int[][] crossoverResult = new int[M][];
                    for (int k = 0; k < M; k++)
                    {
                        int indexS = Rand.Next(M);
                        int indexT = Rand.Next(M);
                        while (indexT == indexS)
                            indexT = Rand.Next(M);
                        int[] S = path[indexS];
                        int[] T = path[indexT];
                        crossoverResult[k] = Crossover(S, T, S.Length / 2);
                    }
                    path = crossoverResult;
                }
                //</Кроссинговер>
                //<Мутация>
                if (Rand.NextDouble() < PMutation)
                {
                    for (int k = 0; k < M; k++)
                    {
                        int i = Rand.Next(N - 2);
                        int j = Rand.Next(i + 1, N - 1);
                        int l = Rand.Next(j + 1, N);
                        Swap(path[k], i, j, l);
                    }
                }
                //</Мутация>
                //<Обновление генетической информации>
                double[][,] deltaG = new double[M][,];
                double[] G = new double[M];
                for (int k = 0; k < M; k++)
                {
                    G[k] = CalculatePathLenght(path[k], D);
                    deltaG[k] = new double[N, N];
                    for (int i = 1; i < path[k].Length; i++)
                        deltaG[k][path[k][i - 1], path[k][i]] = 1 / G[k];
                }
                for (int i = 0; i < N; i++)
                    for (int j = 0; j < N; j++)
                        g[i, j] = G0 + SummarizeDelta(deltaG, i, j);
                //</Обновление генетической информации>
                for (int i = 0; i < path.Length; i++)
                {
                    double len = CalculatePathLenght(path[i], D);
                    if (len < min)
                        min = len;
                }
            }
            MessageBox.Show(min.ToString());
            //</Основной цикл алгоритма>
        }

        public int[] Crossover(int[] S, int[] T, int k)
        {
            int[] result = new int[S.Length];
            Array.Copy(S, result, S.Length);
            for (int i = 0; i < k; i++)
            {
                int index = Array.IndexOf(result, T[i]);
                Swap(result, i, index);
            }
            return result;
        }
        public void Swap<T>(T[] array, int i, int j, int l)
        {
            T temp = array[l];
            array[l] = array[j];
            array[j] = array[i];
            array[i] = temp;
        }
        public void Swap<T>(T[] array, int i, int j)
        {
            T temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
        public double SummarizeDelta(double[][,] deltaTau, int i, int j)
        {
            double sum = 0;
            for (int k = 0; k < deltaTau.Length; k++)
            {
                sum += deltaTau[k][i, j];
            }
            return sum;
        }
        public double CalculatePathLenght(int[] path, double[,] d)
        {
            double length = 0;
            for (int i = 1; i < path.Length; i++)
            {
                length += d[path[i - 1], path[i]];
            }
            //length += d[path[path.Length - 1], path[0]];
            return length;
        }
        public T[,] InitArray<T>(int i, int j, T value)
        {
            T[,] result = new T[i, j];
            for (int p = 0; p < i; p++)
            {
                for (int q = 0; q < j; q++)
                {
                    result[p, q] = value;
                }
            }
            return result;
        }
    }
}
