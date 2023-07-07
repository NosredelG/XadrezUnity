using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{

    bool[,] mat = new bool[8, 8];
    public Bishop(int qteMovimentos, ChessBoard chessBoard, int positionX, int positionY) : 
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


        int column = base.column;
        int line = base.line;
        //up-right
        while (column < 7 && line < 7)
        {
            if (chessBoard.boards[column + 1, line + 1].GetComponentInChildren<Piece>() == null)
            {
                mat[column + 1, line + 1] = true;
                line++;
                column++;
            }
            else
            {
                if (chessBoard.boards[column + 1, line + 1].GetComponentInChildren<Piece>().color != color)
                {
                    mat[column + 1, line + 1] = true;
                }
                else
                {
                    mat[column + 1, line + 1] = false;
                }
                line = 8;
                column = 8;
            }
        }

        column = base.column;
        line = base.line;
        //up-left
        while (column >= 1 && line < 7)
        {
            if (chessBoard.boards[column - 1, line + 1].GetComponentInChildren<Piece>() == null)
            {
                mat[column - 1, line + 1] = true;
                line++;
                column--;
            }
            else
            {
                if (chessBoard.boards[column - 1, line + 1].GetComponentInChildren<Piece>().color != color)
                {
                    mat[column - 1, line + 1] = true;
                }
                else
                {
                    mat[column - 1, line + 1] = false;
                }
                line = 8;
                column = 0;
            }
        }

        column = base.column;
        line = base.line;
        //down-right
        while (line >= 1 && column < 7)
        {

            if (chessBoard.boards[column + 1, line - 1].GetComponentInChildren<Piece>() == null)
            {
                mat[column + 1, line - 1] = true;
                line--;
                column++;
            }
            else
            {
                if (chessBoard.boards[column + 1, line - 1].GetComponentInChildren<Piece>().color != color)
                {
                    mat[column + 1, line - 1] = true;
                }
                else
                {
                    mat[column + 1, line - 1] = false;
                }
                line = 0;
                column = 8;
            }
        }

        column = base.column;
        line = base.line;
        //down-left
        while (line >= 1 && column >= 1)
        {
            if (chessBoard.boards[column - 1, line - 1].GetComponentInChildren<Piece>() == null)
            {
                mat[column - 1, line - 1] = true;
                line--;
                column--;
            }
            else
            {
                if (chessBoard.boards[column - 1, line - 1].GetComponentInChildren<Piece>().color != color)
                {
                    mat[column - 1, line - 1] = true;
                }
                else
                {
                    mat[column - 1, line - 1] = false;
                }
                line = 0;
                column = 0;
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
