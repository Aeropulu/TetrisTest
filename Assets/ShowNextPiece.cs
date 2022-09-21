using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowNextPiece : MonoBehaviour
{
    [SerializeField] private PieceDispenser _pieceDispenser;
    [SerializeField] private GameObject _blockPrefab;
    private List<Block> _blocks = new List<Block>(4);

    private void Awake()
    {
        if (_pieceDispenser == null)
        {
            Debug.LogError("No piece dispenser on ShowNextPiece.");
            return;
        }

        if (_blockPrefab == null)
        {
            Debug.LogError("No block prefab on ShowNextPiece.");
            return;
        }
        
    }

    public void UpdatePiece()
    {
        Piece piece = _pieceDispenser.NextPiece;
        int blockCountDiff = piece.BlockList.Count - _blocks.Count;
        if (blockCountDiff > 0)
        {
            AddBlocks(blockCountDiff);
        }

        int blockCount = piece.BlockList.Count;
        for (int i = 0; i < blockCount; i++)
        {
            Block block = _blocks[i];
            if (block.gameObject.activeSelf == false)
            {
                block.gameObject.SetActive(true);
            }

            block.SetColor(piece.Color);
            Vector2Int blockPosition = piece.BlockList[i];
            block.transform.localPosition = new Vector3(blockPosition.x, blockPosition.y);
        }

        if (blockCountDiff < 0)
        {
            // Disable supplementary blocks;
            for (int i = blockCount; i < blockCount - blockCountDiff; i++)
            {
                _blocks[i].gameObject.SetActive(false);
            }
        }
    }

    private void AddBlocks(int count)
    {
		for (int i = 0; i < count; i++)
		{
			GameObject blockObject = Instantiate(_blockPrefab, transform);
			Block block = blockObject.GetComponent<Block>();
			_blocks.Add(block);
		}
	}
}
