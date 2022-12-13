using Newtonsoft.Json;

public class SetupConfigModel {
    [JsonProperty("durationSec")] public float DurationSec { get; set; }
    [JsonProperty("delta")] public float Delta { get; set; }
    [JsonProperty("deltaUnit")] public string DeltaUnits { get; set; }
}
