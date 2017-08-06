using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutElement))]
public class LinePanel : MonoBehaviour
{
	public Line Line { get; set; }

	[SerializeField]
	float contractedHeight, expandedHeight;

	[SerializeField]
	Transform lockOverlayTransform, nameTextTransform, formulaTextTransform, colorSwatchTransform;

	[SerializeField]
	Transform expandContainerTransform;

	[SerializeField]
	Transform parameterMInputFieldTransform, parameterBInputFieldTransform;

	[SerializeField]
	GameObject renameLineDialogPrefab;

	GameObject dialogContainer;
	LayoutElement layoutElement;
	ShapePanelContainer shapePanelContainer;
	Text nameText, formulaText;
	Image colorSwatchImage;
	InputField parameterMInputField, parameterBInputField;
	bool isExpanded;

	enum DetailDisplayMode
	{
		Formula,
		Parameters,
		Points
	}

	DetailDisplayMode detailDisplayMode;

	void Start()
	{
		Transform parent = transform.parent;
		shapePanelContainer = parent.GetComponent<ShapePanelContainer>();
		nameText = nameTextTransform.GetComponent<Text>();
		formulaText = formulaTextTransform.GetComponent<Text>();
		parameterMInputField = parameterMInputFieldTransform.GetComponent<InputField>();
		parameterBInputField = parameterBInputFieldTransform.GetComponent<InputField>();
		
		dialogContainer = GameObject.Find("Dialog Container");
		
		detailDisplayMode = DetailDisplayMode.Formula;

		// TODO: dirty hack but seems necessary for resolution-independent layout
		RectTransform rectTransform = GetComponent<RectTransform>();
		rectTransform.localScale = new Vector2(1f, 1f);

		layoutElement = GetComponent<LayoutElement>();
		layoutElement.minHeight = contractedHeight;
		expandContainerTransform.gameObject.SetActive(false);
		isExpanded = false;
	}

	void Update()
	{
		lockOverlayTransform.gameObject.SetActive(Line.Locked);
		nameText.text = Line.Name;
		formulaText.text = getFormulaString(Line.MB.x, Line.MB.y);

		if (!parameterMInputField.isFocused)
		{
			parameterMInputField.text = Line.MB.x.ToString("0.##");
		}

		if (!parameterBInputField.isFocused)
		{
			parameterBInputField.text = Line.MB.y.ToString("0.##");
		}
	}

	string getFormulaString(float m, float b)
	{
		string output = "";

		if (detailDisplayMode == DetailDisplayMode.Formula)
		{
			output += "y = " + (m.ToString("0.##")) + "x ";
			output += ((b < 0f) ? "- " : "+ ");
			output += Mathf.Abs(b).ToString("0.##");
		}
		else if (detailDisplayMode == DetailDisplayMode.Parameters)
		{
			output = ("m = " + m.ToString("0.##") + " b = " + b.ToString("0.##"));
		}
		else if (detailDisplayMode == DetailDisplayMode.Points)
		{
			output += ("P1(" + Line.PointAHandle.x.ToString("0.##") + ", " + Line.PointAHandle.y.ToString("0.##") + ") ");
			output += ("P2(" + Line.PointBHandle.x.ToString("0.##") + ", " + Line.PointBHandle.y.ToString("0.##") + ")");
		}

		return output;
	}

	public void UpdateColorSwatch()
	{
		if (!colorSwatchImage)
		{
			colorSwatchImage = colorSwatchTransform.GetComponent<Image>();
		}

		colorSwatchImage.color = Line.Color;
	}

	public void ColorSwatchButtonClicked()
	{
		Line.RandomizeColor();
		UpdateColorSwatch();
	}

	public void NameButtonClicked()
	{
		GameObject renameLineDialogGameObject = (GameObject)Instantiate(renameLineDialogPrefab, dialogContainer.transform);
		RectTransform renameLineDialogRectTransform = renameLineDialogGameObject.GetComponent<RectTransform>();
		renameLineDialogRectTransform.offsetMin = new Vector2(0f, 0f);
		renameLineDialogRectTransform.offsetMax = new Vector2(0f, 0f);
		RenameLineDialog renameLineDialog = renameLineDialogGameObject.GetComponent<RenameLineDialog>();
		renameLineDialog.SetInitialValues(Line, Line.Name);
	}

	public void FormulaButtonClicked()
	{
		detailDisplayMode++;

		if (detailDisplayMode > DetailDisplayMode.Points)
		{
			detailDisplayMode = DetailDisplayMode.Formula;
		}
	}

	public void ExpandButtonClicked()
	{
		isExpanded = !isExpanded;

		if (isExpanded)
		{
			layoutElement.minHeight = expandedHeight;
		}
		else
		{
			layoutElement.minHeight = contractedHeight;
		}

		expandContainerTransform.gameObject.SetActive(isExpanded);
	}

	public void DeleteButtonClicked()
	{
		shapePanelContainer.RemovePanel(gameObject);
	}

	public void ParameterMInputFieldChanged(string value)
	{
		float m = float.Parse(value);
		Line.MB = new Vector2(m, Line.MB.y);
	}

	public void ParameterBInputFieldChanged(string value)
	{
		float b = float.Parse(value);
		Line.MB = new Vector2(Line.MB.x, b);
	}

	public void SnapToGridToggleChanged(bool value)
	{
		Line.SnapToGrid = value;
	}

	public void InfiniteToggleChanged(bool value)
	{
		Line.Infinite = value;
	}
}
