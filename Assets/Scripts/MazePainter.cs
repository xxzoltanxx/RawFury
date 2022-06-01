using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazePainter : MonoBehaviour
{
    [SerializeField] MazeGenerator mazeGenerator;
    [SerializeField] GameObject wallPrefab;
    [SerializeField] GameObject groundPrefab;
    [SerializeField] GameObject startPrefab;
    [SerializeField] GameObject pathHolder;
    public void PaintMaze()
    {
        mazeGenerator.GenerateMaze();
        CellStatus [,] maze = mazeGenerator.GetMaze();

        for (int i = 0; i < maze.GetLength(0); ++i)
         for (int j = 0; j < maze.GetLength(1); ++j)
         {
            GameObject obj;
             if (maze[i,j] == CellStatus.Taken)
             {
                 obj = Instantiate(wallPrefab, new Vector3(i + 0.5f, j + 0.5f, 0), Quaternion.identity);
             }
             else if (maze[i,j] == CellStatus.Entrance)
            {
                 obj = Instantiate(startPrefab, new Vector3(i + 0.5f, j + 0.5f, 0), Quaternion.identity);
             }
             else
             {
                 obj = Instantiate(groundPrefab, new Vector3(i + 0.5f, j + 0.5f, 0), Quaternion.identity);
             }
             obj.name = i.ToString() + ',' + j.ToString();
         }
    }

    public void PaintPath(int xExit, int yExit)
    {
          foreach (Transform child in pathHolder.transform)
         {
             Destroy(child.gameObject);
         }
        CellStatus [,] maze = mazeGenerator.GetMaze();

        var path = GetComponent<MazeSolver>().SolveMaze(maze, xExit, yExit);

        foreach (var coord in path)
        {
            var obj = Instantiate(startPrefab, new Vector3(coord.Item1 + 0.5f, coord.Item2 + 0.5f, 0), Quaternion.identity);
            obj.transform.parent = pathHolder.transform;
        }


    }
    // Start is called before the first frame update
    void Start()
    {
        PaintMaze();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
