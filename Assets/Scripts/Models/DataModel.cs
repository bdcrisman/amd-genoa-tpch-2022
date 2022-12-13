using System.Collections.Generic;
using System.IO;
using System.Linq;

public class DataModel {
    public List<float> Durations = new();
    public List<float> QueriesPerHour = new();

    public DataModel(string path) {
        LoadDataFromCSV(path);
    }

    public bool LoadDataFromCSV(string path) {
        if (string.IsNullOrWhiteSpace(path) || !path.ToLower().EndsWith(".csv") || !File.Exists(path)) {
            return false;
        }

        try {
            File.ReadAllLines(path)
                .Select(x => x.Split(","))
                .ToList()
                .ForEach(x => {
                    Durations.Add(float.Parse(x[0]));
                    QueriesPerHour.Add(float.Parse(x[1]));
                });
        } catch {
            return false; 
        }

        return true;
    }
}
