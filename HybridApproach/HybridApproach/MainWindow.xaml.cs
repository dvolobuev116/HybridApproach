using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;

namespace HybridApproach
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Random r = new Random();
            XmlSerializer xml = new XmlSerializer(typeof(travellingSalesmanProblemInstance));
            travellingSalesmanProblemInstance test;
            using (StreamReader reader = new StreamReader(@"D:\Torrent\bayg29.xml\bayg29.xml"))
            {
                test = (travellingSalesmanProblemInstance)xml.Deserialize(reader);
            }
            int n = test.graph.Length;
            string[] names = new string[n];
            double[,] d = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                RowDefinition row = new RowDefinition() { Height = new GridLength(30) };
                ColumnDefinition col = new ColumnDefinition() { Width = new GridLength(30) };
                g_Matrix.RowDefinitions.Add(row);
                g_Matrix.ColumnDefinitions.Add(col);
            }
            for (int i = 0; i < n; i++)
            {
                names[i] = i.ToString();
                for (int j = 0; j < n - 1; j++)
                {
                    float length = test.graph[i][j].cost;
                    d[i, test.graph[i][j].Value] = length;
                    TextBlock tb = new TextBlock() { Text = length.ToString() };
                    Grid.SetRow(tb, i);
                    Grid.SetColumn(tb, test.graph[i][j].Value);
                    g_Matrix.Children.Add(tb);
                }
            }
            Approach a = new Approach(d, 10);
            a.Alpha = 1;
            a.Beta = 4;
            a.Gamma = 1;
            a.PCrossover = 0.2;
            a.PMutation = 0.2;
            a.Rho = 0.5;
            a.M = n;
            a.Tau0 = a.M / NearestNeighborTour.GetLength(d, r.Next(n));
            a.TMax = 100;
            a.Start();
        }
    }
}
