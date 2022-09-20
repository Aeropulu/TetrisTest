using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TetrisField : MonoBehaviour
{
	[SerializeField] private int width = 10;
	[SerializeField] private int height = 21;
	private Block[,] _blocks;
	private List<int> _linesToClear = new List<int>(4);
	private List<Vector2Int> _blocksToClear = new List<Vector2Int>(40);
	
	public int xMin { get { return 0; }}
	public int xMax { get { return width - 1; }}
	public int yMin { get { return 0; }}
	public int yMax { get { return height - 1; }}
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

	public async Task CheckAndClearLines(int bottomLine, int topLine)
	{
		if (bottomLine > topLine)
		{
			return;
		}

		_linesToClear.Clear();

		for (int i = bottomLine; i <= topLine; i++)
		{
			if (CheckLine(i))
			{
				_linesToClear.Add(i);
			}
		}

		await ClearLines();
	}

	private bool CheckLine(int line)
	{
		for (int i = 0; i < width; i++)
		{
			Vector2Int position = new Vector2Int(i, line);
			Block block = GetBlock(position);
			if (block == null)
			{
				return false;
			}
		}

		return true;
	}

	private async Task ClearLines()
	{
		int lineCount = _linesToClear.Count;
		if (lineCount <= 0)
		{
			return;
		}

		_blocksToClear.Clear();
		for (int j = 0; j < lineCount; j++)
		{
			for (int i = 0; i < width; i++)
			{
				_blocksToClear.Add(new Vector2Int(i, _linesToClear[j]));
			}
		}

		await ClearBlocks();

		DropLines();
	}

	private async Task ClearBlocks()
	{
		foreach (Vector2Int position in _blocksToClear)
		{
			Destroy(GetBlock(position).gameObject);
			SetBlock(position, null);
		}
		await Task.Delay(250);
	}

	private void DropLines()
	{
		int linesToDrop = 0;
		for (int j = 0; j < height; j++)
		{
			if (_linesToClear.Contains(j))
			{
				linesToDrop++;
				continue;
			}
			if (linesToDrop <= 0)
			{
				continue;
			}
			
			for (int i = 0; i < width; i++)
			{
				Vector2Int destination = new Vector2Int(i, j - linesToDrop);
				Vector2Int source = new Vector2Int(i, j);
				Block block = GetBlock(source);
				if (block != null)
				{
					SetBlock(destination, block);
					block.transform.position = new Vector3(destination.x, destination.y);
					SetBlock(source, null);
				}
			}
		}
	}
}