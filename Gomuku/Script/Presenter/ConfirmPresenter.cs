using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Loyufei.DomainEvents;
using UnityEngine;

namespace Gomuku
{
    public class ConfirmPresenter : Presenter
    {
        public ConfirmPresenter(ConfirmView view, DomainEventService service) : base(service)
        {
            View = view;

            Init();
        }

        public ConfirmView View { get; }

        private Action _OnConfirm = () => { };
        private Action _OnCancel  = () => { };

        protected override void RegisterEvents()
        {
            DomainEventService.Register<SendConfirm>(Confirm, GroupId);
        }

        private void Init() 
        {
            var buttons = View.OfType<ButtonListener>().ToDictionary(k => k.Id);

            buttons[0].AddListener((id) =>
            {
                _OnConfirm?.Invoke();

                View.Close();
            });
            buttons[1].AddListener((id) =>
            {
                _OnCancel?.Invoke();

                View.Close();
            });
        }

        private void Confirm(SendConfirm confirm) 
        {
            _OnConfirm = confirm.OnConfirm;
            _OnCancel  = confirm.OnCancel;

            View.SetMessage(confirm.Message);

            View.Open();
        }
    }

    public class SendConfirm : DomainEventBase 
    {
        public SendConfirm(string message, Action onConfirm, Action onCancel)
        {
            Message   = message;
            OnConfirm = onConfirm;
            OnCancel  = onCancel;
        }

        public string Message   { get; }
        public Action OnConfirm { get; }
        public Action OnCancel  { get; }
    }
}