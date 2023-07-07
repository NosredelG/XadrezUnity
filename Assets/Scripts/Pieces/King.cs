using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    bool[,] mat = new bool[8, 8];
    public King(int qteMovimentos, ChessBoard chessBoard, int positionX, int positionY) : 
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

        //right
        if (column + 1 < 8)
        {
            mat[column + 1, line] = true;
        }
        //left
        if (column - 1 >= 0)
        {
            mat[column - 1, line] = true;
        }
        //UP
        if (line + 1 < 8)
        {
            mat[column, line + 1] = true;
        }
        //down
        if (line - 1 >= 0)
        {
            mat[column, line - 1] = true;
        }
        //up-right
        if (column + 1 < 8 && line + 1 < 8)
        {
            mat[column + 1, line + 1] = true;
        }
        //up-left
        if (column - 1 >= 0 && line + 1 < 8)
        {
            mat[column - 1, line + 1] = true;
        }
        //down-right
        if (line - 1 >= 0 && column + 1 < 8)
        {
            mat[column + 1, line - 1] = true;
        }
        //down-left
        if (line - 1 >= 0 && column - 1 >= 0)
        {
            mat[column - 1, line - 1] = true;
        }

        if(qttMoves == 0)
        {
            //rook left castle
            if (chessBoard.boards[column - 1, line].GetComponentInChildren<Piece>() == null && chessBoard.boards[column - 2, line].GetComponentInChildren<Piece>() == null
                && chessBoard.boards[column - 3, line].GetComponentInChildren<Piece>() == null && chessBoard.boards[column - 4, line].GetComponentInChildren<Piece>() != null
                && chessBoard.boards[column - 4, line].GetComponentInChildren<Piece>().type == Piece.Type.Rook && chessBoard.boards[column - 4, line].GetComponentInChildren<Piece>().qttMoves == 0)
            {
                mat[column - 2, line] = true;
            }

            //rook right castle
            if (chessBoard.boards[5, 0].GetComponentInChildren<Piece>() == null && chessBoard.boards[6, 0].GetComponentInChildren<Piece>() == null
                && chessBoard.boards[7, 0].GetComponentInChildren<Piece>() != null && chessBoard.boards[7, line].GetComponentInChildren<Piece>().type == Piece.Type.Rook
                && chessBoard.boards[7, 0].GetComponentInChildren<Piece>().qttMoves == 0)
            {
                mat[6, line] = true;
            }
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
