﻿using MessageBusLib;
using UnityEngine;

namespace Assets.Ancible_Tools.Scripts.Traits
{
    [CreateAssetMenu(fileName = "Trait", menuName = "Deus Animex/Traits/Trait", order = 0)]
    public class Trait : ScriptableObject
    {
        public int MaxStack = 1;

        protected internal TraitController _controller;
        protected internal string _instanceId;

        public virtual void SetupController(TraitController controller)
        {
            _controller = controller;
            _instanceId = _controller.gameObject.GetInstanceID().ToString();
        }

        public virtual void Destroy()
        {
            _controller.gameObject.UnsubscribeFromAllMessages();
            _controller.transform.parent.gameObject.UnsubscribeFromAllMessagesWithFilter(_instanceId);
        }
    }
}