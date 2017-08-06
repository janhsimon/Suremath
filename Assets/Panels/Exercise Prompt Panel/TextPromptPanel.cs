using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutElement))]
public class TextPromptPanel : MonoBehaviour
{
	[SerializeField]
	Text promptText;

	LayoutElement layoutElement;

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
		// and the 5px border over and under it
		layoutElement.minHeight = Mathf.Abs(promptText.preferredHeight) + 10f;
	}
}
