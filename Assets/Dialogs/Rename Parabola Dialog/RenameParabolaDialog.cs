using UnityEngine;
using UnityEngine.UI;

public class RenameParabolaDialog : MonoBehaviour
{
	[SerializeField]
	InputField inputField;

	Parabola parabola;

	public void SetInitialValues(Parabola parabola, string oldName = "")
	{
		this.parabola = parabola;
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
			parabola.Name = inputField.text;
		}

		CancelButtonClicked();
	}
}
