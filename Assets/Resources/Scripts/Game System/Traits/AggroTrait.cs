using Assets.Ancible_Tools.Scripts.Hitbox;
using Assets.Ancible_Tools.Scripts.Traits;
using Assets.Resources.Scripts.Game_System.Controllers;
using MessageBusLib;
using UnityEngine;

namespace Assets.Resources.Scripts.Game_System.Traits
{
    [CreateAssetMenu(fileName = "Aggro Trait", menuName = "Deus Animex/Traits/Combat/Aggro")]
    public class AggroTrait : Trait
    {
        [SerializeField] private Hitbox _hitbox;

        private HitboxController _hitboxController;
        private GameObject _player = null;
        private Vector2 _direction = Vector2.zero;
        private Vector2 _position = Vector2.zero;
        private UnitState _unitState = UnitState.Active;

        private PlayerCheckMessage _playerCheckMsg = new PlayerCheckMessage();
        private SetDirectionMessage _setDirectionMsg = new SetDirectionMessage();
        private AttackCheckMessage _attackCheckMsg = new AttackCheckMessage();

        public override void SetupController(TraitController controller)
        {
            base.SetupController(controller);
            var hitboxFilter = HitboxController.GenerateHitboxFilter(_hitbox, CollisionLayerFactory.AGGRO);
            _controller.gameObject.SendMessageWithFilterTo(new HitboxCheckMessage{DoAfter = hitboxController =>
            {
                _hitboxController = hitboxController;
            }}, _controller.transform.parent.gameObject, hitboxFilter);
            if (!_hitboxController)
            {
                _hitboxController = Instantiate(_hitbox.Controller, _controller.transform.parent);
                _hitboxController.Setup(_hitbox, CollisionLayerFactory.AGGRO);
            }
            _controller.gameObject.SendMessageTo(new RegisterCollisionMessage{Object = _controller.gameObject}, _hitboxController.gameObject);
            SubscribeToMessages();
        }

        private void SubscribeToMessages()
        {
            _controller.gameObject.SubscribeWithFilter<EnterCollisionWithObjectMessage>(EnterCollisionWithObject, _instanceId);
            _controller.gameObject.SubscribeWithFilter<ExitCollisionWithObjectMessage>(ExitCollisionWithObject, _instanceId);

            _controller.transform.parent.gameObject.SubscribeWithFilter<UnitUpdateMessage>(UnitUpdate, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<UpdateDirectionMessage>(UpdateDirection, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<UpdatePositionMessage>(UpdatePosition, _instanceId);
        }

        private void UnitUpdate(UnitUpdateMessage msg)
        {
            if (_player && _unitState == UnitState.Active)
            {
                var playerPosition = _player.transform.position.ToVector2();
                var attack = false;
                _attackCheckMsg.Position = playerPosition;
                _attackCheckMsg.DoAfter = () =>
                {
                    attack = true;
                };
                _controller.gameObject.SendMessageTo(_attackCheckMsg, _controller.transform.parent.gameObject);
                var direction = (_player.transform.position.ToVector2() - _position).normalized;
                if (_direction != direction)
                {
                    _setDirectionMsg.Direction = direction;
                    _controller.gameObject.SendMessageTo(_setDirectionMsg, _controller.transform.parent.gameObject);
                }
                if (attack)
                {
                    _controller.gameObject.SendMessageTo(ActivateAttackMessage.INSTANCE, _controller.transform.parent.gameObject);
                }
            }
        }

        private void EnterCollisionWithObject(EnterCollisionWithObjectMessage msg)
        {
            var isPlayer = false;
            _playerCheckMsg.DoAfter = () =>
            {
                isPlayer = true;
            };
            _controller.gameObject.SendMessageTo(_playerCheckMsg, msg.Object);
            if (isPlayer)
            {
                _player = msg.Object;
            }
        }

        private void ExitCollisionWithObject(ExitCollisionWithObjectMessage msg)
        {
            var isPlayer = false;
            _playerCheckMsg.DoAfter = () =>
            {
                isPlayer = true;
            };
            _controller.gameObject.SendMessageTo(_playerCheckMsg, msg.Object);
            if (isPlayer)
            {
                _player = null;
            }
        }

        private void UpdateDirection(UpdateDirectionMessage msg)
        {
            _direction = msg.Direction;
        }

        private void UpdatePosition(UpdatePositionMessage msg)
        {
            _position = msg.Position;
        }

        private void UpdateUnitState(UpdateUnitStateMessage msg)
        {
            _unitState = msg.State;
        }
    }
}