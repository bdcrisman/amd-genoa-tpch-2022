using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

public class ConfigManager : MonoBehaviour {
    readonly string ConfigPath = Path.Combine(Application.streamingAssetsPath, "config", "setup.json");
    readonly string AmdDataPath = Path.Combine(Application.streamingAssetsPath, "data", "amd.csv");
    readonly string CompDataPath = Path.Combine(Application.streamingAssetsPath, "data", "comp.csv");

    public EventHandler AllLoaded;

    public SetupConfigModel SetupConfig { get; private set; }
    public DataModel AmdData { get; private set; }
    public DataModel CompData { get; private set; }

    void Start() {
        if (LoadSetupConfig() && LoadDataModels()) {
            AllLoaded?.Invoke(this, EventArgs.Empty);
        }
    }

    private bool LoadSetupConfig() {
        if (!File.Exists(ConfigPath)) {
            print($"Not exist: {ConfigPath}");
            return false;
        }

        try {
            SetupConfig = JsonConvert.DeserializeObject<SetupConfigModel>(File.ReadAllText(ConfigPath));
            return true;
        } catch (Exception ex) {
            print(ex.ToString());
            return false;
        }
    }

    private bool LoadDataModels() {
        if (!File.Exists(AmdDataPath) || !File.Exists(CompDataPath)) {
            print($"Data paths not exist");
            return false;
        }

        AmdData = new DataModel(AmdDataPath);
        CompData = new DataModel(CompDataPath);

        return AmdData.IsLoaded && CompData.IsLoaded;
    }
}
