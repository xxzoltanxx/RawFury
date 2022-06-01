using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GenerationAlgorithm {
    SideWinder = 0,
    BinaryTree = 1
}

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] public int size;
    [SerializeField] public GenerationAlgorithm generationAlgorithm = GenerationAlgorithm.SideWinder; 
    private IMazeAlgo _mazeAlgo;
    private CellStatus[,] _mazeGenerated;
    // Start is called before the first frame update

    public void Awake()
    {
        if (generationAlgorithm == GenerationAlgorithm.SideWinder)
        {
            _mazeAlgo = new SideWinderGenerationAlgo();
        }
        else
        {
            _mazeAlgo = new BinaryTreeGenerationAlgo();
        }
    }
    public CellStatus[,] GetMaze()
    {
        return _mazeGenerated;
    }

    public void GenerateMaze()
    {
        _mazeGenerated = _mazeAlgo.Generate(size);
        _mazeGenerated[0,0] = CellStatus.Entrance;
    }
}
