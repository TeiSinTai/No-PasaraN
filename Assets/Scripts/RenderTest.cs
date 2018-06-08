using System.IO;
using UnityEngine;

public class RenderTest : MonoBehaviour
{
	public int imageWidth = 4096;
	public bool saveAsJPEG = true;

	void Update()
	{
		if( Input.GetKeyDown( KeyCode.P ) )
		{
			byte[] bytes = I360Render.Capture( imageWidth, saveAsJPEG );
			if( bytes != null )
			{
				string path = Path.Combine( Application.persistentDataPath, "360render" + ( saveAsJPEG ? ".jpeg" : ".png" ) );
				File.WriteAllBytes( path, bytes );
				Debug.Log( "360 render saved to " + path );
			}
		}
	}
}