using Assets.Ancible_Tools.Scripts.System;
using Assets.Ancible_Tools.Scripts.Traits;
using MessageBusLib;
using UnityEngine;

namespace Assets.Resources.Scripts.Game_System.Traits
{
    [CreateAssetMenu(fileName = "Attack Trait", menuName = "Deus Animex/Traits/Combat/Attack")]
    public class AttackTrait : Trait
    {
        [SerializeField] private Trait[] _attackTraits;
        [SerializeField] private float _offset;

        private UnitController _attackController = null;
        private UnitState _unitState = UnitState.Active;
        private Vector2 _direction = Vector2.down;

        private SetUnitStateMessage _setUnitStateMsg = new SetUnitStateMessage();
        private SetUnitAnimationStateMessage _setUnitAnimationStateMsg = new SetUnitAnimationStateMessage();
        private AddTraitToUnitMessage _addTraitToUnitMsg = new AddTraitToUnitMessage();
        private SetOwnerMessage _setOwnerMsg = new SetOwnerMessage();

        public override void SetupController(TraitController controller)
        {
            base.SetupController(controller);
            SubscribeToMessages();
        }

        internal virtual void SubscribeToMessages()
        {
            _controller.transform.parent.gameObject.SubscribeWithFilter<ActivateAttackMessage>(ActivateAttack, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<UpdateDirectionMessage>(UpdateDirection, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<AttackStartedMessage>(AttackStarted, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<AttackFinishedMessage>(AttackFinished, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<UpdateUnitStateMessage>(UpdateUnitState, _instanceId);
        }

        private void ActivateAttack(ActivateAttackMessage msg)
        {
            if (_unitState == UnitState.Active)
            {
                _setUnitStateMsg.State = UnitState.Attacking;
                _controller.gameObject.SendMessageTo(_setUnitStateMsg, _controller.transform.parent.gameObject);
                _setUnitAnimationStateMsg.State = UnitAnimationState.Attack;
                _controller.gameObject.SendMessageTo(_setUnitAnimationStateMsg, _controller.transform.parent.gameObject);
            }
        }

        private void UpdateDirection(UpdateDirectionMessage msg)
        {
            if (msg.Direction != Vector2.zero)
            {
                _direction = msg.Direction;
            }
        }

        private void AttackStarted(AttackStartedMessage msg)
        {
            var offset = _offset * _direction;
            //if (_direction.x < 0)
            //{
            //    offset.x = _offset * -1;
            //}
            //if (_direction.y < 0)
            //{
            //    offset.y = _offset * -1;
            //}
            var position = _controller.transform.parent.position.ToVector2();
            position += offset;
            _attackController = Instantiate(FactoryController.UNIT_CONTROLLER, position, Quaternion.identity);
            for (var i = 0; i < _attackTraits.Length; i++)
            {
                _addTraitToUnitMsg.Trait = _attackTraits[i];
                _controller.gameObject.SendMessageTo(_addTraitToUnitMsg, _attackController.gameObject);
            }
            _setOwnerMsg.Owner = _controller.transform.parent.gameObject;
            _controller.gameObject.SendMessageTo(_setOwnerMsg, _attackController.gameObject);
        }

        private void AttackFinished(AttackFinishedMessage msg)
        {
            if (_unitState == UnitState.Attacking)
            {
                _setUnitAnimationStateMsg.State = UnitAnimationState.Idle;
                _controller.gameObject.SendMessageTo(_setUnitAnimationStateMsg, _controller.transform.parent.gameObject);
                _setUnitStateMsg.State = UnitState.Active;
                _controller.gameObject.SendMessageTo(_setUnitStateMsg, _controller.transform.parent.gameObject);
            }
            else if (_attackController)
            {
                Destroy(_attackController.gameObject);
                _attackController = null;
            }
        }

        private void UpdateUnitState(UpdateUnitStateMessage msg)
        {
            if (msg.State != UnitState.Attacking && _unitState == UnitState.Attacking && _attackController)
            {
                Destroy(_attackController.gameObject);
                _attackController = null;
            }
            _unitState = msg.State;

        }
    }
}