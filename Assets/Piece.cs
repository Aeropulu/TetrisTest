using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// List wrapper for nested list serialization.
[System.Serializable]
public class BlockSetup
{
	[SerializeField] private List<Vector2Int> _blockList = new List<Vector2Int>();

	public List<Vector2Int> BlockList { get { return _blockList; } }
}

[CreateAssetMenu(menuName ="Tetris/Tetris Piece")]
public class Piece : ScriptableObject
{
	[SerializeField] private Color _color = Color.white;
	[SerializeField] private List<BlockSetup> _rotationFrames = new List<BlockSetup>(1);
	
	private int _currentFrameIndex = 0;

	public Color Color { get { return _color; } }
	public List<Vector2Int> BlockList { get { return _rotationFrames[_currentFrameIndex].BlockList; } }
	public List<Vector2Int> RotatedBlockList { get { return _rotationFrames[GetNextFrameIndex()].BlockList; } }

	public void Rotate()
	{
		_currentFrameIndex = GetNextFrameIndex();
	}

	public void ResetRotation()
	{
		_currentFrameIndex = 0;
	}

	private int GetNextFrameIndex()
	{
		int frameCount = _rotationFrames.Count;
		if (frameCount <= 1)
		{
			return _currentFrameIndex;
		}

		if (_currentFrameIndex == frameCount - 1)
		{
			return 0;
		}

		return _currentFrameIndex + 1;
	}
}
