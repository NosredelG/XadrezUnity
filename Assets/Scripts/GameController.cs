using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using Unity.VisualScripting;
using static UnityEngine.UI.Image;

public class GameController : MonoBehaviour
{
    GameObject selectedPiece;
    public GameObject selectionScreen;
    public GameObject checkScreen;
    public TextMeshProUGUI checkText;
    public TextMeshProUGUI playerTurnCheck;
    GameObject selectedPiecePromo;
    Cell actualCell;
    public ChessBoard chessBoard;
    public GameObject chessBoardCopy;
    public int turn = 0;
    public int testTurn = 0;

    public int line;
    public int column;

    public int selectPromoPiece;

    public GameObject graveyard;

    public List<GameObject> promotionPiecesWhite;
    public List<GameObject> promotionPiecesBlack;

    public Piece.Color actualPlayer;
    bool promotionSucess;

    public static GameController gc;

    // Start is called before the first frame update
    void Start()
    {
        gc = this;
        turn = 0;
        testTurn = 0;
        line = 0;
        column = 0;
        chessBoardCopy = Instantiate(chessBoard.gameObject);
        chessBoardCopy.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(testTurn < turn)
        {
            if(turn%2 == 0)
            {
                actualPlayer = Piece.Color.White;
                playerTurnCheck.text = "Peças brancas";                
            }
            else
            {
                actualPlayer = Piece.Color.Black;
                playerTurnCheck.text = "Peças pretas";                
            }
            if (TestCheck(Piece.Color.White))
            {
                checkText.text = "Rei branco!";                
                checkScreen.SetActive(true);
            }
            else if (TestCheck(Piece.Color.Black))
            {
                checkText.text = "Rei preto!";
                checkScreen.SetActive(true);
            }
            else
            {
                checkScreen.SetActive(false);
            }
            foreach(GameObject p in chessBoard.piecesInGame)
            {
                p.GetComponent<Piece>().testCheck = false;
            }
            CancelEnPassant();
            testTurn = turn;
        }
    
        if (Input.GetMouseButtonDown(0))
        {            
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (selectedPiece == null)
            {
                if (hit.collider != null && hit.collider.gameObject.tag == "Piece" && hit.collider.gameObject.GetComponent<Piece>().color == actualPlayer)
                {
                    selectedPiece = hit.collider.gameObject;                    
                    actualCell = hit.collider.gameObject.GetComponentInParent<Cell>();
                    hit.collider.gameObject.GetComponent<Piece>().isSelected = !hit.collider.gameObject.GetComponent<Piece>().isSelected;
                }
            }
            else
            {
                if (hit.collider != null && hit.collider.gameObject == selectedPiece && hit.collider.gameObject.tag == "Piece")
                {
                    ClearPossibleMoves();
                    selectedPiece.GetComponent<Piece>().isSelected = false;
                    actualCell = null;
                    selectedPiece = null;
                }
                else if (hit.collider != null && hit.collider.gameObject != selectedPiece && hit.collider.gameObject.tag == "Piece")
                {
                    if (hit.collider.gameObject.GetComponent<Piece>().color == selectedPiece.GetComponent<Piece>().color)
                    {
                        selectedPiece.GetComponent<Piece>().isSelected = false;
                        actualCell = hit.collider.gameObject.GetComponentInParent<Cell>();
                        hit.collider.gameObject.GetComponent<Piece>().isSelected = !hit.collider.gameObject.GetComponent<Piece>().isSelected;
                        selectedPiece = hit.collider.gameObject;                        
                    }
                    else
                    {
                        if (selectedPiece.GetComponent<Piece>().TestMoves(hit.collider.gameObject.GetComponent<Piece>().column, hit.collider.gameObject.GetComponent<Piece>().line)) 
                        {                           

                            line = hit.collider.gameObject.GetComponent<Piece>().line;
                            column = hit.collider.gameObject.GetComponent<Piece>().column;
                            
                            GetPiece(column, line, hit.collider.gameObject);

                            actualCell.ChangeEnableCollider();
                            turn++;                        
                            actualCell = null;
                            selectedPiece = null;
                            
                            ClearPossibleMoves();
                        }
                    }
                }
                else if (hit.collider != null && hit.collider.gameObject.tag == "Cell")
                {                 
                    line = hit.collider.gameObject.GetComponent<Cell>().line;
                    column = hit.collider.gameObject.GetComponent<Cell>().column;
                    for (int x = 0; x < 8; x++)
                    {
                        for (int y = 0; y < 8; y++)
                        {
                            if (chessBoard.boards[x, y] == hit.collider.gameObject)
                            {                                
                                if (selectedPiece.GetComponent<Piece>().TestMoves(x, y))
                                {
                                    if (selectedPiece.GetComponent<Piece>().type == Piece.Type.Pawn)
                                    {
                                        if (hit.collider.gameObject.GetComponentInChildren<Piece>() == null && x != selectedPiece.GetComponent<Piece>().column)
                                        {
                                            if (selectedPiece.GetComponent<Piece>().color == Piece.Color.White)
                                            {
                                                int i = y - 1;                                                
                                                GetPiece(chessBoard.boards[x, i].GetComponentInChildren<Piece>().gameObject);
                                            }
                                            else
                                            {
                                                int i = y + 1;                                                
                                                GetPiece(chessBoard.boards[x, i].GetComponentInChildren<Piece>().gameObject);
                                            }
                                        }
                                    }

                                    if (selectedPiece.GetComponent<Piece>().type == Piece.Type.King)
                                    {
                                        if (hit.collider.gameObject.GetComponentInChildren<Piece>() == null)
                                        {
                                            if (x == selectedPiece.GetComponent<Piece>().column + 2)
                                            {
                                                chessBoard.boards[7, y].GetComponentInChildren<Piece>().column = 5;
                                                chessBoard.boards[7, y].GetComponentInChildren<Piece>().ChangePosition(chessBoard.boards[5, y].gameObject);
                                                chessBoard.boards[7, y].GetComponentInChildren<Cell>().ChangeEnableCollider();
                                                chessBoard.boards[5, y].GetComponentInChildren<Cell>().ChangeEnableCollider();
                                            }
                                            if (x == selectedPiece.GetComponent<Piece>().column - 2)
                                            {
                                                chessBoard.boards[0, y].GetComponentInChildren<Piece>().column = 3;
                                                chessBoard.boards[0, y].GetComponentInChildren<Piece>().ChangePosition(chessBoard.boards[3, y].gameObject);
                                                chessBoard.boards[3, y].GetComponentInChildren<Cell>().ChangeEnableCollider();
                                                chessBoard.boards[0, y].GetComponentInChildren<Cell>().ChangeEnableCollider();
                                            }
                                        }
                                    }                                                                        
                                    
                                    selectedPiece.GetComponent<Piece>().line = y;
                                    selectedPiece.GetComponent<Piece>().column = x;
                                    selectedPiece.GetComponent<Piece>().ChangePosition(hit.collider.gameObject);
                                    selectedPiece.GetComponent<Piece>().IncreaseMovements();
                                    CheckPromotion(selectedPiece, y);
                                    actualCell.ChangeEnableCollider();
                                    hit.collider.gameObject.GetComponent<Cell>().ChangeEnableCollider();
                                    turn++;
                                    actualCell = null;
                                    selectedPiece = null;

                                    ClearPossibleMoves();
                                }
                            }
                        }
                    }
                }
            }            
        }
    }

    public void GetPiece(int vertical, int horizontal, GameObject piece)
    {        
        selectedPiece.GetComponent<Piece>().isSelected = false;
        selectedPiece.GetComponent<Piece>().line = horizontal;
        selectedPiece.GetComponent<Piece>().column = vertical;
        selectedPiece.GetComponent<Piece>().ChangePosition(chessBoard.boards[vertical, horizontal]);
        selectedPiece.GetComponent<Piece>().IncreaseMovements();
        CheckPromotion(selectedPiece, horizontal);
        piece.transform.SetParent(graveyard.transform);
        piece.transform.position = graveyard.transform.position;    
        piece.SetActive(false);
    }

    public void GetPiece(GameObject piece)
    {
        piece.GetComponentInParent<Cell>().ChangeEnableCollider();
        piece.transform.SetParent(graveyard.transform);
        piece.transform.position = graveyard.transform.position;
        piece.SetActive(false);
    }

    public void CheckPromotion(GameObject piece, int horizontal)
    {
        if(piece.GetComponent<Piece>().type == Piece.Type.Pawn)
        {
            if(piece.GetComponent<Piece>().color == Piece.Color.White && horizontal == 7)
            {
                selectedPiecePromo = piece;
                selectionScreen.SetActive(true);
            }
            if (piece.GetComponent<Piece>().color == Piece.Color.Black && horizontal == 0)
            {
                selectedPiecePromo = piece;
                selectionScreen.SetActive(true);
            }
        }
    }

    public void SelectPiecePromo(int pieceId)
    {
        PawnPromotion(selectedPiecePromo, pieceId);
        selectionScreen.SetActive(false);
    }

    public void PawnPromotion(GameObject piece, int id)
    {
        if (piece.GetComponent<Piece>().color == Piece.Color.White)
        {
            foreach (GameObject p in promotionPiecesWhite)
            {
                switch (id) 
                {
                    case 0:
                        if (p.GetComponent<Piece>().type == Piece.Type.Queen)
                        {
                            GameObject p1 = Instantiate(p, chessBoard.boards[column, line].transform.position, chessBoard.boards[column, line].transform.transform.rotation);
                            p1.transform.SetParent(chessBoard.boards[column, line].transform);
                            p1.GetComponent<Piece>().column = piece.GetComponent<Piece>().column;
                            p1.GetComponent<Piece>().line = piece.GetComponent<Piece>().line;
                            chessBoard.piecesInGame.Add(p1);
                        }
                        break;
                    case 1:
                        if (p.GetComponent<Piece>().type == Piece.Type.Bishop)
                        {
                            GameObject p1 = Instantiate(p, chessBoard.boards[column, line].transform.position, chessBoard.boards[column, line].transform.transform.rotation);
                            p1.transform.SetParent(chessBoard.boards[column, line].transform);
                            p1.GetComponent<Piece>().column = piece.GetComponent<Piece>().column;
                            p1.GetComponent<Piece>().line = piece.GetComponent<Piece>().line;
                            chessBoard.piecesInGame.Add(p1);
                        }
                        break;

                    case 2:
                        if (p.GetComponent<Piece>().type == Piece.Type.Knight)
                        {
                            GameObject p1 = Instantiate(p, chessBoard.boards[column, line].transform.position, chessBoard.boards[column, line].transform.transform.rotation);
                            p1.transform.SetParent(chessBoard.boards[column, line].transform);
                            p1.GetComponent<Piece>().column = piece.GetComponent<Piece>().column;
                            p1.GetComponent<Piece>().line = piece.GetComponent<Piece>().line;
                            chessBoard.piecesInGame.Add(p1);
                        }
                        break;
                    case 3:
                        if (p.GetComponent<Piece>().type == Piece.Type.Rook)
                        {
                            GameObject p1 = Instantiate(p, chessBoard.boards[column, line].transform.position, chessBoard.boards[column, line].transform.transform.rotation);
                            p1.transform.SetParent(chessBoard.boards[column, line].transform);
                            p1.GetComponent<Piece>().column = piece.GetComponent<Piece>().column;
                            p1.GetComponent<Piece>().line = piece.GetComponent<Piece>().line;
                            chessBoard.piecesInGame.Add(p1);
                        }
                        break;
                }                                
            }
        }

        if (piece.GetComponent<Piece>().color == Piece.Color.Black)
        {
            foreach (GameObject p in promotionPiecesBlack)
            {
                switch (id)
                {
                    case 0:
                        if (p.GetComponent<Piece>().type == Piece.Type.Queen)
                        {
                            GameObject p1 = Instantiate(p, chessBoard.boards[column, line].transform.position, chessBoard.boards[column, line].transform.transform.rotation);
                            p1.transform.SetParent(chessBoard.boards[column, line].transform);
                            p1.GetComponent<Piece>().column = piece.GetComponent<Piece>().column;
                            p1.GetComponent<Piece>().line = piece.GetComponent<Piece>().line;
                            chessBoard.piecesInGame.Add(p1);
                        }
                        break;

                    case 1:
                        if (p.GetComponent<Piece>().type == Piece.Type.Bishop)
                        {
                            GameObject p1 = Instantiate(p, chessBoard.boards[column, line].transform.position, chessBoard.boards[column, line].transform.transform.rotation);
                            p1.transform.SetParent(chessBoard.boards[column, line].transform);
                            p1.GetComponent<Piece>().column = piece.GetComponent<Piece>().column;
                            p1.GetComponent<Piece>().line = piece.GetComponent<Piece>().line;
                            chessBoard.piecesInGame.Add(p1);
                        }
                        break;

                    case 2:
                        if (p.GetComponent<Piece>().type == Piece.Type.Knight)
                        {
                            GameObject p1 = Instantiate(p, chessBoard.boards[column, line].transform.position, chessBoard.boards[column, line].transform.transform.rotation);
                            p1.transform.SetParent(chessBoard.boards[column, line].transform);
                            p1.GetComponent<Piece>().column = piece.GetComponent<Piece>().column;
                            p1.GetComponent<Piece>().line = piece.GetComponent<Piece>().line;
                            chessBoard.piecesInGame.Add(p1);
                        }
                        break;
                    case 3:
                        if (p.GetComponent<Piece>().type == Piece.Type.Rook)
                        {
                            GameObject p1 = Instantiate(p, chessBoard.boards[column, line].transform.position, chessBoard.boards[column, line].transform.transform.rotation);
                            p1.transform.SetParent(chessBoard.boards[column, line].transform);
                            p1.GetComponent<Piece>().column = piece.GetComponent<Piece>().column;
                            p1.GetComponent<Piece>().line = piece.GetComponent<Piece>().line;
                            chessBoard.piecesInGame.Add(p1);
                        }
                        break;
                }
            }
        }

        piece.transform.SetParent(graveyard.transform);
        piece.transform.position = graveyard.transform.position;
        piece.SetActive(false);
    }

    public void ClearPossibleMoves()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                chessBoard.boards[x, y].GetComponent<Cell>().light.SetActive(false);
            }
        }
    }

    public bool TestCell(int vertical, int horizontal, Piece piece)
    {
        if(chessBoard.boards[vertical, horizontal].GetComponentInChildren<Piece>() != null)
        {
            if(chessBoard.boards[vertical, horizontal].GetComponentInChildren<Piece>().color == piece.color)
            {
                return false;
            }                       
            else { return true; }
        }        
        else { return true; }
    }

    public void PossiblesMoves(bool[,] mat, Piece piece)
    {        
        for(int x = 0;x < 8; x++)
        {
            for(int y = 0;y < 8; y++)
            {
                if (mat[x, y] && TestCell(x, y, piece))
                {
                    chessBoard.boards[x, y].GetComponent<Cell>().light.SetActive(true);
                }
                else
                {
                    chessBoard.boards[x, y].GetComponent<Cell>().light.SetActive(false);
                }
            }
        }
    }

    public bool TestCheck(Piece.Color color)
    {
        bool[,] mat = new bool[8, 8];
        foreach (GameObject piece in chessBoard.piecesInGame)
        {
            if(piece.GetComponent<Piece>().color != color)
            {                
                if (piece.activeSelf)
                {
                    mat = piece.GetComponent<Piece>().PossibleMoves();
                }

                foreach (GameObject p in chessBoard.piecesInGame)
                {
                    if (p.GetComponent<Piece>().type == Piece.Type.King && p.GetComponent<Piece>().color == color)
                    {
                        if (mat[p.GetComponent<Piece>().column, p.GetComponent<Piece>().line])
                        {
                            return true;
                        }
                    }
                }                
            }
        }
        
        return false;
    }     

    public void CancelEnPassant()
    {
        foreach (GameObject p in chessBoard.pawns)
        {
            int temp = p.GetComponent<Pawn>().turnEnPassant + 2;
            if (p.GetComponent<Pawn>().enPassantVunerable && turn >= temp)
            {
                p.GetComponent<Pawn>().enPassantVunerable = false;
            }
        }
    }
}
