using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Loyufei.ViewManagement;

namespace Gomuku
{
    public class ConfirmView : MenuBase
    {
        [SerializeField]
        private TextMeshProUGUI _Message;

        public void SetMessage(string message) 
        {
            _Message.SetText(message);
        }
    }
}