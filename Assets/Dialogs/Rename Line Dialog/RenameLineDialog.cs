using UnityEngine;
using UnityEngine.UI;

public class RenameLineDialog : MonoBehaviour
{
	[SerializeField]
	InputField inputField;

	Line line;

	public void SetInitialValues(Line line, string oldName = "")
	{
		this.line = line;
		inputField.text = oldName;

		// TODO: dirty hack but seems necessary for resolution-independent layout
		RectTransform rectTransform = GetComponent<RectTransform>();
		rectTransform.localScale = new Vector2(1f, 1f);
	}

	public void CancelButtonClicked()
	{
		Destroy(gameObject);
	}

	public void AcceptButtonClicked()
	{
		if (inputField.text.Length > 0)
		{
			line.Name = inputField.text;
		}

		CancelButtonClicked();
	}
}
