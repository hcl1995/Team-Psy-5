﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
	public Texture2D[] texture;
	public ColorToPrefab[] colorMappings;

	void Start ()
	{
		GenerateLevel();

		foreach (ColorToPrefab colorMapping in colorMappings)
		{
			colorMapping.prefab.GetComponent<Renderer>().sharedMaterial.color = colorMapping.color;
		}
	}

	void GenerateLevel()
	{
		for (int x = 0; x < texture[0].width; x++)
		{
			for (int z = 0; z < texture[0].height; z++)
			{
				GenerateLevel(x, z);
			}
		}
	}

	void GenerateLevel(int x, int z)
	{
		for (int i = 0; i < texture.Length; i++)
		{
			float oriScale = 1;
			float oriYPosition = 1;
			Color pixelColor = texture[i].GetPixel(x, z);

			if (pixelColor.a == 0)
			{
				return;
			}

			// IF Designer Arrange In Wrong Layer Order = GG.
			if (i < 1)
			{
				oriYPosition = 0;
			}
			else if (i > 1)
			{
				oriYPosition = 1;
			}

			foreach (ColorToPrefab colorMapping in colorMappings)
			{
				if (colorMapping.color.Equals(pixelColor))
				{
					// Preset Scale | Normally It's Always 1 For What Artist Made.

					// magic number. Means Scale + 1 = Y + 0.5. | Original Scale & Y-Position = 1.
					float scale = colorMapping.prefab.transform.localScale.y - oriScale; 
					float yPosition = oriYPosition + (scale * 0.5f);

					Vector3 position = new Vector3(x, yPosition, z);
					Instantiate(colorMapping.prefab, position, Quaternion.identity, transform);
					// Required Grouping The Shapes Into Game Object & Modify. FUCK THIS SHIT, BLOCKS CMI.
				}
			}
		}
	}
}