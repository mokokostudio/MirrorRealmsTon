using UnityEngine;

namespace CameraTest {
    public class ScreenAdaption : MonoBehaviour {
        public bool m_Reverse;

        private RectTransform m_Transform;

        private void Awake() {
            m_Transform = transform as RectTransform;
        }

        private void Update() {
            var a = Screen.width / (float)Screen.height;
            var b = 16 / 9f;
            var sf = a > b ? Screen.height / 1440f : Screen.width / 2560f;
            var sa = Screen.safeArea;
            var min = sa.min / sf;
            var max = (sa.max - new Vector2(Screen.width, Screen.height)) / sf;
            if (m_Reverse) {
                m_Transform.offsetMin = -min;
                m_Transform.offsetMax = -max;
            } else {
                m_Transform.offsetMin = min;
                m_Transform.offsetMax = max;
            }
        }
    }
}
