using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellStatus{
    Taken,
    NotTaken,
    Entrance,
    Exit,
}

public interface IMazeAlgo
{
    CellStatus[,] Generate(int size);
}