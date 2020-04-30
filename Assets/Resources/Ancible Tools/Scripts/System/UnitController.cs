﻿using System.Collections.Generic;
using System.Linq;
using Assets.Ancible_Tools.Scripts.Traits;
using Assets.Resources.Scripts.Game_System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Ancible_Tools.Scripts.System
{
    public class UnitController : MonoBehaviour
    {
        public Trait[] StartingTraits;

        private List<TraitController> _currentTraits = new List<TraitController>();

        internal virtual void Awake()
        {
            SubscribeToMessages();
        }

        internal virtual void Start()
        {
            for (var i = 0; i < StartingTraits.Length; i++)
            {
                gameObject.SendMessageTo(new AddTraitToUnitMessage{Trait = StartingTraits[i]}, gameObject);
            }
        }

        void FixedUpdate()
        {
            gameObject.SendMessageTo(UnitFixedUpdateMessage.INSTANCE, gameObject);
        }

        void Update()
        {
            gameObject.SendMessageTo(UnitUpdateMessage.INSTANCE, gameObject);
        }

        internal virtual void SubscribeToMessages()
        {
            gameObject.Subscribe<AddTraitToUnitMessage>(AddTraitToUnit);
            gameObject.Subscribe<RemoveTraitFromUnitMessage>(RemoveTraitFromUnit);
            gameObject.Subscribe<RemoveTraitFromUnitByControllerMessage>(RemoveTraitFromUnitByController);
            gameObject.Subscribe<TraitCheckMessage>(TraitCheck);
        }

        internal virtual void AddTraitToUnit(AddTraitToUnitMessage msg)
        {
            var traitCount = _currentTraits.Count(c => c.Trait.name == msg.Trait.name);
            if (traitCount < msg.Trait.MaxStack)
            {
                var controller = Instantiate(FactoryController.TRAIT_CONTROLLER, transform);
                _currentTraits.Add(controller);
                controller.Setup(msg.Trait);
            }
        }

        internal virtual void RemoveTraitFromUnit(RemoveTraitFromUnitMessage msg)
        {
            var controller = _currentTraits.Find(t => t.Trait.name == msg.Trait.name);
            if (controller)
            {
                controller.Destroy();
                _currentTraits.Remove(controller);
                Destroy(controller.gameObject);
            }
        }

        internal virtual void RemoveTraitFromUnitByController(RemoveTraitFromUnitByControllerMessage msg)
        {
            if (_currentTraits.Contains(msg.Controller))
            {
                msg.Controller.Destroy();
                _currentTraits.Remove(msg.Controller);
                Destroy(msg.Controller.gameObject);
            }
        }

        internal virtual void TraitCheck(TraitCheckMessage msg)
        {
            for (var i = 0; i < msg.TraitsToCheck.Count; i++)
            {
                if (_currentTraits.Exists(c => c.Trait.name == msg.TraitsToCheck[i].name))
                {
                    msg.DoAfter.Invoke();
                    return;
                }
            }
        }

        internal virtual void OnDestroy()
        {
            gameObject.UnsubscribeFromAllMessages();
            
        }
    }
}