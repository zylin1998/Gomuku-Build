using Loyufei;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Gomuku
{
    public class TextContext : MonoBehaviour, IUpdateContext
    {
        [SerializeField]
        protected TextMeshProUGUI _Text;
        [SerializeField]
        protected string          _Id;

        public object Id => _Id;

        public virtual void SetContext(object context) 
        {
            _Text?.SetText(context.To<string>());
        }
    }
}