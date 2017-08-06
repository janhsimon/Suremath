using UnityEngine;

public class ShapePanelContainer : MonoBehaviour
{
	[SerializeField]
	GameObject newShapePanelPrefab, linePanelPrefab, parabolaPanelPrefab;

	void RemoveNewShapePanel()
	{
		if (transform.childCount > 0)
		{
			Destroy(transform.GetChild(transform.childCount - 1).gameObject);
		}
	}

	void AddNewShapePanel()
	{
		Instantiate(newShapePanelPrefab, transform);
	}

	public void AddLinePanel(bool locked, string name, Color color, Vector2 pointAHandle, Vector2 pointBHandle, bool infinite)
	{
		RemoveNewShapePanel();

		GameObject linePanelGameObject = (GameObject)Instantiate(linePanelPrefab, transform);
		LinePanel linePanel = linePanelGameObject.GetComponent<LinePanel>();
		linePanel.Line = new Line(locked, name, color, pointAHandle, pointBHandle, infinite);

		if (color == Color.black)
		{
			linePanel.Line.RandomizeColor();
		}

		linePanel.UpdateColorSwatch();

		AddNewShapePanel();
	}

	public void AddParabolaPanel(bool locked, string name, Color color, Vector2 originHandle, Vector2 curveHandle, bool infinite)
	{
		RemoveNewShapePanel();

		GameObject parabolaPanelGameObject = (GameObject)Instantiate(parabolaPanelPrefab, transform);
		ParabolaPanel parabolaPanel = parabolaPanelGameObject.GetComponent<ParabolaPanel>();
		parabolaPanel.Parabola = new Parabola(locked, name, color, originHandle, curveHandle, infinite);

		if (color == Color.black)
		{
			parabolaPanel.Parabola.RandomizeColor();
		}

		parabolaPanel.UpdateColorSwatch();

		AddNewShapePanel();
	}

	public void RemovePanel(GameObject panel)
	{
		Destroy(panel);
	}

	public void ClearAllPanels()
	{
		for (int i = transform.childCount - 1; i >= 0; i--)
		{
			Destroy(transform.GetChild(i).gameObject);
		}

		AddNewShapePanel();
	}

	public string PanelsToString()
	{
		string output = "";

		for (int i = 0; i < transform.childCount; i++)
		{
			GameObject childGameObject = transform.GetChild(i).gameObject;

			LinePanel linePanel = childGameObject.GetComponent<LinePanel>();
			if (linePanel != null)
			{
				output += "<line name=\"" + linePanel.Line.Name + "\">";
				output += "<color r=\"" + linePanel.Line.Color.r + "\" g=\"" + linePanel.Line.Color.g + "\" b=\"" + linePanel.Line.Color.b + "\"/>";
				output += "<handle1 x=\"" + linePanel.Line.PointAHandle.x + "\" y=\"" + linePanel.Line.PointAHandle.y + "\"/>";
				output += "<handle2 x=\"" + linePanel.Line.PointBHandle.x + "\" y=\"" + linePanel.Line.PointBHandle.y + "\"/>";
				output += "<properties infinite=\"" + linePanel.Line.Infinite + "\"/>";
				output += "</line>";
				continue;
			}

			ParabolaPanel parabolaPanel = childGameObject.GetComponent<ParabolaPanel>();
			if (parabolaPanel != null)
			{
				output += "<parabola name=\"" + parabolaPanel.Parabola.Name + "\">";
				output += "<color r=\"" + parabolaPanel.Parabola.Color.r + "\" g=\"" + parabolaPanel.Parabola.Color.g + "\" b=\"" + parabolaPanel.Parabola.Color.b + "\"/>";
				output += "<originhandle x=\"" + parabolaPanel.Parabola.OriginHandle.x + "\" y=\"" + parabolaPanel.Parabola.OriginHandle.y + "\"/>";
				output += "<curvehandle x=\"" + parabolaPanel.Parabola.CurveHandle.x + "\" y=\"" + parabolaPanel.Parabola.CurveHandle.y + "\"/>";
				output += "<properties infinite=\"" + parabolaPanel.Parabola.Infinite + "\"/>";
				output += "</parabola>";
			}
		}

		return output;
	}
}
