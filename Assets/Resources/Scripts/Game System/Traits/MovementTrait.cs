using Assets.Ancible_Tools.Scripts.Traits;
using MessageBusLib;
using UnityEngine;

namespace Assets.Resources.Scripts.Game_System.Traits
{
    [CreateAssetMenu(fileName = "Movement Trait", menuName = "Deus Animex/Traits/Movement")]
    public class MovementTrait : Trait
    {
        [SerializeField] private float _moveSpeed;

        private Vector2 _direction = Vector2.zero;
        private Rigidbody2D _rigidBody = null;
        private UnitState _unitState = UnitState.Active;

        private UpdateDirectionMessage _updateDirectionMsg = new UpdateDirectionMessage();
        private SetUnitAnimationStateMessage _setUnitAnimationStateMsg = new SetUnitAnimationStateMessage();
        private UpdatePositionMessage _updatePositionMsg = new UpdatePositionMessage();

        public override void SetupController(TraitController controller)
        {
            base.SetupController(controller);
            _rigidBody = _controller.transform.parent.gameObject.GetComponent<Rigidbody2D>();
            SubscribeToMessages();
            _controller.StartCoroutine(StaticMethods.WaitForFrames(1, () =>
            {
                _controller.transform.parent.gameObject.SendMessageTo(RequestPositionMessage.INSTANCE,
                    _controller.transform.parent.gameObject);
            }));
        }

        private void SubscribeToMessages()
        {
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetDirectionMessage>(SetDirection, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<UnitFixedUpdateMessage>(UnitFixedUpdate, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<UpdateUnitStateMessage>(UpdateUnitState, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<RequestPositionMessage>(RequestPosition, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetPositionMessage>(SetPosition, _instanceId);
        }

        private void SetDirection(SetDirectionMessage msg)
        {
            if (_unitState == UnitState.Active && _direction != msg.Direction)
            {
                _direction = msg.Direction;
                _updateDirectionMsg.Direction = _direction;
                _controller.gameObject.SendMessageTo(_updateDirectionMsg, _controller.transform.parent.gameObject);
                _setUnitAnimationStateMsg.State = _direction == Vector2.zero ? UnitAnimationState.Idle : UnitAnimationState.Walk;
                _controller.gameObject.SendMessageTo(_setUnitAnimationStateMsg, _controller.transform.parent.gameObject);
            }

        }

        private void UnitFixedUpdate(UnitFixedUpdateMessage msg)
        {
            if (_direction != Vector2.zero && _unitState == UnitState.Active)
            {
                var position = _rigidBody.position;
                var moveSpeed = _moveSpeed * Time.fixedDeltaTime;
                position += Vector2.ClampMagnitude(_direction * moveSpeed, moveSpeed);
                //_rigidBody.position = position;
                _rigidBody.MovePosition(position);
                _updatePositionMsg.Position = _rigidBody.position;
                _controller.gameObject.SendMessageTo(_updatePositionMsg, _controller.transform.parent.gameObject);
            }
        }

        private void UpdateUnitState(UpdateUnitStateMessage msg)
        {
            _unitState = msg.State;
        }

        private void RequestPosition(RequestPositionMessage msg)
        {
            _controller.gameObject.SendMessageTo(_updatePositionMsg, msg.Sender);
        }

        private void SetPosition(SetPositionMessage msg)
        {
            _rigidBody.position = msg.Position;
            _updatePositionMsg.Position = _rigidBody.position;
            _controller.gameObject.SendMessageTo(_updatePositionMsg, _controller.transform.parent.gameObject);
        }
    }
}