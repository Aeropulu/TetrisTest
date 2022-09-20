using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePiece : MonoBehaviour
{
	[SerializeField] private TetrisField _field;
	[SerializeField] private PieceDispenser _pieceDispenser;
	[SerializeField] private float _moveDownInterval = 0.1f;
	[SerializeField] private GameObject _blockPrefab;

	private List<Block> _blocks = null;
	private Vector2Int _currentPiecePosition = Vector2Int.zero;
	private float _nextMoveDownTime = 0.0f;

	private Piece _currentPiece = null;

	void Start()
	{
		if (_field == null)
		{
			Debug.LogError("Tetris Field not assigned on MovePiece");
			return;
		}

		if (_pieceDispenser == null)
		{
			Debug.LogError("Piece Dispenser not assigned on MovePiece");
			return;
		}

		_blocks = new List<Block>(4);

		_currentPiecePosition = _field.DropPoint;
		MakePieceGameObjects();

		
		_nextMoveDownTime = Time.time + _moveDownInterval;
	}


	void Update()
	{
		if (Time.time > _nextMoveDownTime)
		{
			_nextMoveDownTime += _moveDownInterval;
			MoveDown();
		}

		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			_nextMoveDownTime = Time.time + _moveDownInterval;
			MoveDown();
		}

		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			MoveSide(1);
		}

		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			MoveSide(-1);
		}

		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			Rotate();
		}
	}

	private void MoveDown()
	{
		Vector2Int nextPosition = _currentPiecePosition + Vector2Int.down;

		if (CheckIfBlocked(_currentPiece.BlockList, nextPosition) || CheckIfBottom(_currentPiece.BlockList, nextPosition))
		{
			PlacePiece();
		}
		else
		{
			_currentPiecePosition = nextPosition;
			UpdateBlockObjects();
		}
	}
	
	private void MoveSide(int amount)
	{
		Vector2Int nextPosition = _currentPiecePosition + new Vector2Int(amount, 0);

		if (CheckIfBlocked(_currentPiece.BlockList, nextPosition))
		{
			return;
		}

		nextPosition = KeepInsideField(_currentPiece.BlockList, nextPosition);
		_currentPiecePosition = nextPosition;
		UpdateBlockObjects();
	}

	private void Rotate()
	{
		List<Vector2Int> rotatedList = _currentPiece.RotatedBlockList;
		Vector2Int nextPosition = KeepInsideField(rotatedList, _currentPiecePosition);
		if (CheckIfBlocked(rotatedList, nextPosition) || CheckIfBottom(rotatedList, nextPosition))
		{
			// Can't rotate here.
			return;
		}

		_currentPiecePosition = nextPosition;
		_currentPiece.Rotate();
		UpdateBlockObjects();
	}

	private bool CheckIfBottom(List<Vector2Int> blockList, Vector2Int piecePosition)
	{
		int blockCount = blockList.Count;
		for (int i = 0; i < blockCount; i++)
		{
			Vector2Int position = blockList[i] + piecePosition;
			if (position.y < _field.yMin)
			{
				return true;
			}
		}

		return false;
	}

	private bool CheckIfBlocked(List<Vector2Int> blockList, Vector2Int piecePosition)
	{
		int blockCount = blockList.Count;
		for (int i = 0; i < blockCount; i++)
		{
			Vector2Int blockPosition = blockList[i] + piecePosition;
			if (_field.GetBlock(blockPosition) != null)
			{
				return true;
			}
		}

		return false;
	}

	private Vector2Int KeepInsideField(List<Vector2Int> blockList, Vector2Int piecePosition)
	{
		int offset = 0;
		int blockCount = blockList.Count;
		for (int i = 0; i < blockCount; i++)
		{
			Vector2Int position = blockList[i] + piecePosition;

			if (position.x < _field.xMin)
			{
				offset = Mathf.Max(offset, _field.xMin - position.x);
			}

			if (position.x > _field.xMax)
			{
				offset = Mathf.Min(offset, _field.xMax - position.x);
			}
		}

		return piecePosition + new Vector2Int(offset, 0);
	}

	private void UpdateBlockObjects()
	{
		int blockCount = _blocks.Count;
		for (int i = 0; i < blockCount; i++)
		{
			Block block = _blocks[i];
			block.transform.position = GridToWorldPosition(_currentPiece.BlockList[i]);
		}
	}

	private async void PlacePiece()
	{
		int blockCount = _blocks.Count;
		int bottomLine = _field.yMax;
		int topLine = 0;

		for (int i = 0; i < blockCount; i++)
		{
			Block block = _blocks[i];
			Vector2Int position = _currentPiece.BlockList[i] + _currentPiecePosition;
			_field.SetBlock(position, block);

			bottomLine = Mathf.Min(bottomLine, position.y);
			topLine = Mathf.Max(topLine, position.y);
		}

		_blocks.Clear();
		await _field.CheckAndClearLines(bottomLine, topLine);
		_currentPiecePosition = _field.DropPoint;
		MakePieceGameObjects();
	}

	private void MakePieceGameObjects()
	{
		_currentPiece = _pieceDispenser.GetNext();

		if (_blockPrefab == null || _currentPiece.BlockList == null)
		{
			return;
		}

		_blocks.Clear();

		foreach(Vector2Int blockPosition in _currentPiece.BlockList)
		{
			GameObject blockObject = Instantiate(_blockPrefab);
			Block block = blockObject.GetComponent<Block>();
			_blocks.Add(block);
			SpriteRenderer spriteRenderer = blockObject.GetComponent<SpriteRenderer>();
			spriteRenderer.color = _currentPiece.Color;

			blockObject.transform.position = GridToWorldPosition(blockPosition);
		}
	}

	private Vector3 GridToWorldPosition(Vector2Int blockPosition)
	{
		return new Vector3(_currentPiecePosition.x + blockPosition.x, _currentPiecePosition.y + blockPosition.y);
	}
}
