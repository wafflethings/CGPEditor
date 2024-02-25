using UnityEngine;

namespace CgpEditor.IO
{
    public class WebGlFileReceiver : MonoSingleton<WebGlFileReceiver>
    {
        public void OnFileUpload(string url)
        {
#if UNITY_WEBGL
            Debug.Log("OnFileUpload called for " + url);
            StartCoroutine(FileIO.LoadFileWeb(url));
#endif
        }

        public void OnFileDownload()
        {
            Debug.Log("File downloaded i think");
        }
    }
}
