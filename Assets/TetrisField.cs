using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisField : MonoBehaviour
{
	[SerializeField] private int width = 10;
	[SerializeField] private int height = 21;
	private Block[,] _blocks;
	
	public int xMin { get { return 0; }}
	public int xMax { get { return width - 1; }}
	public int yMin { get { return 0; }}
	public Vector2Int DropPoint { get { return new Vector2Int(width / 2, height); }}

	private void Start()
	{
		_blocks = new Block[width,height];
	}

	public Block GetBlock(Vector2Int position)
	{
		if (_blocks == null || 
			position.x < 0 || position.x >= width ||
			position.y < 0 || position.y >= height)
		{
			return null;
		}

		return _blocks[position.x, position.y];
	}

	public void SetBlock(Vector2Int position, Block block)
	{
		if (_blocks == null)
		{
			return;
		}

		_blocks[position.x, position.y] = block;
	}
}
