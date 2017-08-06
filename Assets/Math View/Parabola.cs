using UnityEngine;
using System.Collections.Generic;

public class Parabola
{
	Vector3 abc;
	public Vector3 ABC
	{
		get
		{
			return abc;
		}

		set
		{
			// limit the parameters
			value.x = Mathf.Clamp(value.x, -100f, 100f);
			value.y = Mathf.Clamp(value.y, -100f, 100f);
			value.z = Mathf.Clamp(value.z, -100f, 100f);

			if (Mathf.Abs(value.x) <= 0.01f)
			// no zero or near zero parameter a
			{
				value.x = 0.01f;
			}

			abc = value;

			RecalculateHandlesFromABC();
		}
	}

	Vector2 originHandle;
	public Vector2 OriginHandle
	{
		get
		{
			return originHandle;
		}

		set
		{
			originHandle = value;
			RecalculateABCFromHandles();
		}
	}

	Vector2 curveHandle;
	public Vector2 CurveHandle
	{
		get
		{
			return curveHandle;
		}

		set
		{
			curveHandle = value;
			RecalculateABCFromHandles();
		}
	}

	public Color Color { get; private set; }

	public bool Locked { get; set; }

	public string Name { get; set; }

	bool snapToGrid;
	public bool SnapToGrid
	{
		get
		{
			return snapToGrid;
		}

		set
		{
			snapToGrid = value;

			if (snapToGrid)
			{
				RasterizeOriginAndHandles();
			}
		}
	}

	public bool Infinite { get; set; }

	public Parabola(bool locked, string name, Color color, Vector2 originHandle, Vector2 curveHandle, bool infinite)
	{
		Locked = locked;
		Name = name;
		Color = color;
		this.originHandle = originHandle;
		this.curveHandle = curveHandle;
		RecalculateABCFromHandles();
		Infinite = infinite;
		SnapToGrid = true;
	}

	Vector2 GetOriginFromABC()
	{
		float a = abc.x;
		float b = abc.y;

		if (Mathf.Abs(a) <= 0.01f)
		{
			return new Vector2(0f, 0f);
		}

		float x = -b / (2f * a);
		return new Vector2(x, Solve(x));
	}

	void RecalculateABCFromHandles()
	{
		if (Mathf.Abs(originHandle.x - curveHandle.x) <= 0.01f)
		{
			return;
		}

		Vector2 curveHandleCopy = originHandle;
		curveHandleCopy.x += originHandle.x - curveHandle.x;
		curveHandleCopy.y = curveHandle.y;

		Vector4 v = new Vector4(originHandle.y, curveHandle.y, curveHandleCopy.y, 1f);
		Matrix4x4 mat = new Matrix4x4();
		mat.SetRow(0, new Vector4(originHandle.x * originHandle.x, originHandle.x, 1f, 0f));
		mat.SetRow(1, new Vector4(curveHandle.x * curveHandle.x, curveHandle.x, 1f, 0f));
		mat.SetRow(2, new Vector4(curveHandleCopy.x * curveHandleCopy.x, curveHandleCopy.x, 1f, 0f));
		mat.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
		abc = mat.inverse * v;
	}

	void RecalculateHandlesFromABC()
	{
		float originalDistance = curveHandle.x - originHandle.x;
		originHandle = GetOriginFromABC();
		curveHandle = new Vector2(originHandle.x + originalDistance, Solve(originHandle.x + originalDistance));
	}

	public void RandomizeColor()
	{
		Color color = new Color();

		int cycle = Random.Range(0, 5);
		
		if (cycle == 0)
		{
			color.r = Random.Range(0f, 1f);
			color.g = 0f;
			color.b = 1f;
		}
		else if (cycle == 1)
		{
			color.r = Random.Range(0f, 1f);
			color.g = 1f;
			color.b = 0f;
		}
		else if (cycle == 2)
		{
			color.r = 0f;
			color.g = Random.Range(0f, 1f);
			color.b = 1f;
		}
		else if (cycle == 3)
		{
			color.r = 1f;
			color.g = Random.Range(0f, 1f);
			color.b = 0f;
		}
		else if (cycle == 4)
		{
			color.r = 0f;
			color.g = 1f;
			color.b = Random.Range(0f, 1f);
		}
		else
		{
			color.r = 1f;
			color.g = 0f;
			color.b = Random.Range(0f, 1f);
		}

		color.a = 1f;
		Color = color;
	}

	public void RasterizeOriginAndHandles()
	{
		Vector2 rasterizedOriginHandle = originHandle;
		rasterizedOriginHandle.x = Mathf.Round(rasterizedOriginHandle.x);
		rasterizedOriginHandle.y = Mathf.Round(rasterizedOriginHandle.y);
		OriginHandle = rasterizedOriginHandle;

		Vector2 rasterizedCurveHandle = curveHandle;
		rasterizedCurveHandle.x = Mathf.Round(rasterizedCurveHandle.x);
		rasterizedCurveHandle.y = Mathf.Round(rasterizedCurveHandle.y);
		CurveHandle = rasterizedCurveHandle;
	}

	public float Solve(float x)
	{
		float a = abc.x;
		float b = abc.y;
		float c = abc.z;

		return a * x * x + b * x + c;
	}

	public List<float> Intersect(float y)
	{
		float a = abc.x;
		float b = abc.y;
		float c = abc.z;

		List<float> output = new List<float>();

		float squareRootPart = b * b - 4.0f * a * (c - y);

		if (squareRootPart <= 0.01f)
		{
			return null;
		}

		squareRootPart = Mathf.Sqrt(squareRootPart);

		if (Mathf.Abs(a) <= 0.01f)
		{
			return null;
		}

		output.Add((-b - squareRootPart) / (2f * a));

		if (Mathf.Abs(squareRootPart) <= 0.01f)
		{
			return output;
		}

		output.Add((-b + squareRootPart) / (2f * a));

		return output;
	}
}
