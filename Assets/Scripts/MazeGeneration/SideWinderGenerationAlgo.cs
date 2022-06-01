using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWinderGenerationAlgo : IMazeAlgo
{
    public CellStatus[,] Generate(int size)
    {
        CellStatus[,] mazeGenerated = new CellStatus[size, size];
        for (int i = 0; i < size; ++i)
            for (int j = 0; j < size; ++j)
            {
                 mazeGenerated[i,j] = CellStatus.Taken;;
             }
        int w = size - 1;
        int h = size - 1;
        List<int> openSet = new List<int>();
        for (int j = 0; j < h; j += 2)
        {
            int i = 1;
            if (j == 0)
            {
                for (;i < w; ++i)
                {
                    mazeGenerated[i,0] = CellStatus.NotTaken;
                }
            }
            else
            {
            while (i < w - 1)
            {
                openSet.Clear();
                openSet.Add(i++);
                while ((Random.Range(0, 10) > 2) && i < w - 1)
                {
                    openSet.Add(i++);
                }
                if (openSet.Count == 1 && mazeGenerated[openSet[0], j - 2] == CellStatus.Taken && i < w)
                {
                    openSet.Add(i++);
                }
                foreach (int xCoord in openSet)
                {
                    mazeGenerated[xCoord,j] = CellStatus.NotTaken;
                }
                List<int> culledSet = new List<int>();
                foreach (int index in openSet)
                {
                    if (mazeGenerated[index, j - 2] == CellStatus.NotTaken)
                    {
                        culledSet.Add(index);
                    }
                }
                //Edge case for right side
                if (culledSet.Count == 0)
                {
                    mazeGenerated[openSet[0], j - 1] = CellStatus.NotTaken;
                    mazeGenerated[openSet[0], j - 2] = CellStatus.NotTaken;
                }
                else
                {
                int xRand = culledSet[Random.Range(0, culledSet.Count)];
                if (j - 1 < h)
                {
                    mazeGenerated[xRand, j - 1] = CellStatus.NotTaken;
                }
            }
            if (i < w - 1)
            {
                i++;
            }

            }
            }
        }
        return mazeGenerated;
    }
}