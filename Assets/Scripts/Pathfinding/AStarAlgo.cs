using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public interface IPathFindingAlgo
{
    List<Tuple<int,int>> FindPath(CellStatus[,] maze, int xExit, int yExit);
}

public class AStarAlgo : IPathFindingAlgo
{
    private int hScore(int x1, int y1, int x2, int y2)
    {
        return (Math.Abs(x1 - x1) + Math.Abs(y1 - y2));
    }


    private List<Tuple<int,int>> reconstructPath(Dictionary<int,int> cameFrom, int current, int width)
    {
        List<Tuple<int,int>> totalPath = new List<Tuple<int,int>>();
        while (cameFrom[current] != 0)
        {
            totalPath.Add(MazeSolver.getCoordFromInt(cameFrom[current], width));
            current = cameFrom[current];
        }
        return totalPath;
    }

    public List<Tuple<int,int>> FindPath(CellStatus[,] maze, int xExit, int yExit)
    {
        return Astar(maze, xExit, yExit);
    }

    //Score for moving is 1, H values is manhattan distance

    private List<Tuple<int,int>> Astar(CellStatus[,] maze, int xExit, int yExit)
    {
        List<int> frontier = new List<int> ();
        
        Dictionary<int, int> gScore = new Dictionary<int, int>();
        Dictionary<int, int> fScore = new Dictionary<int, int>();
        Dictionary<int, int> cameFrom = new Dictionary<int,int>();

        //Adding 0,0, aka Entrance to open list;
        frontier.Add(0);
        gScore.Add(0, 0);
        fScore.Add(0, 1);

        while (frontier.Count != 0)
        {
            int min =int.MaxValue;
            int current = 0;
            foreach(var coord in frontier)
            {
                if (fScore[coord] < min)
                {
                    min = fScore[coord];
                    current = coord;
                }
            }

            if (current == MazeSolver.reduceCoordToInt(xExit, yExit, maze.GetLength(0)))
            {
                return reconstructPath(cameFrom, current, maze.GetLength(0));
            }

            frontier.Remove(current);

            HashSet<int> neighbours = new HashSet<int>();

            Tuple<int,int> currentTuple = MazeSolver.getCoordFromInt(current, maze.GetLength(0));
            if (currentTuple.Item1 > 0 && maze[currentTuple.Item1 - 1, currentTuple.Item2] == CellStatus.NotTaken)
            {
                neighbours.Add(MazeSolver.reduceCoordToInt(currentTuple.Item1 - 1, currentTuple.Item2, maze.GetLength(0)));
            }
            if (currentTuple.Item1 < maze.GetLength(0) - 1 && maze[currentTuple.Item1 + 1, currentTuple.Item2] == CellStatus.NotTaken)
            {
                neighbours.Add(MazeSolver.reduceCoordToInt(currentTuple.Item1 + 1, currentTuple.Item2, maze.GetLength(0)));
            }
            if (currentTuple.Item2 < maze.GetLength(1) - 1 && maze[currentTuple.Item1, currentTuple.Item2 + 1] == CellStatus.NotTaken)
            {
                neighbours.Add(MazeSolver.reduceCoordToInt(currentTuple.Item1, currentTuple.Item2 + 1, maze.GetLength(0)));
            }
            if (currentTuple.Item2 > 0 && maze[currentTuple.Item1, currentTuple.Item2 - 1] == CellStatus.NotTaken)
            {
                neighbours.Add(MazeSolver.reduceCoordToInt(currentTuple.Item1, currentTuple.Item2 - 1, maze.GetLength(0)));
            }

            foreach (var neighbour in neighbours)
            {
                //Distance is always 1
                int tentativeScore = gScore[current] + 1;

                if (!gScore.ContainsKey(neighbour) || tentativeScore < gScore[neighbour])
                {
                    cameFrom[neighbour] = current;
                    gScore[neighbour] = tentativeScore;
                    fScore[neighbour] = tentativeScore + hScore(xExit, yExit, neighbour % maze.GetLength(0), neighbour / maze.GetLength(0));

                    if (!frontier.Contains(neighbour))
                    {
                        frontier.Add(neighbour);
                    }
                }
            }
            
        }
        return new List<Tuple<int,int>>();

    }

}