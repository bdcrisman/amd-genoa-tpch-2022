using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

namespace BC2_AMD_Hero {
    [RequireComponent(typeof(CanvasGroup))]
    public class Hero : MonoBehaviour {
        const float DisplayDuration = 0.5f;

        [SerializeField] private TextMeshProUGUI _delta;
        [SerializeField] private TextMeshProUGUI _deltaSubtitle;
        [SerializeField] private TextMeshProUGUI _message;
        [SerializeField] private TextMeshProUGUI _subtitle;

        private CanvasGroup _cg;
        private float _delayToDisplay;
        private bool _isDisplay;

        private void OnEnable() {
            Setup();

            _cg = GetComponent<CanvasGroup>();
            _cg.alpha = 0;
        }

        private void Setup() {
            var h = LoadHeroFromDisk();
            if (h == null) return;

            _isDisplay = h.IsDisplay;
            _delayToDisplay = h.DelayToDisplay;
            _delta.text = $"{h.Delta}{h.DeltaUnits}";
            _deltaSubtitle.text = h.DeltaSubtitle;
            _message.text = h.Message;
            _subtitle.text = h.Subtitle;
        }

        public float Display() {
            if (!_isDisplay) return -1;
            StartCoroutine(DisplayCo());
            return _delayToDisplay;
        }

        private IEnumerator DisplayCo() {
            yield return new WaitForSeconds(_delayToDisplay);

            var t = 0f;
            while (t < DisplayDuration) {
                _cg.alpha = Mathf.Lerp(0, 1, t / DisplayDuration);
                t += Time.deltaTime;
                yield return null;
            }

            _cg.alpha = 1;
        }

        private HeroModel LoadHeroFromDisk() {
            try {
                return JsonConvert.DeserializeObject<HeroModel>(File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "hero", "hero.json")));
            } catch (Exception ex) {
                print(ex.ToString());
                return null;
            }
        }
    }

    public class HeroModel {
        [JsonProperty("display")] public bool IsDisplay { get; set; }
        [JsonProperty("delayToDisplay")] public float DelayToDisplay { get; set; }
        [JsonProperty("delta")] public string Delta { get; set; }
        [JsonProperty("deltaUnits")] public string DeltaUnits { get; set; }
        [JsonProperty("deltaSubtitle")] public string DeltaSubtitle { get; set; }
        [JsonProperty("message")] public string Message { get; set; }
        [JsonProperty("subtitle")] public string Subtitle { get; set; }
    }
}