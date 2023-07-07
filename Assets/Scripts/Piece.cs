using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    bool[,] mat = new bool[8, 8];
    public Color color;
    public Type type;
    public int qttMoves { get; protected set; }
    public ChessBoard chessBoard { get; protected set; }

    public int line;
    public int column;
    public bool isSelected;
    public GameObject light;
    public bool showMoves;
    public bool testCheck;

    public Piece(int qteMovimentos, ChessBoard chessBoard, int line, int column)
    {        
        this.qttMoves = 0;
        this.chessBoard = chessBoard;
        this.line = line;
        this.column = column;
    }

    public enum Color
    {
        White,
        Black
    }    

    public enum Type
    {
        King,
        Queen,
        Rook,
        Bishop,
        Knight,
        Pawn
    }

    private void Start()
    {
        chessBoard = GameController.gc.chessBoard;
        mat = new bool[8, 8];
    }

    private void Update()
    {
        if (isSelected)
        {
            light.SetActive(true);
            if (!showMoves)
            {
                column = GetComponentInParent<Cell>().column;
                line = GetComponentInParent<Cell>().line;                
                
                GameController.gc.PossiblesMoves(PossibleMoves(), this);
                showMoves = true;
            }
            
        }
        else
        {
            light.SetActive(false);
            showMoves = false;
        }       

    }

    public void IncreaseMovements() { qttMoves++; }

    public void DecreaseMovements() { qttMoves--; }

    public void ChangePosition(GameObject gameObj)
    {
        isSelected = false;
        transform.position = gameObj.transform.position;
        transform.SetParent(gameObj.transform);        
    }

    public abstract bool[,] PossibleMoves();

    public abstract bool TestMoves(int vertical, int horizontal);  
}
