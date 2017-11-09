using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
	public Texture2D map;
	public Texture2D props;

	public ColorToPrefab[] colorMappings;

	void Start ()
	{
		GeneratePropsLayer();
		GenerateGroundLayer();
	}

	void GenerateGroundLayer()
	{
		for (int x = 0; x < map.width; x++)
		{
			for (int z = 0; z < map.height; z++)
			{
				GenerateGround(x, z);
			}
		}
	}

	void GenerateGround(int x, int z)
	{
		Color pixelColor = map.GetPixel(x, z);

		if (pixelColor.a == 0)
		{
			return;
		}

//		foreach (ColorToPrefab colorMapping in colorMappings)
//		{
//			if (colorMapping.color.Equals(pixelColor))
//			{
//				Vector3 position = new Vector3(x, 0, z);
//				Instantiate(colorMapping.prefab, position, Quaternion.identity, transform);
//			}
//		}

		foreach (ColorToPrefab colorMapping in colorMappings)
		{
//			if (pixelColor == Color.black)
//			{
//				Vector3 position = new Vector3(x, 0, z);
//				Instantiate(colorMapping.prefab, position, Quaternion.identity, transform);
//			}

			if (colorMapping.color.Equals(pixelColor))
			{
				Vector3 position = new Vector3(x, 0.0f, z);
				Instantiate(colorMapping.prefab, position, Quaternion.identity, transform);
			}
		}
	}

	void GeneratePropsLayer()
	{
		for (int x = 0; x < props.width; x++)
		{
			for (int z = 0; z < props.height; z++)
			{
				GenerateProps(x, z);
			}
		}
	}

	void GenerateProps(int x, int z)
	{
		Color pixelColor02 = props.GetPixel(x, z);

		if (pixelColor02.a == 0)
		{
			return;
		}

		foreach (ColorToPrefab colorMapping in colorMappings)
		{
			if (colorMapping.color.Equals(pixelColor02))
			{
				Vector3 position = new Vector3(x, 1, z);
				Instantiate(colorMapping.prefab, position, Quaternion.identity, transform);
			}
		}
	}
}