using System.Collections;
using TMPro;
using UnityEngine;

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
            _cg = GetComponent<CanvasGroup>();
            _cg.alpha = 0;
        }

        public void Setup(HeroModel h) {
            _isDisplay = h.IsDisplay;
            _delayToDisplay = h.DelayToDisplay;
            _delta.text = h.Delta;
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
    }

    public class HeroModel {
        public bool IsDisplay { get; set; }
        public float DelayToDisplay { get; set; }
        public string Delta { get; set; }
        public string DeltaSubtitle { get; set; }
        public string Message { get; set; }
        public string Subtitle { get; set; }
    }
}