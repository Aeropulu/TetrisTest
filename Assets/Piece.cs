using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// List wrapper for nested list serialization.
[System.Serializable]
public class BlockSetup
{
	[SerializeField] private List<Vector2Int> _blockList = new List<Vector2Int>();

	public int Count { get { return _blockList.Count; } }
	public Vector2Int this[int index] { get { return _blockList[index]; } }
}

[CreateAssetMenu(menuName ="Tetris/Tetris Piece")]
public class Piece : ScriptableObject
{
	[SerializeField] private List<BlockSetup> _blockSetup = new List<BlockSetup>(1);

}
