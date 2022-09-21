using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Block : MonoBehaviour
{
	private SpriteRenderer _spriteRenderer;
	private void Awake()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void SetColor(Color color)
	{
		_spriteRenderer.color = color;
	}
}
