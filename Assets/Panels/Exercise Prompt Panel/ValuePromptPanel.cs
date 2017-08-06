using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutElement))]
public class ValuePromptPanel : MonoBehaviour
{
	[SerializeField]
	Text text;

	[SerializeField]
	InputField valueInputField;

	[SerializeField]
	RawImage validationOverlay;

	LayoutElement layoutElement;
	float solution;

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

	public void SetInitialValues(string text, float solution)
	{
		this.text.text = text;
		this.solution = solution;

		validationOverlay.color = new Color(1f, 1f, 1f, 0f);
	}

	public void Validate()
	{
		if (Mathf.Abs(float.Parse(valueInputField.text) - solution) <= Mathf.Epsilon)
		{
			validationOverlay.color = new Color(0f, 1f, 0f, 0.1f);
		}
		else
		{
			validationOverlay.color = new Color(1f, 0f, 0f, 0.1f);
		}
	}
}
