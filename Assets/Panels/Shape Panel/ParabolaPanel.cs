using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutElement))]
public class ParabolaPanel : MonoBehaviour
{
	public Parabola Parabola { get; set; }

	[SerializeField]
	float contractedHeight, expandedHeight;

	[SerializeField]
	Transform lockOverlayTransform, nameTextTransform, formulaTextTransform, colorSwatchTransform;

	[SerializeField]
	Transform expandContainerTransform;

	[SerializeField]
	Transform parameterAInputFieldTransform, parameterBInputFieldTransform, parameterCInputFieldTransform;

	[SerializeField]
	GameObject renameParabolaDialogPrefab;

	GameObject dialogContainer;
	LayoutElement layoutElement;
	ShapePanelContainer shapePanelContainer;
	Text nameText, formulaText;
	Image colorSwatchImage;
	InputField parameterAInputField, parameterBInputField, parameterCInputField;
	bool isExpanded;

	enum DetailDisplayMode
	{
		StandardForm,
		VertexForm,
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
		parameterAInputField = parameterAInputFieldTransform.GetComponent<InputField>();
		parameterBInputField = parameterBInputFieldTransform.GetComponent<InputField>();
		parameterCInputField = parameterCInputFieldTransform.GetComponent<InputField>();
		
		dialogContainer = GameObject.Find("Dialog Container");
		
		detailDisplayMode = DetailDisplayMode.StandardForm;

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
		lockOverlayTransform.gameObject.SetActive(Parabola.Locked);
		nameText.text = Parabola.Name;
		formulaText.text = getFormulaString(Parabola.ABC.x, Parabola.ABC.y, Parabola.ABC.z);

		if (!parameterAInputField.isFocused)
		{
			parameterAInputField.text = Parabola.ABC.x.ToString("0.##");
		}

		if (!parameterBInputField.isFocused)
		{
			parameterBInputField.text = Parabola.ABC.y.ToString("0.##");
		}

		if (!parameterCInputField.isFocused)
		{
			parameterCInputField.text = Parabola.ABC.z.ToString("0.##");
		}
	}

	string getFormulaString(float a, float b, float c)
	{
		string output = "";

		if (detailDisplayMode == DetailDisplayMode.StandardForm)
		{
			output += "y = " + (a.ToString("0.##")) + "x²";
			output += ((b < 0f) ? " - " : " + ");
			output += Mathf.Abs(b).ToString("0.##") + "x";
			output += ((c < 0f) ? " - " : " + ");
			output += Mathf.Abs(c).ToString("0.##");
		}
		else if (detailDisplayMode == DetailDisplayMode.VertexForm)
		{
			output += "y = " + (a.ToString("0.##")) + " * (x";
			output += ((Parabola.OriginHandle.x < 0f) ? " + " : " - ");
			output += Mathf.Abs(Parabola.OriginHandle.x).ToString("0.##") + ")²";
			output += ((Parabola.OriginHandle.y < 0f) ? " - " : " + ");
			output += Mathf.Abs(Parabola.OriginHandle.y).ToString("0.##");
		}
		else if (detailDisplayMode == DetailDisplayMode.Parameters)
		{
			output += ("a = " + a.ToString("0.##") + " b = " + b.ToString("0.##") + " c = " + c.ToString("0.##"));
		}
		else if (detailDisplayMode == DetailDisplayMode.Points)
		{
			output += ("S(" + Parabola.OriginHandle.x.ToString("0.##") + ", " + Parabola.OriginHandle.y.ToString("0.##") + ") ");
			output += ("P(" + Parabola.CurveHandle.x.ToString("0.##") + ", " + Parabola.CurveHandle.y.ToString("0.##") + ")");
		}

		return output;
	}

	public void UpdateColorSwatch()
	{
		if (!colorSwatchImage)
		{
			colorSwatchImage = colorSwatchTransform.GetComponent<Image>();
		}

		colorSwatchImage.color = Parabola.Color;
	}

	public void ColorSwatchButtonClicked()
	{
		Parabola.RandomizeColor();
		UpdateColorSwatch();
	}

	public void NameButtonClicked()
	{
		GameObject renameParabolaDialogGameObject = (GameObject)Instantiate(renameParabolaDialogPrefab, dialogContainer.transform);
		RectTransform renameParabolaDialogRectTransform = renameParabolaDialogGameObject.GetComponent<RectTransform>();
		renameParabolaDialogRectTransform.offsetMin = new Vector2(0f, 0f);
		renameParabolaDialogRectTransform.offsetMax = new Vector2(0f, 0f);
		RenameParabolaDialog renameParabolaDialog = renameParabolaDialogGameObject.GetComponent<RenameParabolaDialog>();
		renameParabolaDialog.SetInitialValues(Parabola, Parabola.Name);
	}

	public void FormulaButtonClicked()
	{
		detailDisplayMode++;

		if (detailDisplayMode > DetailDisplayMode.Points)
			detailDisplayMode = DetailDisplayMode.StandardForm;
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

	public void ParameterAInputFieldChanged(string value)
	{
		float a = float.Parse(value);
		Parabola.ABC = new Vector3(a, Parabola.ABC.y, Parabola.ABC.z);
	}

	public void ParameterBInputFieldChanged(string value)
	{
		float b = float.Parse(value);
		Parabola.ABC = new Vector3(Parabola.ABC.x, b, Parabola.ABC.z);
	}

	public void ParameterCInputFieldChanged(string value)
	{
		float c = float.Parse(value);
		Parabola.ABC = new Vector3(Parabola.ABC.x, Parabola.ABC.y, c);
	}

	public void SnapToGridToggleChanged(bool value)
	{
		Parabola.SnapToGrid = value;
	}

	public void InfiniteToggleChanged(bool value)
	{
		Parabola.Infinite = value;
	}
}
