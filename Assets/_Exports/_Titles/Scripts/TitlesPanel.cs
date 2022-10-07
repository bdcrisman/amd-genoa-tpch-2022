using Newtonsoft.Json;
using System;
using System.IO;
using TMPro;
using UnityEngine;

public class TitlesPanel : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _mainTitle;
    [SerializeField] private TextMeshProUGUI _compTitle;
    [SerializeField] private TextMeshProUGUI _compSubtitle;
    [SerializeField] private TextMeshProUGUI _amdTitle;
    [SerializeField] private TextMeshProUGUI _amdSubtitle;
    [SerializeField] private TextMeshProUGUI _disclaimer;

    private void OnEnable() {
        Setup();
    }

    private void Setup() {
        _mainTitle.text = _compTitle.text = _compSubtitle.text = _amdTitle.text = _amdSubtitle.text = _disclaimer.text = string.Empty;

        var t = LoadTitlesFromDisk();
        if (t == null || !t.IsDisplay) return;

        _mainTitle.text = t.MainTitle;
        _compTitle.text = t.CompTitle;
        _compSubtitle.text = t.CompSubtitle;
        _amdTitle.text = t.AmdTitle;
        _amdSubtitle.text = t.AmdSubtitle;
        _disclaimer.text = t.Disclaimer;
    }

    private TitlesDataModel LoadTitlesFromDisk() {
        try {
            return JsonConvert.DeserializeObject<TitlesDataModel>(File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "titles", "titles.json")));
        }catch(Exception ex) {
            print(ex.ToString());
            return null;
        }
    }
}

public class TitlesDataModel {
    [JsonProperty("display")] public bool IsDisplay { get; set; }
    [JsonProperty("main")] public string MainTitle { get; set; }
    [JsonProperty("comp")] public string CompTitle { get; set; }
    [JsonProperty("compSubtitle")] public string CompSubtitle { get; set; }
    [JsonProperty("amd")] public string AmdTitle { get; set; }
    [JsonProperty("amdSubtitle")] public string AmdSubtitle { get; set; }
    [JsonProperty("disclaimer")] public string Disclaimer { get; set; }
}
