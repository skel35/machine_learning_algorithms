using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_means_clustering
{
    class Program
    {
        static void Main(string[] args)
        {
            Clusterer clusterer = new Clusterer();
            double[][] data = new double[][]
            {
                new double[] {73, 72.6 },
                new double[] {61, 54.4 },
                new double[] {67.0, 99.9},
                new double[] {68.0, 97.3 },
                new double[] {62.0, 59.0 },
                new double[] {75.0, 81.6 },
                new double[] {74.0, 77.1 },
                new double[] {66.0, 97.3 },
                new double[] {68.0, 93.3 },
                new double[] {61.0, 59.0 }
            };

            int[] res = clusterer.Kmeans(data, 3);

            Console.ReadKey();
        }
    }
}
