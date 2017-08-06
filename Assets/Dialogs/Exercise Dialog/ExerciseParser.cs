using System.Collections;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class ExerciseParser : MonoBehaviour
{
	static public ExerciseParser instance;

	void Awake()
	{
		instance = this;
	}

	public static void ParseList(string text, GameObject exerciseBrowserListItemPrefab, GameObject viewport, ExerciseDialog exerciseDialog)
	{
		XmlDocument doc = new XmlDocument();
		doc.LoadXml(text);

		foreach (XmlNode node in doc.DocumentElement.ChildNodes)
		{
			if (node.Name.CompareTo("exercise") != 0)
				continue;

			string id = "?", title = "?", author = "?", faculty = "?";

			foreach (XmlAttribute attribute in node.Attributes)
			{
				if (attribute.Name.CompareTo("id") == 0)
				{
					id = attribute.Value;
				}
				else if (attribute.Name.CompareTo("title") == 0)
				{
					title = attribute.Value;
				}
				else if (attribute.Name.CompareTo("author") == 0)
				{
					author = attribute.Value;
				}
				else if (attribute.Name.CompareTo("faculty") == 0)
				{
					faculty = attribute.Value;
				}
			}

			//Debug.Log("Parsed exercise \"" + title + "\" with id: " + id);

			GameObject exerciseBrowserListItem = (GameObject)Instantiate(exerciseBrowserListItemPrefab, viewport.transform);
			ExerciseBrowserPanel exerciseBrowserListItemController = exerciseBrowserListItem.GetComponent<ExerciseBrowserPanel>();
			exerciseBrowserListItemController.SetExerciseDialog(exerciseDialog);
			exerciseBrowserListItemController.SetInitialValues(id, title, author, faculty);
		}
	}

	static void ParsePrompt(XmlNode node, ExercisePanelContainer exercisePanelContainer)
	{
		XmlNode typeAttribute = node.Attributes.GetNamedItem("type");

		if (typeAttribute != null)
		{
			if (typeAttribute.Value.CompareTo("text") == 0)
			{
				ParseTextPrompt(node, exercisePanelContainer);
			}
			else if (typeAttribute.Value.CompareTo("value") == 0)
			{
				ParseValuePrompt(node, exercisePanelContainer);
			}
			else if (typeAttribute.Value.CompareTo("point") == 0)
			{
				ParsePointPrompt(node, exercisePanelContainer);
			}
		}
	}

	static void ParseTextPrompt(XmlNode node, ExercisePanelContainer exercisePanelContainer)
	{
		exercisePanelContainer.AddTextPromptPanel(node.InnerText);
	}

	static void ParseValuePrompt(XmlNode node, ExercisePanelContainer exercisePanelContainer)
	{
		float solution = 0f;

		XmlNode solutionAttribute = node.Attributes.GetNamedItem("solution");

		if (solutionAttribute != null)
		{
			solution = float.Parse(solutionAttribute.Value);
		}

		exercisePanelContainer.AddValuePromptPanel(node.InnerText, solution);
	}

	static void ParsePointPrompt(XmlNode node, ExercisePanelContainer exercisePanelContainer)
	{
		Vector2 solution = Vector2.zero;

		XmlNode solutionXAttribute = node.Attributes.GetNamedItem("solution_x");
		XmlNode solutionYAttribute = node.Attributes.GetNamedItem("solution_y");

		if (solutionXAttribute != null)
		{
			solution.x = float.Parse(solutionXAttribute.Value);
		}

		if (solutionYAttribute != null)
		{
			solution.y = float.Parse(solutionYAttribute.Value);
		}

		exercisePanelContainer.AddPointPromptPanel(node.InnerText, solution);
	}

	static void ParseLine(XmlNode node, ShapePanelContainer shapePanelContainer)
	{
		string name = "Unbenannte Gerade";
		float colorR = 0f, colorG = 0f, colorB = 0f, handle1X = 0f, handle1Y = 0f, handle2X = 1f, handle2Y = 1f;
		bool infinite = true;

		foreach (XmlAttribute attribute in node.Attributes)
		{
			if (attribute.Name.CompareTo("name") == 0)
			{
				name = attribute.Value;
			}
		}

		foreach (XmlNode childNode in node.ChildNodes)
		{
			if (childNode.Name.CompareTo("color") == 0)
			{
				foreach (XmlAttribute attribute in childNode.Attributes)
				{
					if (attribute.Name.CompareTo("r") == 0)
					{
						colorR = float.Parse(attribute.Value);
					}
					else if (attribute.Name.CompareTo("g") == 0)
					{
						colorG = float.Parse(attribute.Value);
					}
					else if (attribute.Name.CompareTo("b") == 0)
					{
						colorB = float.Parse(attribute.Value);
					}
				}
			}
			else if (childNode.Name.CompareTo("handle1") == 0)
			{
				foreach (XmlAttribute attribute in childNode.Attributes)
				{
					if (attribute.Name.CompareTo("x") == 0)
					{
						handle1X = float.Parse(attribute.Value);
					}
					else if (attribute.Name.CompareTo("y") == 0)
					{
						handle1Y = float.Parse(attribute.Value);
					}
				}
			}
			else if (childNode.Name.CompareTo("handle2") == 0)
			{
				foreach (XmlAttribute attribute in childNode.Attributes)
				{
					if (attribute.Name.CompareTo("x") == 0)
					{
						handle2X = float.Parse(attribute.Value);
					}
					else if (attribute.Name.CompareTo("y") == 0)
					{
						handle2Y = float.Parse(attribute.Value);
					}
				}
			}
			else if (childNode.Name.CompareTo("properties") == 0)
			{
				foreach (XmlAttribute attribute in childNode.Attributes)
				{
					if (attribute.Name.CompareTo("infinite") == 0)
					{
						infinite = bool.Parse(attribute.Value);
					}
				}
			}
		}

		shapePanelContainer.AddLinePanel(true, name, new Color(colorR, colorG, colorB), new Vector2(handle1X, handle1Y), new Vector2(handle2X, handle2Y), infinite);
	}

	static void ParseParabola(XmlNode node, ShapePanelContainer shapePanelContainer)
	{
		string name = "Unbenannte Parabel";
		float colorR = 0f, colorG = 0f, colorB = 0f, originHandleX = 0f, originHandleY = 0f, curveHandleX = 1f, curveHandleY = 1f;
		bool infinite = true;

		foreach (XmlAttribute attribute in node.Attributes)
		{
			if (attribute.Name.CompareTo("name") == 0)
			{
				name = attribute.Value;
			}
		}

		foreach (XmlNode childNode in node.ChildNodes)
		{
			if (childNode.Name.CompareTo("color") == 0)
			{
				foreach (XmlAttribute attribute in childNode.Attributes)
				{
					if (attribute.Name.CompareTo("r") == 0)
					{
						colorR = float.Parse(attribute.Value);
					}
					else if (attribute.Name.CompareTo("g") == 0)
					{
						colorG = float.Parse(attribute.Value);
					}
					else if (attribute.Name.CompareTo("b") == 0)
					{
						colorB = float.Parse(attribute.Value);
					}
				}
			}
			else if (childNode.Name.CompareTo("originhandle") == 0)
			{
				foreach (XmlAttribute attribute in childNode.Attributes)
				{
					if (attribute.Name.CompareTo("x") == 0)
					{
						originHandleX = float.Parse(attribute.Value);
					}
					else if (attribute.Name.CompareTo("y") == 0)
					{
						originHandleY = float.Parse(attribute.Value);
					}
				}
			}
			else if (childNode.Name.CompareTo("curvehandle") == 0)
			{
				foreach (XmlAttribute attribute in childNode.Attributes)
				{
					if (attribute.Name.CompareTo("x") == 0)
					{
						curveHandleX = float.Parse(attribute.Value);
					}
					else if (attribute.Name.CompareTo("y") == 0)
					{
						curveHandleY = float.Parse(attribute.Value);
					}
				}
			}
			else if (childNode.Name.CompareTo("properties") == 0)
			{
				foreach (XmlAttribute attribute in childNode.Attributes)
				{
					if (attribute.Name.CompareTo("infinite") == 0)
					{
						infinite = bool.Parse(attribute.Value);
					}
				}
			}
		}

		shapePanelContainer.AddParabolaPanel(true, name, new Color(colorR, colorG, colorB), new Vector2(originHandleX, originHandleY), new Vector2(curveHandleX, curveHandleY), infinite);
	}

	IEnumerator ParseSequence(XmlNode node)
	{
		foreach (XmlNode childNode in node.ChildNodes)
		{
			if (childNode.Name.CompareTo("debug") == 0)
			{
				string message = "";

				foreach (XmlAttribute attribute in childNode.Attributes)
				{
					if (attribute.Name.CompareTo("message") == 0)
					{
						message = attribute.Value;
					}
				}

				// TODO: this prints the message, waits one second and then RETURNS, cancelling everything else, I need to find a way around that.
				Debug.Log(message);
				yield return new WaitForSeconds(1f);
				Debug.Log("Aha");
			}
			/*
			else if (childNode.Name.CompareTo("wait") == 0)
			{
				float seconds = 0f;

				foreach (XmlAttribute attribute in childNode.Attributes)
				{
					if (attribute.Name.CompareTo("seconds") == 0)
					{
						seconds = float.Parse(attribute.Value);
					}
				}

				Debug.Log("Now waiting for " + seconds + " seconds...");
				yield return new WaitForSeconds(seconds);
			}
			*/
		}
	}

	public static void ParseExercise(string text, Text exerciseTitleText, ExercisePanelContainer exercisePanelContainer, ShapePanelContainer shapePanelContainer)
	{
		shapePanelContainer.ClearAllPanels();
		exercisePanelContainer.ClearPanels(false);

		XmlDocument doc = new XmlDocument();
		doc.LoadXml(text);

		foreach (XmlNode node in doc.DocumentElement.ChildNodes)
		{
			if (node.Name.CompareTo("title") == 0)
			{
				exerciseTitleText.text = node.InnerText;
			}
			else if (node.Name.CompareTo("prompt") == 0)
			{
				ParsePrompt(node, exercisePanelContainer);
			}
			else if (node.Name.CompareTo("line") == 0)
			{
				ParseLine(node, shapePanelContainer);
			}
			else if (node.Name.CompareTo("parabola") == 0)
			{
				ParseParabola(node, shapePanelContainer);
			}
			else if (node.Name.CompareTo("sequence") == 0)
			{
				instance.StartCoroutine("ParseSequence", node);
			}
		}
	}
}
