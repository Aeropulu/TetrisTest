using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PieceDispenser : MonoBehaviour
{
    [SerializeField] private List<Piece> _pieceList = new List<Piece>();
    [SerializeField] private UnityEvent _nextPieceDrawnEvent = new UnityEvent();
    private Piece _nextPiece = null;

    public Piece NextPiece { get { return _nextPiece; } }

    private void Start()
    {
        DrawNextPiece();
    }

    public Piece GetNext()
    {
        if (_nextPiece == null)
        {
            DrawNextPiece();
        }
        Piece returnPiece = _nextPiece;
        DrawNextPiece();
        return returnPiece;
    }

    private void DrawNextPiece()
    {
		if (_pieceList.Count <= 0)
		{
			return;
		}

		int randomIndex = Random.Range(0, _pieceList.Count);
        _nextPiece = _pieceList[randomIndex];
        _nextPiece.ResetRotation();
        _nextPieceDrawnEvent.Invoke();
	}
}
