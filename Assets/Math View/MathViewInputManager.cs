using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(MathViewRenderer))]
[RequireComponent(typeof(RectTransform))]
public class MathViewInputManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IScrollHandler
{
	[SerializeField]
	Transform panelContainer;

	[SerializeField]
	float cameraDragSpeed;

	MathViewRenderer mathViewRenderer;

	Line draggedLine;
	int draggedLinePiece;

	Parabola draggedParabola;
	int draggedParabolaPiece;

	float doubleClickCooldown;

	RectTransform rectTransform;
	
	void Start()
	{
		mathViewRenderer = GetComponent<MathViewRenderer>();
		rectTransform = GetComponent<RectTransform>();
		
		draggedLine = null;
		draggedParabola = null;

		doubleClickCooldown = 0f;
	}

	void Update()
	{
		if (doubleClickCooldown > 0f)
		{
			doubleClickCooldown -= Time.deltaTime;
		}
	}

	public void OnPointerDown(PointerEventData data)
	{
		/*
		Vector2 mousePosition = data.position - new Vector2(780f, 310f);
		mousePosition /= 260f;
		*/

		Vector3[] corners = new Vector3[4];
		rectTransform.GetWorldCorners(corners);
		Vector2 mousePosition = data.position - new Vector2(corners[0].x, corners[0].y);
		Vector2 size = corners[2] - corners[0];
		mousePosition.x /= size.x;
		mousePosition.y /= size.y;

		mousePosition.x = mousePosition.x * 2f - 1f;
		mousePosition.y = mousePosition.y * 2f - 1f;
		//Debug.Log(mousePosition);

		LinePanel[] linePanels = panelContainer.GetComponentsInChildren<LinePanel>();
		foreach (LinePanel linePanel in linePanels)
		{
			Line line = linePanel.Line;

			if (line.Locked)
				continue;

			Vector2 v = (line.PointAHandle /*- cameraPosition*/ / (mathViewRenderer.cameraZoom / 2f)) - mousePosition;
			if (v.magnitude < 0.1f)
			{
				line.PointAHandle += data.delta * mathViewRenderer.cameraZoom * (cameraDragSpeed / 1000f);
				draggedLine = line;
				draggedParabola = null;

				if (doubleClickCooldown > 0f)
				{
					draggedLinePiece = 2;
				}
				else
				{
					draggedLinePiece = 0;
				}

				return;
			}

			v = (line.PointBHandle /*- cameraPosition*/ / (mathViewRenderer.cameraZoom / 2f)) - mousePosition;
			if (v.magnitude < 0.1f)
			{
				line.PointBHandle += data.delta * mathViewRenderer.cameraZoom * (cameraDragSpeed / 1000f);
				draggedLine = line;
				draggedParabola = null;

				if (doubleClickCooldown > 0f)
				{
					draggedLinePiece = 2;
				}
				else
				{
					draggedLinePiece = 1;
				}

				return;
			}
		}

		ParabolaPanel[] parabolaPanels = panelContainer.GetComponentsInChildren<ParabolaPanel>();
		foreach (ParabolaPanel parabolaPanel in parabolaPanels)
		{
			Parabola parabola = parabolaPanel.Parabola;

			if (parabola.Locked)
				continue;

			Vector2 v = (parabola.OriginHandle /*- cameraPosition*/ / (mathViewRenderer.cameraZoom / 2f)) - mousePosition;
			//Debug.Log("Handle = " + (parabola.CurveHandle /*- cameraPosition*/ / (cameraZoom / 2f)) + "   Mouse Click = " + mouseClick);
			if (v.magnitude < 0.1f)
			{
				parabola.OriginHandle += data.delta * mathViewRenderer.cameraZoom * (cameraDragSpeed / 1000f);
				draggedParabola = parabola;
				draggedLine = null;

				if (doubleClickCooldown > 0f)
				{
					draggedParabolaPiece = 2;
				}
				else
				{
					draggedParabolaPiece = 0;
				}

				return;
			}

			v = (parabola.CurveHandle /*- cameraPosition*/ / (mathViewRenderer.cameraZoom / 2f)) - mousePosition;
			if (v.magnitude < 0.1f)
			{
				parabola.CurveHandle += data.delta * mathViewRenderer.cameraZoom * (cameraDragSpeed / 1000f);
				draggedParabola = parabola;
				draggedLine = null;

				if (doubleClickCooldown > 0f)
				{
					draggedParabolaPiece = 2;
				}
				else
				{
					draggedParabolaPiece = 1;
				}

				return;
			}

			/*
			v = (parabola.CurveHandleCopy / (cameraZoom / 2f)) - mousePosition;
			if (v.magnitude < 0.1f)
			{
				parabola.CurveHandleCopy += data.delta * cameraZoom * (cameraDragSpeed / 1000f);
				draggedParabola = parabola;
				draggedParabolaPiece = 2;
				return;
			}
			*/
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (draggedLine != null)
		{
			if (draggedLine.SnapToGrid)
			{
				draggedLine.RasterizeHandles();
			}

			draggedLine = null;
			doubleClickCooldown = 0.5f;
		}
		else if (draggedParabola != null)
		{
			if (draggedParabola.SnapToGrid)
			{
				draggedParabola.RasterizeOriginAndHandles();
			}

			draggedParabola = null;
			doubleClickCooldown = 0.5f;
		}
	}

	public void OnDrag(PointerEventData data)
	{
		if (draggedLine != null)
		{
			if (draggedLinePiece == 0 || draggedLinePiece == 2)
				draggedLine.PointAHandle += data.delta * mathViewRenderer.cameraZoom * (cameraDragSpeed / 1000f);
			
			if (draggedLinePiece == 1 || draggedLinePiece == 2)
				draggedLine.PointBHandle += data.delta * mathViewRenderer.cameraZoom * (cameraDragSpeed / 1000f);
		}
		else if (draggedParabola != null)
		{
			if (draggedParabolaPiece == 0 || draggedParabolaPiece == 2)
				draggedParabola.OriginHandle += data.delta * mathViewRenderer.cameraZoom * (cameraDragSpeed / 1000f);
			
			if (draggedParabolaPiece == 1 || draggedParabolaPiece == 2)
				draggedParabola.CurveHandle += data.delta * mathViewRenderer.cameraZoom * (cameraDragSpeed / 1000f);
		}
		else
		{
			//cameraPosition += data.delta * cameraZoom * (cameraDragSpeed / 1000f);
		}
	}

	public void OnScroll(PointerEventData data)
	{
		mathViewRenderer.cameraZoom -= data.scrollDelta.y;

		if (mathViewRenderer.cameraZoom < 1f)
			mathViewRenderer.cameraZoom = 1f;
		else if (mathViewRenderer.cameraZoom > 100f)
			mathViewRenderer.cameraZoom = 100f;

		//Debug.Log("Camera Zoom = " + cameraZoom);
	}
}
