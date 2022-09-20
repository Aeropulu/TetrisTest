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
	private List<Vector2Int> _blockPositions = null;
	private List<Block> _blocks = null;
	private Vector2Int _currentPiecePosition = Vector2Int.zero;
	private float _nextMoveDown = 0.0f;

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
		
		MakePieceGameObjects();

		_currentPiecePosition = _field.DropPoint;
		_nextMoveDown = Time.time + _moveDownInterval;
	}


	void Update()
	{
		if (Time.time > _nextMoveDown || Input.GetKeyDown(KeyCode.DownArrow))
		{
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
		_nextMoveDown += _moveDownInterval;
		_currentPiecePosition += Vector2Int.down;

		if (CheckIfBlocked() || CheckIfBottom())
		{
			_currentPiecePosition += Vector2Int.up;
			PlacePiece();
		}

		UpdateBlockObjects();
	}

	private bool CheckIfBottom()
	{
		int blockCount = _blocks.Count;
		for (int i = 0; i < blockCount; i++)
		{
			Vector2Int position = _blockPositions[i] + _currentPiecePosition;
			if (position.y < _field.yMin)
			{
				return true;
			}
		}

		return false;
	}

	private void MoveSide(int amount)
	{
		Vector2Int moveVector = new Vector2Int(amount, 0);
		_currentPiecePosition += moveVector;

		if (CheckIfBlocked())
		{
			_currentPiecePosition -= moveVector;
		}

		KeepInsideField();

		UpdateBlockObjects();
	}

	private bool CheckIfBlocked()
	{
		int blockCount = _blocks.Count;
		for (int i = 0; i < blockCount; i++)
		{
			Block block = _blocks[i];
			Vector2Int position = _blockPositions[i] + _currentPiecePosition;
			if (_field.GetBlock(position) != null)
			{
				return true;
			}
		}


		return false;
	}

	private void Rotate()
	{
		int blockCount = _blocks.Count;
		_currentPiece.Rotate();
		_blockPositions = _currentPiece.BlockList;

		KeepInsideField();

		UpdateBlockObjects();
	}

	private void KeepInsideField()
	{
		int offset = 0;
		int blockCount = _blocks.Count;
		for (int i = 0; i < blockCount; i++)
		{
			Vector2Int position = _blockPositions[i] + _currentPiecePosition;

			if (position.x < _field.xMin)
			{
				offset = Mathf.Max(offset, _field.xMin - position.x);
			}

			if (position.x > _field.xMax)
			{
				offset = Mathf.Min(offset, _field.xMax - position.x);
			}
		}

		_currentPiecePosition += new Vector2Int(offset, 0);
	}

	private void UpdateBlockObjects()
	{
		int blockCount = _blocks.Count;
		for (int i = 0; i < blockCount; i++)
		{
			Block block = _blocks[i];
			block.transform.position = GridToWorldPosition(_blockPositions[i]);
		}
	}

	private void PlacePiece()
	{
		int blockCount = _blocks.Count;
		for (int i = 0; i < blockCount; i++)
		{
			Block block = _blocks[i];
			Vector2Int position = _blockPositions[i] + _currentPiecePosition;
			_field.SetBlock(position, block);
		}

		_blocks.Clear();
		_currentPiecePosition = _field.DropPoint;
		MakePieceGameObjects();
	}

	private void MakePieceGameObjects()
	{
		_currentPiece = _pieceDispenser.GetNext();
		_blockPositions = _currentPiece.BlockList;

		if (_blockPrefab == null || _blockPositions == null)
		{
			return;
		}

		_blocks.Clear();

		foreach(Vector2Int blockPosition in _blockPositions)
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
