using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Pawn : Piece
{
    bool[,] mat;
    public bool enPassantVunerable = false;
    public int turnEnPassant = 0;
    public Pawn(int qteMovimentos, ChessBoard chessBoard, int positionX, int positionY) : base(qteMovimentos, chessBoard, positionX, positionY)
    {

    }

    private void Start()
    {
        mat = new bool[8, 8];
        chessBoard = GameController.gc.chessBoard;
        enPassantVunerable = false;
        turnEnPassant = 0;
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

        if (color == Color.White)
        {
            //walk 1 cell
            if (line + 1 < 8 && chessBoard.boards[column, line + 1].GetComponentInChildren<Piece>() == null)
            {
                mat[column, line + 1] = true;
            }
            //walk 2 cell first move
            if (qttMoves == 0 && chessBoard.boards[column, 3].GetComponentInChildren<Piece>() == null && chessBoard.boards[column, line + 1].GetComponentInChildren<Piece>() == null)
            {
                mat[column, 3] = true;
                enPassantVunerable = true;
                turnEnPassant = GameController.gc.turn;
            }
            //get piece right
            if (column + 1 < 8 && line + 1 < 8 && chessBoard.boards[column + 1, line + 1].GetComponentInChildren<Piece>() != null && chessBoard.boards[column + 1, line + 1].GetComponentInChildren<Piece>().color != color)
            {
                mat[column + 1, line + 1] = true;
            }
            //get piece left
            if (column - 1 >= 0 && line + 1 < 8 && chessBoard.boards[column - 1, line + 1].GetComponentInChildren<Piece>() != null && chessBoard.boards[column - 1, line + 1].GetComponentInChildren<Piece>().color != color)
            {
                mat[column - 1, line + 1] = true;
            }
            //get piece right en Passant
            if (column + 1 < 8 && line + 1 < 8 && chessBoard.boards[column + 1, line].GetComponentInChildren<Piece>() != null && chessBoard.boards[column + 1, line].GetComponentInChildren<Piece>().color != color
                && chessBoard.boards[column + 1, line].GetComponentInChildren<Piece>().type == Type.Pawn && chessBoard.boards[column + 1, line].GetComponentInChildren<Pawn>().enPassantVunerable
                && chessBoard.boards[column + 1, line + 1].GetComponentInChildren<Piece>() == null)
            {
                mat[column + 1, line + 1] = true;
            }
            //get piece left en Passant
            if (line + 1 < 8 && column - 1 >= 0 && chessBoard.boards[column - 1, line].GetComponentInChildren<Piece>() != null && chessBoard.boards[column - 1, line].GetComponentInChildren<Piece>().color != color
                 && chessBoard.boards[column - 1, line].GetComponentInChildren<Piece>().type == Type.Pawn && chessBoard.boards[column - 1, line].GetComponentInChildren<Pawn>().enPassantVunerable
                 && chessBoard.boards[column - 1, line + 1].GetComponentInChildren<Piece>() == null)
            {
                mat[column - 1, line + 1] = true;
            }            
        }
        if (color == Color.Black)
        {
            //walk 1 cell
            if (line - 1 >= 0 && chessBoard.boards[column, line - 1].GetComponentInChildren<Piece>() == null)
            {
                mat[column, line - 1] = true;
            }
            //walk 2 cell first
            if (qttMoves == 0 && chessBoard.boards[line, 4].GetComponentInChildren<Piece>() == null && chessBoard.boards[column, line - 1].GetComponentInChildren<Piece>() == null)
            {
                mat[column, 4] = true;
                enPassantVunerable = true;
                turnEnPassant = GameController.gc.turn;
            }
            //get piece right
            if (column + 1 < 8 && line - 1 >= 0 && chessBoard.boards[column + 1, line - 1].GetComponentInChildren<Piece>() != null && chessBoard.boards[column + 1, line - 1].GetComponentInChildren<Piece>().color != color)
            {
                mat[column + 1, line - 1] = true;
            }
            //get piece left
            if (column - 1 >= 0 && line - 1 >= 0 && chessBoard.boards[column - 1, line - 1].GetComponentInChildren<Piece>() != null && chessBoard.boards[column - 1, line - 1].GetComponentInChildren<Piece>().color != color)
            {
                mat[column - 1, line - 1] = true;
            }
            //get piece right en Passant
            if (column + 1 < 8 && line - 1 >= 0 && chessBoard.boards[column + 1, line].GetComponentInChildren<Piece>() != null && chessBoard.boards[column + 1, line].GetComponentInChildren<Piece>().color != color
                && chessBoard.boards[column + 1, line].GetComponentInChildren<Piece>().type == Type.Pawn && chessBoard.boards[column + 1, line].GetComponentInChildren<Pawn>().enPassantVunerable
                && chessBoard.boards[column + 1, line - 1].GetComponentInChildren<Piece>() == null)
            {
                mat[column + 1, line - 1] = true;
            }
            //get piece left en Passant
            if (line - 1 >= 0 && column - 1 >= 0 && chessBoard.boards[column - 1, line].GetComponentInChildren<Piece>() != null && chessBoard.boards[column - 1, line].GetComponentInChildren<Piece>().color != color
                 && chessBoard.boards[column - 1, line].GetComponentInChildren<Piece>().type == Type.Pawn && chessBoard.boards[column - 1, line].GetComponentInChildren<Pawn>().enPassantVunerable
                 && chessBoard.boards[column - 1, line - 1].GetComponentInChildren<Piece>() == null)
            {
                mat[column - 1, line - 1] = true;
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
