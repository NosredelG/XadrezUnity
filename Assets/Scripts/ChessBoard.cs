using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class ChessBoard : MonoBehaviour
{
    public GameObject cellPrefabWhite;  // Prefab da célula do tabuleiro
    public GameObject cellPrefabBlack;  // Prefab da célula do tabuleiro
    public int Line { get; private set; }
    public int Column { get; private set; }

    public GameObject[,] boards = new GameObject[8,8];

    private GameObject cellActual; 

    public List<GameObject> pieces;
    public List<GameObject> piecesInGame;

    public List<GameObject> pawns;

    public ChessBoard(int line, int column) 
    { 
        Line = line;
        Column = column;        
    }

    private void Awake()
    {
        Line = 8; 
        Column = 8;
        boards = new GameObject[8, 8];
    }

    void Start()
    {
        // Criação do tabuleiro
        for (int line = 0; line < Line; line++)
        {
            for (int column = 0; column < Column; column++)
            {
                if ((line + column) %2 == 1) 
                {
                    cellActual = cellPrefabWhite;
                }
                else
                {
                    cellActual = cellPrefabBlack;
                }
                GameObject cell = Instantiate(cellActual, transform);  // Instancia a célula
                cell.transform.position = new Vector2(column, line);  // Define a posição da célula no tabuleiro
                boards[column, line] = cell;     
                cell.gameObject.name = "Cell: linha = " + line.ToString() + ", coluna = " + column.ToString();
                cell.GetComponent<Cell>().line = line;
                cell.GetComponent<Cell>().column = column;
            }            
        }        

        transform.position = new Vector3(-2.5f, -2.5f, 0);
        InstantiatePieces();
    }

    public void InstantiatePieces()
    {
        foreach(GameObject piece in pieces)
        {
            if (piece.GetComponent<Piece>().type == Piece.Type.King && piece.GetComponent<Piece>().color == Piece.Color.White)
            {
                GameObject p1 = Instantiate(piece, boards[4, 0].transform.position, transform.rotation);
                p1.transform.SetParent(boards[4, 0].transform);
                p1.GetComponent<Piece>().line = 0;
                p1.GetComponent<Piece>().column = 4;
                piecesInGame.Add(p1);
            }
            if (piece.GetComponent<Piece>().type == Piece.Type.King && piece.GetComponent<Piece>().color == Piece.Color.Black)
            {
                GameObject p1 = Instantiate(piece, boards[4, 7].transform.position, transform.rotation);
                p1.transform.SetParent(boards[4, 7].transform);
                p1.GetComponent<Piece>().line = 7;
                p1.GetComponent<Piece>().column = 4;
                piecesInGame.Add(p1);
            }

            if (piece.GetComponent<Piece>().type == Piece.Type.Queen && piece.GetComponent<Piece>().color == Piece.Color.White)
            {
                GameObject p1 = Instantiate(piece, boards[3, 0].transform.position, transform.rotation);
                p1.transform.SetParent(boards[3, 0].transform);
                p1.GetComponent<Piece>().column = 3;
                p1.GetComponent<Piece>().line = 0;
                piecesInGame.Add(p1);
            }
            if (piece.GetComponent<Piece>().type == Piece.Type.Queen && piece.GetComponent<Piece>().color == Piece.Color.Black)
            {
                GameObject p1 = Instantiate(piece, boards[3, 7].transform.position, transform.rotation);
                p1.transform.SetParent(boards[3, 7].transform);
                p1.GetComponent<Piece>().column = 3;
                p1.GetComponent<Piece>().line = 7;
                piecesInGame.Add(p1);
            }

            if (piece.GetComponent<Piece>().type == Piece.Type.Bishop && piece.GetComponent<Piece>().color == Piece.Color.White)
            {
                GameObject p1 = Instantiate(piece, boards[2, 0].transform.position, transform.rotation);
                p1.transform.SetParent(boards[2, 0].transform);
                p1.GetComponent<Piece>().column = 2;
                p1.GetComponent<Piece>().line = 0;
                GameObject p2 = Instantiate(piece, boards[5, 0].transform.position, transform.rotation);
                p2.transform.SetParent(boards[5, 0].transform);
                p2.GetComponent<Piece>().column = 5;
                p2.GetComponent<Piece>().line = 0;
                piecesInGame.Add(p1);
                piecesInGame.Add(p2);
            }
            if (piece.GetComponent<Piece>().type == Piece.Type.Bishop && piece.GetComponent<Piece>().color == Piece.Color.Black)
            {
                GameObject p1 = Instantiate(piece, boards[2, 7].transform.position, transform.rotation);
                p1.transform.SetParent(boards[2, 7].transform);
                p1.GetComponent<Piece>().column = 2;
                p1.GetComponent<Piece>().line = 7;
                GameObject p2 = Instantiate(piece, boards[5, 7].transform.position, transform.rotation);
                p2.transform.SetParent(boards[5, 7].transform);
                p2.GetComponent<Piece>().column = 5;
                p2.GetComponent<Piece>().line = 7;
                piecesInGame.Add(p1);
                piecesInGame.Add(p2);
            }

            if (piece.GetComponent<Piece>().type == Piece.Type.Knight && piece.GetComponent<Piece>().color == Piece.Color.White)
            {
                GameObject p1 = Instantiate(piece, boards[1, 0].transform.position, transform.rotation);
                p1.transform.SetParent(boards[1, 0].transform);
                p1.GetComponent<Piece>().column = 1;
                p1.GetComponent<Piece>().line = 0;
                GameObject p2 = Instantiate(piece, boards[6, 0].transform.position, transform.rotation);
                p2.transform.SetParent(boards[6, 0].transform);
                p2.GetComponent<Piece>().column = 6;
                p2.GetComponent<Piece>().line = 0;
                piecesInGame.Add(p1);
                piecesInGame.Add(p2);
            }
            if (piece.GetComponent<Piece>().type == Piece.Type.Knight && piece.GetComponent<Piece>().color == Piece.Color.Black)
            {
                GameObject p1 = Instantiate(piece, boards[1, 7].transform.position, transform.rotation);
                p1.transform.SetParent(boards[1, 7].transform);
                p1.GetComponent<Piece>().column = 1;
                p1.GetComponent<Piece>().line = 7;
                GameObject p2 = Instantiate(piece, boards[6, 7].transform.position, transform.rotation);
                p2.transform.SetParent(boards[6, 7].transform);
                p2.GetComponent<Piece>().column = 6;
                p2.GetComponent<Piece>().line = 7;
                piecesInGame.Add(p1);
                piecesInGame.Add(p2);
            }

            if (piece.GetComponent<Piece>().type == Piece.Type.Rook && piece.GetComponent<Piece>().color == Piece.Color.White)
            {
                GameObject p1 = Instantiate(piece, boards[0, 0].transform.position, transform.rotation);
                p1.transform.SetParent(boards[0, 0].transform);
                p1.GetComponent<Piece>().column = 0;
                p1.GetComponent<Piece>().line = 0;
                GameObject p2 = Instantiate(piece, boards[7, 0].transform.position, transform.rotation);
                p2.transform.SetParent(boards[7, 0].transform);
                p2.GetComponent<Piece>().column = 7;
                p2.GetComponent<Piece>().line = 0;
                piecesInGame.Add(p1);
                piecesInGame.Add(p2);
            }
            if (piece.GetComponent<Piece>().type == Piece.Type.Rook && piece.GetComponent<Piece>().color == Piece.Color.Black)
            {
                GameObject p1 = Instantiate(piece, boards[0, 7].transform.position, transform.rotation);
                p1.transform.SetParent(boards[0, 7].transform);
                p1.GetComponent<Piece>().column = 0;
                p1.GetComponent<Piece>().line = 7;
                GameObject p2 = Instantiate(piece, boards[7, 7].transform.position, transform.rotation);
                p2.transform.SetParent(boards[7, 7].transform);
                p2.GetComponent<Piece>().column = 7;
                p2.GetComponent<Piece>().line = 7;
                piecesInGame.Add(p1);
                piecesInGame.Add(p2);
            }

            if (piece.GetComponent<Piece>().type == Piece.Type.Pawn && piece.GetComponent<Piece>().color == Piece.Color.White)
            {
                for (int i = 0; i < 8; i++)
                {
                    GameObject p1 = Instantiate(piece, boards[i, 1].transform.position, transform.rotation);
                    p1.transform.SetParent(boards[i, 1].transform);
                    p1.GetComponent<Piece>().line = 1;
                    p1.GetComponent<Piece>().column = i;
                    pawns.Add(p1);
                    piecesInGame.Add(p1);
                }
            }
            if (piece.GetComponent<Piece>().type == Piece.Type.Pawn && piece.GetComponent<Piece>().color == Piece.Color.Black)
            {
                for (int i = 0; i < 8; i++)
                {
                    GameObject p1 = Instantiate(piece, boards[i, 6].transform.position, transform.rotation);
                    p1.transform.SetParent(boards[i, 6].transform);
                    p1.GetComponent<Piece>().line = 6;
                    p1.GetComponent<Piece>().column = i;
                    pawns.Add(p1);
                    piecesInGame.Add(p1);
                }
            }

        }
    }
}
