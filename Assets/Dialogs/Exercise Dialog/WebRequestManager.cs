using UnityEngine;
using UnityEngine.Networking;

public class WebRequestManager
{
	public static bool CheckRequest(UnityWebRequest request)
	{
		if (request != null)
		{
			if (request.isError)
			{
				Debug.Log("Error downloading: " + request.error);
			}
			else if (request.isDone)
			{
				if (request.responseCode == 200)
				{
					return true;
				}
				else
				{
					Debug.Log("Server error: " + request.responseCode);
				}
			}
			else
			{
				float progress = request.downloadProgress;
				//Debug.Log("Downloading... Progress: " + progress);
			}
		}

		return false;
	}
}
