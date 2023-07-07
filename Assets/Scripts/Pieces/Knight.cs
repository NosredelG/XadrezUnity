using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    bool[,] mat = new bool[8, 8];
    public Knight(int qteMovimentos, ChessBoard chessBoard, int positionX, int positionY) : 
        base(qteMovimentos, chessBoard, positionX, positionY)
    {

    }
    private void Start()
    {
        mat = new bool[8, 8];
        chessBoard = GameController.gc.chessBoard;
    }

    public override bool[,] PossibleMoves()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                mat[x, y] = false;
            }
        }

        //right-up
        if (column + 2 < 8 && line + 1 < 8)
        {
            mat[column + 2, line + 1] = true;
        }
        //right-down
        if (column + 2 < 8 && line - 1 >= 0)
        {
            mat[column + 2, line - 1] = true;
        }
        //UP-right
        if (line + 2 < 8 && column + 1 < 8)
        {
            mat[column + 1, line + 2] = true;
        }
        //up-left
        if (line + 2 < 8 && column - 1 >= 0)
        {
            mat[column - 1, line + 2] = true;
        }
        //down-right
        if (column + 1 < 8 && line - 2 >= 0)
        {
            mat[column + 1, line - 2] = true;
        }
        //down-left
        if (column - 1 >= 0 && line - 2 >= 0)
        {
            mat[column - 1, line - 2] = true;
        }
        //left-up
        if (line + 1 < 8 && column - 2 >= 0)
        {
            mat[column - 2, line + 1] = true;
        }
        //left-down
        if (line - 1 >= 0 && column - 2 >= 0)
        {
            mat[column - 2, line - 1] = true;
        }

        return mat;
    }

    public override bool TestMoves(int x, int y)
    {
        if (mat[x, y])
        {
            return true;
        }
        return false;
    }
}
