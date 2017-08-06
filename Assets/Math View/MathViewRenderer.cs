using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(RawImage))]
public class MathViewRenderer : MonoBehaviour
{
	public float cameraZoom { get; set; }
	public Vector2 cameraPosition { get; set; }

	[SerializeField]
	Material renderMaterial;

	[SerializeField]
	Transform panelContainer;

	[SerializeField]
	Color backgroundColor;

	[SerializeField]
	Color gridLineColor;

	[SerializeField]
	Color axisColor;

	[SerializeField]
	Texture2D fontSprite;

	[SerializeField]
	Vector2 fontGlyphSize;

	RenderTexture renderTexture;
	
	void Start()
	{
		cameraZoom = 10f;
		cameraPosition = new Vector2(0f, 0f);

		RawImage rawImage = GetComponent<RawImage>();
		renderTexture = (RenderTexture)rawImage.texture;
	}

	void OnRenderObject()
	{
		Graphics.SetRenderTarget(renderTexture);

		renderMaterial.SetPass(0);

		GL.Clear(false, true, backgroundColor);
		GL.PushMatrix();
		GL.LoadOrtho();
		GL.LoadPixelMatrix(0f, 1f, 0f, 1f);

		RenderGrid();

		LinePanel[] linePanels = panelContainer.GetComponentsInChildren<LinePanel>();
		foreach (LinePanel linePanel in linePanels)
		{
			RenderLine(linePanel.Line);
		}

		ParabolaPanel[] parabolaPanels = panelContainer.GetComponentsInChildren<ParabolaPanel>();
		foreach (ParabolaPanel parabolaPanel in parabolaPanels)
		{
			RenderParabola(parabolaPanel.Parabola, 100);
		}

		GL.PopMatrix();
		Graphics.SetRenderTarget(null);
	}

	Vector2 TransformPoint(Vector2 point)
	{
		return (point + cameraPosition) / cameraZoom + new Vector2(0.5f, 0.5f);
	}

	void RenderPoint(Vector2 point, Color outlineColor, Color fillColor, float radius = -1f)
	{
		if (Mathf.Abs(radius) <= Mathf.Epsilon)
			return;
		
		if (radius < 0f)
			radius = cameraZoom / 20f;

		point = TransformPoint(point);

		GL.Begin(GL.QUADS);

		GL.Color(fillColor);

		GL.Vertex3(point.x - radius, point.y, 0f);
		GL.Vertex3(point.x, point.y - radius, 0f);

		GL.Vertex3(point.x, point.y - radius, 0f);
		GL.Vertex3(point.x + radius, point.y, 0f);

		GL.Vertex3(point.x + radius, point.y, 0f);
		GL.Vertex3(point.x, point.y + radius, 0f);

		GL.Vertex3(point.x, point.y + radius, 0f);
		GL.Vertex3(point.x - radius, point.y, 0f);

		GL.End();

		GL.Begin(GL.LINES);

		GL.Color(outlineColor);

		GL.Vertex3(point.x - radius,	point.y,			0f);
		GL.Vertex3(point.x,				point.y - radius,	0f);

		GL.Vertex3(point.x,				point.y - radius,	0f);
		GL.Vertex3(point.x + radius,	point.y,			0f);

		GL.Vertex3(point.x + radius,	point.y,			0f);
		GL.Vertex3(point.x,				point.y + radius,	0f);

		GL.Vertex3(point.x,				point.y + radius,	0f);
		GL.Vertex3(point.x - radius,	point.y,			0f);

		GL.End();
	}

	void RenderLine(Vector2 from, Vector2 to, Color color)
	{
		from = TransformPoint(from);
		to = TransformPoint(to);

		GL.Begin(GL.LINES);

		GL.Color(color);

		GL.Vertex3(from.x, from.y, 0f);
		GL.Vertex3(to.x, to.y, 0f);

		GL.End();
	}

	void RenderThickLine(Vector2 from, Vector2 to, float width, Color color)
	{
		Vector2 v = to - from;
		v.Normalize();
		v *= width;
		
		float w_x = -v.y / 100f;
		float w_y = v.x / 100f;

		from = TransformPoint(from);
		to = TransformPoint(to);

		GL.Begin(GL.QUADS);

		GL.Color(color);

		GL.Vertex3(from.x + w_x, from.y + w_y, 0f);
		GL.Vertex3(from.x - w_x, from.y - w_y, 0f);
		GL.Vertex3(to.x - w_x, to.y - w_y, 0f);
		GL.Vertex3(to.x + w_x, to.y + w_y, 0f);

		GL.End();
	}

	public void RenderText(string text, Vector2 position, Color color)
	{
		float numGlyphs = fontSprite.width / fontGlyphSize.x;
		float scale = 64f;

		for (int i = 0; i < text.Length; ++i)
		{
			int index = 0;

			if (text[i] > '0' && text[i] <= '9')
				index = text[i] - '0';
			else if (text[i] == '+')
				index = 10;
			else if (text[i] == '-')
				index = 11;
			else if (text[i] == ',')
				index = 12;
			else if (text[i] == '.')
				index = 13;
			else if (text[i] == ':')
				index = 14;
			else if (text[i] == '%')
				index = 15;

			Rect sourceRect = new Rect(index / numGlyphs, 1f, fontGlyphSize.x / fontSprite.width, -1f);
			Rect destinationRect = new Rect(position.x + i * (fontGlyphSize.x / scale), position.y, fontGlyphSize.x / scale, fontGlyphSize.y / scale);
			Graphics.DrawTexture(destinationRect, fontSprite, sourceRect, 0, 0, 0, 0, color);
		}
	}

	void RenderGrid()
	{
		// render grid lines
		for (float x = -1f; x >= -100f; x--)
			RenderLine(new Vector2(x, -100f), new Vector2(x, 100f), gridLineColor);
		for (float x = 1f; x <= 100f; x++)
			RenderLine(new Vector2(x, -100f), new Vector2(x, 100f), gridLineColor);
		for (float y = -1f; y >= -100f; y--)
			RenderLine(new Vector2(-100f, y), new Vector2(100f, y), gridLineColor);
		for (float y = 1f; y <= 100f; y++)
			RenderLine(new Vector2(-100f, y), new Vector2(100f, y), gridLineColor);

		// render axises
		RenderThickLine(new Vector2(-100f, 0f), new Vector2(100f, 0f), 0.35f, axisColor); // x axis
		RenderThickLine(new Vector2(0f, -100f), new Vector2(0f, 100f), 0.35f, axisColor); // y axis
		//RenderLine(new Vector2(-100f, 0f), new Vector2(100f, 0f), axisColor); // x axis
		//RenderLine(new Vector2(0f, -100f), new Vector2(0f, 100f), axisColor); // y axis

		//RenderText("1", new Vector2(1.0f - 0.01f, -1.0f), Color.white);
	}

	void RenderLine(Line line)
	{
		float fromX = -cameraZoom / 2f - cameraPosition.x;
		float toX = cameraZoom / 2f - cameraPosition.x;

		if (!line.Infinite)
		{
			fromX = line.PointAHandle.x;
			toX = line.PointBHandle.x;
		}

		RenderThickLine(new Vector2(fromX, line.Solve(fromX)), new Vector2(toX, line.Solve(toX)), 0.5f, line.Color);

		RenderPoint(line.PointAHandle, Color.black, Color.white, 0.02f);
		RenderPoint(line.PointAHandle, Color.black, line.Color, 0.01f);

		RenderPoint(line.PointBHandle, Color.black, Color.white, 0.02f);
		RenderPoint(line.PointBHandle, Color.black, line.Color, 0.01f);
	}

	void RenderParabola(Parabola parabola, int resolution)
	{
		if (resolution <= 0)
		{
			return;
		}

		float x1 = Mathf.Min(parabola.OriginHandle.x, parabola.CurveHandle.x), x2 = Mathf.Max(parabola.OriginHandle.x, parabola.CurveHandle.x);

		if (Mathf.Abs(x1 - x2) <= 0.01f)
		{
			x1 -= 0.01f;
			x2 += 0.01f;
		}

		if (parabola.Infinite)
		{
			if (parabola.OriginHandle.y < parabola.CurveHandle.y)
			// if the origin handle is below the curve handle the parabole opens up towards the top
			{
				List<float> intersects = parabola.Intersect(cameraZoom / 2f - cameraPosition.y);

				if (intersects == null)
				{
					return;
				}

				if (intersects.Count != 2)
				{
					return;
				}

				x1 = intersects[0];
				x2 = intersects[1];
			}
			else if (parabola.OriginHandle.y > parabola.CurveHandle.y)
			{
				List<float> intersects = parabola.Intersect(-cameraZoom / 2f - cameraPosition.y);

				if (intersects == null)
				{
					return;
				}

				if (intersects.Count != 2)
				{
					return;
				}

				x1 = intersects[1];
				x2 = intersects[0];
			}
		}

		float spanWidth = x2 - x1;
		float stepWidth = spanWidth / resolution;

		for (float x = x1; x < x1 + spanWidth; x += stepWidth)
		{
			Vector2 from, to;

			from = new Vector2(x, parabola.Solve(x));
			to = new Vector2(x + stepWidth, parabola.Solve(x + stepWidth));

			RenderThickLine(from, to, 0.5f, parabola.Color);
		}

		RenderPoint(parabola.OriginHandle, Color.black, Color.white, 0.02f);
		RenderPoint(parabola.OriginHandle, Color.black, parabola.Color, 0.01f);

		RenderPoint(parabola.CurveHandle, Color.black, Color.white, 0.02f);
		RenderPoint(parabola.CurveHandle, Color.black, parabola.Color, 0.01f);

		//RenderPoint(parabola.CurveHandleCopy, Color.black, Color.white, 0.02f);
		//RenderPoint(parabola.CurveHandleCopy, Color.black, parabola.Color, 0.01f);
	}
}
