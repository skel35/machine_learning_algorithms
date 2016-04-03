using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_means_clustering
{
    public class Clusterer
    {
        Random random = new Random(0);
        InitOption initOption = InitOption.RandomClusters;
        int[] clustering;
        int k = 3; // number of clusters
        double[][] centroid;
        /// <summary>
        /// Simple example of K-means clustering algorithm
        /// </summary>
        /// <param name="data"> floating point data </param>
        /// <param name="k"> number of clusters </param>
        /// <returns> array of cluster numbers assigned to every element </returns>
        public int[] Kmeans(double[][] data, int k)
        {
            this.k = k;
            // assuming data is non-empty
            centroid = new double[k][];
            for (int i = 0; i < k; i++)
            {
                centroid[i] = new double[data[0].Length];
            }
            
            if (initOption == InitOption.RandomClusters)
            {
                clustering = RandomInit(data.Length);
                ComputeCentroids(data);
            }
            else // initOption == InitOption.RandomCentroids
            {
                clustering = new int[data.Length];
                do
                {
                    RandomCentroids(data);
                    GetNewClusterNumbers(data);

                } while (IsEmptyCluster());
                

                ComputeCentroids(data);
            }

            bool changed = true;
            // main loop of the algorithm:
            do
            {
                changed = GetNewClusterNumbers(data);
                ComputeCentroids(data);
            } while (changed);

            return clustering;
        }

        /// <summary>
        /// Shows if any cluster is empty
        /// </summary>
        /// <returns> true if at least one of the clusters is empty
        /// false otherwise
        /// </returns>
        bool IsEmptyCluster()
        {
            bool[] nonempty = new bool[k];
            int n = 0;
            for (int i = 0; i < clustering.Length; i++)
            {
                if (!nonempty[clustering[i]]) n++;
                nonempty[clustering[i]] = true;
            }
            return n != k;
        }

        /// <summary>
        /// Random assignment of clusters to every element of array
        /// </summary>
        /// <param name="n"> size of array </param>
        /// <param name="k"> number of clusters </param>
        /// <returns> array of randomly assigned cluster numbers </returns>
        int[] RandomInit(int n)
        {
            int[] clusterNumbers = new int[n];
            for (int i = 0; i < n; i++)
            {
                clusterNumbers[i] = random.Next(k);
            }
            return clusterNumbers;
        }

        
        void RandomCentroids(double[][] data)
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
        void ComputeCentroids(double[][] data)
        {
            // data.Length == clustering.Length
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
                N_cluster[clustering[i]]++;
                for (int j = 0; j < data[0].Length; j++)
                {
                    centroid[clustering[i]][j] += data[i][j];
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

        bool GetNewClusterNumbers(double[][] data)
        {
            bool changed = false;
            for (int i = 0; i < data.Length; i++)
            {
                int minK = clustering[i];
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
                clustering[i] = minK;
            }
            // int[] clustering - our result
            return changed;
        }

        double DistSqr(double[] dataPoint, double[] clusterCentroid)
        {
            double res = 0.0;
            for (int i = 0; i < dataPoint.Length; i++)
            {
                res += (dataPoint[i] - clusterCentroid[i]) * (dataPoint[i] - clusterCentroid[i]);
            }
            return res;
        }


        public enum InitOption { RandomClusters, RandomCentroids };
    }
}
