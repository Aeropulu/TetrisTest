using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceDispenser : MonoBehaviour
{
    [SerializeField] private List<Piece> _pieceList = new List<Piece>();

    public Piece GetNext()
    {
        if (_pieceList.Count <= 0)
        {
            return null;
        }

        int randomIndex = Random.Range(0, _pieceList.Count);
        return _pieceList[randomIndex];
    }
}
