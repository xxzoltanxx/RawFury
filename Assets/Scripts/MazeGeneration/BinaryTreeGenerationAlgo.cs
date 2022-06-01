using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryTreeGenerationAlgo : IMazeAlgo
{
    private int[] allowedDirections = new int[]{1,2};


    public CellStatus[,] Generate(int size)
    {
        CellStatus[,] mazeGenerated = new CellStatus[size, size];
        for (int i = 0; i < size; ++i)
        {
            mazeGenerated[i,0] = CellStatus.NotTaken;
        }
         for (int i = 0; i < size; ++i)
            for (int j = 1; j < size; ++j)
            {
                 mazeGenerated[i,j] = CellStatus.Taken;;
             }
        for (int i = 1; i < size; i += 2)
         for (int j = 1; j < size; j += 2)
         {
             HashSet<int> theseDirections = new HashSet<int>(allowedDirections);
             if (i == 1)
                theseDirections.Remove(-1);
            if (i == size - 1)
                theseDirections.Remove(1);
            if (j == 1)
                theseDirections.Remove(-2);
            if (j == size - 1)
                theseDirections.Remove(2);
            
            List<int> theseDirectionsList = new List<int>(theseDirections);
            Debug.Log(theseDirections.Count);
            int dir = 3;
            if (theseDirectionsList.Count > 0)
                dir = theseDirectionsList[Random.Range(0, theseDirectionsList.Count)];
            mazeGenerated[i,j] = CellStatus.NotTaken;

            if (dir == -1)
            {
                mazeGenerated[i - 1, j] = CellStatus.NotTaken;
            }
            else if (dir == 1)
            {
                mazeGenerated[i + 1, j] = CellStatus.NotTaken;
            }
            else if (dir == -2)
            {
                mazeGenerated[i, j - 1] = CellStatus.NotTaken;
            }
            else if (dir == 2)
            {
                mazeGenerated[i, j + 1] = CellStatus.NotTaken;
            }
         }
         return mazeGenerated;
    }
}
