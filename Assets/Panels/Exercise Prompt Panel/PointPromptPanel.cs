using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutElement))]
public class PointPromptPanel : MonoBehaviour
{
	[SerializeField]
	Text text;

	[SerializeField]
	InputField xInputField, yInputField;

	[SerializeField]
	RawImage validationOverlay;

	LayoutElement layoutElement;
	Vector2 solution;

	void Start()
	{
		layoutElement = GetComponent<LayoutElement>();

		// TODO: dirty hack but seems necessary for resolution-independent layout
		RectTransform rectTransform = GetComponent<RectTransform>();
		rectTransform.localScale = new Vector2(1f, 1f);
	}

	void Update()
	{
		// make sure the prompt panel is always large enough for the text
		// and the input field underneath and the 5px border over, in the middle and under it
		layoutElement.minHeight = Mathf.Abs(text.preferredHeight) + 24f + 15f;
	}

	public void SetInitialValues(string text, Vector2 solution)
	{
		this.text.text = text;
		this.solution = solution;

		validationOverlay.color = new Color(1f, 1f, 1f, 0f);
	}

	public void Validate()
	{
		Vector2 input = new Vector2(float.Parse(xInputField.text), float.Parse(yInputField.text));
		if (Vector2.Distance(input, solution) <= Mathf.Epsilon)
		{
			validationOverlay.color = new Color(0f, 1f, 0f, 0.1f);
		}
		else
		{
			validationOverlay.color = new Color(1f, 0f, 0f, 0.1f);
		}
	}
}
