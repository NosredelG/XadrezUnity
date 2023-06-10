using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Color color;
    public Type type;
    public int qttMoves { get; protected set; }
    public ChessBoard tab { get; protected set; }

    public Vector2 position;

    public Piece(int qteMovimentos, ChessBoard tab, Vector2 position)
    {        
        this.qttMoves = 0;
        this.tab = tab;
        this.position = position;
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

    public void IncreaseMovements() { qttMoves++; }

    public void DecreaseMovements() { qttMoves--; }
}
