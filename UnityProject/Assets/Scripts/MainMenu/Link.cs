using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class Link : MonoBehaviour 
{

	public void OpenVSVLink()
	{
#if !UNITY_EDITOR
		openWindow("https://www.vsv.be/");
#endif
    }

    [DllImport("__Internal")]
	private static extern void openWindow(string url);

}