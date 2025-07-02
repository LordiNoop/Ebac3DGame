using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

namespace Itens
{
    public class ItemLayout : MonoBehaviour
    {
        private ItemSetup _currSetup;

        public Image uiIcon;
        public TextMeshProUGUI uiValue;
        public Image uiKey;

        private void Start()
        {
            if (_currSetup.uiKey == null)
            {
                uiKey.enabled = false;
            }
        }

        private void Update()
        {
            uiValue.text = _currSetup.soInt.value.ToString();
        }

        public void Load(ItemSetup setup)
        {
            _currSetup = setup;
            UpdateUI();
        }

        private void UpdateUI()
        {
            uiIcon.sprite = _currSetup.icon;
        }
    }
}
