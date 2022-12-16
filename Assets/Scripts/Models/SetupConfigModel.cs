using Newtonsoft.Json;

public class SetupConfigModel {
    [JsonProperty("durationSec")] public float DurationSec { get; set; }
    [JsonProperty("scoreLabel")] public string ScoreLabel { get; set; }
    [JsonProperty("amdFinalDataValue")] public float AmdFinalDataValue { get; set; }
    [JsonProperty("compFinalDataValue")] public float CompFinalDataValue { get; set; }
}
