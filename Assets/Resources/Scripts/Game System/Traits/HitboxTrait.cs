using System.Collections.Generic;
using Assets.Ancible_Tools.Scripts.Hitbox;
using Assets.Ancible_Tools.Scripts.Traits;
using MessageBusLib;
using UnityEngine;

namespace Assets.Resources.Scripts.Game_System.Traits
{
    [CreateAssetMenu(fileName = "Hitbox Trait", menuName = "Deus Animex/Traits/Hitbox")]
    public class HitboxTrait : Trait
    {
        [SerializeField] private Hitbox _hitbox;
        [SerializeField] private CollisionLayer _collisionLayer;

        [SerializeField] private List<Trait> _requiredTraits;

        [SerializeField] private Trait[] _applyToTargetOnEnter;
        [SerializeField] private Trait[] _applyToTargetOnExit;

        [SerializeField] private Trait[] _applyToSelfOnEnter;
        [SerializeField] private Trait[] _applyToSelfOnExit;

        private HitboxController _hitboxController = null;
        private bool _register = false;
        private GameObject _owner = null;
        
        private TraitCheckMessage _traitCheckMsg = new TraitCheckMessage();
        private AddTraitToUnitMessage _addTraitToUnitMsg = new AddTraitToUnitMessage();

        public override void SetupController(TraitController controller)
        {
            base.SetupController(controller);
            var hitboxFilter = HitboxController.GenerateHitboxFilter(_hitbox, _collisionLayer);
            _controller.gameObject.SendMessageWithFilterTo(new HitboxCheckMessage{DoAfter = hitboxController =>
            {
                _hitboxController = hitboxController;
            }}, _controller.transform.parent.gameObject, hitboxFilter);
            if (!_hitboxController)
            {
                _hitboxController = Instantiate(_hitbox.Controller, _controller.transform.parent);
                _hitboxController.Setup(_hitbox, _collisionLayer);
            }
            _register = _applyToTargetOnEnter.Length > 0 || _applyToTargetOnExit.Length > 0 || _applyToSelfOnEnter.Length > 0 || _applyToSelfOnExit.Length > 0;
            if (_register)
            {
                _traitCheckMsg.TraitsToCheck = _requiredTraits;
                _controller.gameObject.SendMessageTo(new RegisterCollisionMessage{Object = _controller.gameObject}, _hitboxController.gameObject);
                SubscribeToMessages();
            }

        }

        private void SubscribeToMessages()
        {
            _controller.gameObject.SubscribeWithFilter<EnterCollisionWithObjectMessage>(EnterCollisionWithObject, _instanceId);
            _controller.gameObject.SubscribeWithFilter<ExitCollisionWithObjectMessage>(ExitCollisionWithObject, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<UpdateOwnerMessage>(UpdateOwner, _instanceId);
        }

        private void EnterCollisionWithObject(EnterCollisionWithObjectMessage msg)
        {
            if (!_owner || msg.Object != _owner)
            {
                var apply = false;
                if (_requiredTraits.Count > 0)
                {
                    _traitCheckMsg.DoAfter = () =>
                    {
                        apply = true;
                    };
                    _controller.gameObject.SendMessageTo(_traitCheckMsg, msg.Object);
                }
                else
                {
                    apply = true;
                }
                if (apply)
                {
                    for (var i = 0; i < _applyToTargetOnEnter.Length; i++)
                    {
                        _addTraitToUnitMsg.Trait = _applyToTargetOnEnter[i];
                        _controller.gameObject.SendMessageTo(_addTraitToUnitMsg, msg.Object);
                    }

                    for (var i = 0; i < _applyToSelfOnEnter.Length; i++)
                    {
                        _addTraitToUnitMsg.Trait = _applyToSelfOnEnter[i];
                        _controller.gameObject.SendMessageTo(_addTraitToUnitMsg, _controller.transform.parent.gameObject);
                    }
                }
            }

        }

        private void ExitCollisionWithObject(ExitCollisionWithObjectMessage msg)
        {
            if (!_owner || _owner != msg.Object)
            {
                var apply = false;
                if (_requiredTraits.Count > 0)
                {
                    _traitCheckMsg.DoAfter = () =>
                    {
                        apply = true;
                    };
                    _controller.gameObject.SendMessageTo(_traitCheckMsg, msg.Object);
                }
                else
                {
                    apply = true;
                }
                if (apply)
                {
                    for (var i = 0; i < _applyToTargetOnExit.Length; i++)
                    {
                        _addTraitToUnitMsg.Trait = _applyToTargetOnEnter[i];
                        _controller.gameObject.SendMessageTo(_addTraitToUnitMsg, msg.Object);
                    }

                    for (var i = 0; i < _applyToSelfOnExit.Length; i++)
                    {
                        _addTraitToUnitMsg.Trait = _applyToSelfOnEnter[i];
                        _controller.gameObject.SendMessageTo(_addTraitToUnitMsg, _controller.transform.parent.gameObject);
                    }
                }
            }
        }

        private void UpdateOwner(UpdateOwnerMessage msg)
        {
            _owner = msg.Owner;
        }

        public override void Destroy()
        {
            if (_register)
            {
                _controller.gameObject.SendMessageTo(new UnregisterCollisionMessage {Object = _controller.gameObject}, _hitboxController.gameObject);
            }
            base.Destroy();
        }
    }
}