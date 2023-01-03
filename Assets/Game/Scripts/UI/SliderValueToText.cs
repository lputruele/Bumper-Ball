using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

namespace Game.UI
{
    public class SliderValueToText : MonoBehaviour
    {
        public Slider sliderUI;
        private TMP_Text textSliderValue;

        void Start()
        {
            ShowSliderValue();
        }

        public void ShowSliderValue()
        {
            if (textSliderValue == null)
                textSliderValue = GetComponent<TMP_Text>();
            textSliderValue.text = "" + sliderUI.value;
        }
    }
}