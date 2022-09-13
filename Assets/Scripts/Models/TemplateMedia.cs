using RenderHeads.Media.AVProVideo;
using System.IO;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MediaPlayer))]
public class TemplateMedia : MonoBehaviour {
    private MediaPlayer _mp;

    private void Awake() {
        _mp = GetComponent<MediaPlayer>();
    }

    private void Start() {
        LoadTemplateMedia();
    }

    private void LoadTemplateMedia() {
        var dir = Path.Combine(Application.streamingAssetsPath, "media", "template");
        if (!Directory.Exists(dir)) {
            gameObject.SetActive(false);
            return;
        }

        var path = Directory.GetFiles(dir, "*.*", SearchOption.TopDirectoryOnly)
            .FirstOrDefault(p => p.ToLower().EndsWith(".mp4"));

        if (string.IsNullOrEmpty(path)) {
            gameObject.SetActive(false);
            return;
        }

        _mp.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, path, true);
        _mp.Control.SetLooping(true);
    }
}
