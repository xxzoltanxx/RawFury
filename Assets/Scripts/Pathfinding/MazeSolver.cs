using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PathFindingAlgoType
{
    Astar = 1,
}

public class MazeSolver : MonoBehaviour
{
    [SerializeField] PathFindingAlgoType _algoType = PathFindingAlgoType.Astar;

    private IPathFindingAlgo _pathFindingAlgo;

    void Awake()
    {
        if (_algoType == PathFindingAlgoType.Astar)
        {
            _pathFindingAlgo = new AStarAlgo();
        }
        else
        {
            //It performs the best in most cases anyway since the exit is known
            _pathFindingAlgo = new AStarAlgo();
        }
    }
    public static int reduceCoordToInt(int x ,int y, int width)
    {
        return y * width + x;
    }

    public static Tuple<int,int> getCoordFromInt(int coord, int width)
    {
        int y = coord / width;
        int x = coord % width;
        return Tuple.Create(x,y);
    }
    
    public List<Tuple<int,int>> SolveMaze(CellStatus[,] maze, int xExit, int yExit)
    {
        return _pathFindingAlgo.FindPath(maze, xExit, yExit);
    }

}
