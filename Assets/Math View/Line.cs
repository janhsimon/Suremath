using UnityEngine;

public class Line
{
	Vector2 mb;
	public Vector2 MB
	{
		get
		{
			return mb;
		}

		set
		{
			// limit the parameters
			value.x = Mathf.Clamp(value.x, -100f, 100f);
			value.y = Mathf.Clamp(value.y, -100f, 100f);

			mb = value;

			RecalculateHandlesFromMB();
		}
	}

	Vector2 pointAHandle;
	public Vector2 PointAHandle
	{
		get
		{
			return pointAHandle;
		}

		set
		{
			pointAHandle = value;
			RecalculateMBFromHandles();
		}
	}

	Vector2 pointBHandle;
	public Vector2 PointBHandle
	{
		get
		{
			return pointBHandle;
		}

		set
		{
			pointBHandle = value;
			RecalculateMBFromHandles();
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
				RasterizeHandles();
			}
		}
	}

	public bool Infinite { get; set; }

	public Line(bool locked, string name, Color color, Vector2 pointAHandle, Vector2 pointBHandle, bool infinite)
	{
		Locked = locked;
		Name = name;
		Color = color;
		this.pointAHandle = pointAHandle;
		this.pointBHandle = pointBHandle;
		RecalculateMBFromHandles();
		Infinite = infinite;
		SnapToGrid = true;
	}

	void RecalculateMBFromHandles()
	{
		if (Mathf.Abs(pointAHandle.x - pointBHandle.x) <= Mathf.Epsilon)
		{
			return;
		}

		float m = 0f;
		float divisor = pointBHandle.x - pointAHandle.x;

		if (divisor < -Mathf.Epsilon || divisor > Mathf.Epsilon)
		{
			m = (pointBHandle.y - pointAHandle.y) / divisor;
		}

		float b = PointAHandle.y - m * PointAHandle.x;

		mb = new Vector2(m, b);
	}

	void RecalculateHandlesFromMB()
	{
		pointAHandle = new Vector2(pointAHandle.x, Solve(pointAHandle.x));
		pointBHandle = new Vector2(pointBHandle.x, Solve(pointBHandle.x));
	}

	public void RasterizeHandles()
	{
		Vector2 rasterizedPointAHandle = pointAHandle;
		rasterizedPointAHandle.x = Mathf.Round(rasterizedPointAHandle.x);
		rasterizedPointAHandle.y = Mathf.Round(rasterizedPointAHandle.y);
		PointAHandle = rasterizedPointAHandle;

		Vector2 rasterizedPointBHandle = pointBHandle;
		rasterizedPointBHandle.x = Mathf.Round(rasterizedPointBHandle.x);
		rasterizedPointBHandle.y = Mathf.Round(rasterizedPointBHandle.y);
		PointBHandle = rasterizedPointBHandle;
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

	public float Solve(float x)
	{
		float m = mb.x;
		float b = mb.y;

		return m * x + b;
	}
}
