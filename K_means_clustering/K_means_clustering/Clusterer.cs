using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_means_clustering
{
    public class Clusterer
    {
        Random random = new Random();
        InitOption initOption = InitOption.RandomClusters;

        /// <summary>
        /// Simple example of K-means clustering algorithm
        /// </summary>
        /// <param name="data"> 2-dimensional floating point data </param>
        /// <param name="k"> number of clusters </param>
        /// <returns> array of cluster numbers assigned to every element </returns>
        public int[] Kmeans(double[][] data, int k)
        {
            // assuming data is non-empty
            // data[0].Length should be == 2
            int[] clusterNumbers;
            double[][] centroid = new double[k][];
            for (int i = 0; i < k; i++)
            {
                centroid[i] = new double[data[0].Length];
            }
            
            if (initOption == InitOption.RandomClusters)
            {
                clusterNumbers = RandomInit(data.Length, k);
                ComputeCentroids(data, clusterNumbers, k, centroid);
            }
            else // initOption == InitOption.RandomCentroids
            {
                clusterNumbers = new int[data.Length];
                do
                {
                    RandomCentroids(data, k, centroid);
                    GetNewClusterNumbers(data, clusterNumbers, k, centroid);

                } while (IsEmptyCluster(clusterNumbers, k));
                

                ComputeCentroids(data, clusterNumbers, k, centroid);
            }

            bool changed = true;
            // main loop of the algorithm:
            do
            {
                changed = GetNewClusterNumbers(data, clusterNumbers, k, centroid);
                ComputeCentroids(data, clusterNumbers, k, centroid);
            } while (changed);

            return clusterNumbers;
        }

        bool IsEmptyCluster(int[] clusterNumbers, int k)
        {
            bool[] nonempty = new bool[k];
            int n = 0;
            for (int i = 0; i < clusterNumbers.Length; i++)
            {
                if (!nonempty[clusterNumbers[i]]) n++;
                nonempty[clusterNumbers[i]] = true;
            }
            return n != k;
        }

        /// <summary>
        /// Random assignment of clusters to every element of array
        /// </summary>
        /// <param name="n"> size of array </param>
        /// <param name="k"> number of clusters </param>
        /// <returns> array of randomly assigned cluster numbers </returns>
        int[] RandomInit(int n, int k)
        {
            int[] clusterNumbers = new int[n];
            for (int i = 0; i < n; i++)
            {
                clusterNumbers[i] = random.Next(k);
            }
            return clusterNumbers;
        }

        
        void RandomCentroids(double[][] data, int k, double[][] centroid)
        {
            // 1. compute maximum and minimum of elements in data
            double minV = data[0][0], maxV = minV;
            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < data[0].Length; j++)
                {
                    if (data[i][j] > maxV) maxV = data[i][j];
                    if (data[i][j] < minV) minV = data[i][j];
                }
            }

            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < data[0].Length; j++)
                {
                    centroid[i][j] = minV + random.NextDouble() * (maxV - minV);
                }
            }
            // double[][] centroid - our result
        }


        /// <summary>
        /// computing coordinates of centroid for every cluster
        /// </summary>
        /// <param name="data"> input data </param>
        /// <param name="clusterNumbers"> current clustering </param>
        /// <param name="k"> number of clusters </param>
        /// <param name="centroid"> centroid coordinates, an output parameter </param>
        void ComputeCentroids(double[][] data, int[] clusterNumbers, int k, double[][] centroid)
        {
            // data.Length == clusterNumbers.Length
            // data[0].Length - dimension, data[0].Length == 2

            // clearing centroid data
            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < data[0].Length; j++)
                {
                    centroid[i][j] = 0.0;
                }
            }

            int[] N_cluster = new int[k]; // number of points in cluster
            // summing up coordinates for every element in cluster
            for (int i = 0; i < data.Length; i++)
            {
                N_cluster[clusterNumbers[i]]++;
                for (int j = 0; j < data[0].Length; j++)
                {
                    centroid[clusterNumbers[i]][j] += data[i][j];
                }
            }
            // getting the average numbers (dividing sum by amount of elements)
            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < data[0].Length; j++)
                {
                    centroid[i][j] /= N_cluster[i];
                }
            }
            // double[][] centroid - our result
            
        }

        bool GetNewClusterNumbers(double[][] data, int[] clusterNumbers, int k, double[][] centroid)
        {
            bool changed = false;
            for (int i = 0; i < data.Length; i++)
            {
                int minK = clusterNumbers[i];
                double minDist = DistSqr(data[i], centroid[minK]);
                
                for (int j = 0; j < k; j++)
                {
                    double curDist = DistSqr(data[i], centroid[j]);
                    if (curDist < minDist)
                    {
                        minDist = curDist;
                        minK = j;
                        changed = true;
                    }
                }
                clusterNumbers[i] = minK;
            }
            // int[] clusterNumbers - our result
            return changed;
        }

        double DistSqr(double[] dataPoint, double[] centroid)
        {
            double res = 0.0;
            for (int i = 0; i < dataPoint.Length; i++)
            {
                res += (dataPoint[i] - centroid[i]) * (dataPoint[i] - centroid[i]);
            }
            return res;
        }


        public enum InitOption { RandomClusters, RandomCentroids };
    }
}
