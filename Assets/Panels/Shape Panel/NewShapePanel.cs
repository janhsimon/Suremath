using UnityEngine;

public class NewShapePanel : MonoBehaviour
{
	ShapePanelContainer shapePanelContainer;

	void Start()
	{
		shapePanelContainer = transform.parent.GetComponent<ShapePanelContainer>();
		
		// TODO: dirty hack but seems necessary for resolution-independent layout
		RectTransform rectTransform = GetComponent<RectTransform>();
		rectTransform.localScale = new Vector2(1f, 1f);
	}

	public void NewLineButtonClicked()
	{
		shapePanelContainer.AddLinePanel(false, "Neue Gerade", Color.black, new Vector2(0f, 0f), new Vector2(1f, 1f), true);
	}

	public void NewParabolaButtonClicked()
	{
		shapePanelContainer.AddParabolaPanel(false, "Neue Parabel", Color.black, new Vector2(0f, 0f), new Vector2(1f, 1f), true);
	}
}
