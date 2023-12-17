using UnityEngine;
using HighlightPlus;

namespace HighlightPlus.Demos {

    public class ManualSelectionDemo : MonoBehaviour {

        HighlightManager hm;

        public Transform objectToSelect;

        void Start() {
            hm = FindObjectOfType<HighlightManager>();
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.Keypad1)) {
                hm.SelectObject(objectToSelect);
            }
            if (Input.GetKeyDown(KeyCode.Keypad2)) {
                hm.ToggleObject(objectToSelect);
            }
            if (Input.GetKeyDown(KeyCode.Keypad3)) {
                hm.UnselectObject(objectToSelect);
            }
        }
    }
}
