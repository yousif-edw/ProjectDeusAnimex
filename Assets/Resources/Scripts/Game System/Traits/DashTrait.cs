using Assets.Ancible_Tools.Scripts.Traits;
using DG.Tweening;
using MessageBusLib;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Assets.Resources.Scripts.Game_System.Traits
{
    [CreateAssetMenu(fileName = "Dash Trait", menuName = "Deus Animex/Traits/Dash")]
    public class DashTrait : Trait
    {
        [SerializeField] private float _dashSpeed;
        [SerializeField] private float _dashDistance;
        [SerializeField] private float _cooldown;

        private Vector2 _direction = Vector2.down;
        private float _currentDashDistance = 0f;
        private UnitState _unitState = UnitState.Active;
        private Rigidbody2D _rigidBody = null;
        private Sequence _cooldownTimer = null;

        private SetUnitStateMessage _setUnitStateMsg = new SetUnitStateMessage();
        private UpdatePositionMessage _updatePositionMsg = new UpdatePositionMessage();

        public override void SetupController(TraitController controller)
        {
            base.SetupController(controller);
            _rigidBody = _controller.transform.parent.gameObject.GetComponent<Rigidbody2D>();
            SubscribeToMessages();
        }

        private void SubscribeToMessages()
        {
            _controller.transform.parent.gameObject.SubscribeWithFilter<UpdateDirectionMessage>(UpdateDirection, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<ActivateDashMessage>(ActivateDash, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<DeactivateDashMessage>(DeactivateDash, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<UpdateUnitStateMessage>(UpdateUnitState, _instanceId);
        }

        private void UnitFixedUpdate(UnitFixedUpdateMessage msg)
        {
            if (_unitState == UnitState.Dashing)
            {
                if (_currentDashDistance < _dashDistance)
                {
                    var position = _rigidBody.position;
                    var dashSpeed = _dashSpeed * Time.fixedDeltaTime;
                    _currentDashDistance += dashSpeed;
                    position += Vector2.ClampMagnitude(_direction * dashSpeed, dashSpeed);
                    _rigidBody.position = position;
                    _updatePositionMsg.Position = _rigidBody.position;
                    _controller.gameObject.SendMessageTo(_updatePositionMsg, _controller.transform.parent.gameObject);
                    if (_currentDashDistance >= _dashDistance)
                    {
                        _controller.gameObject.SendMessageTo(DeactivateDashMessage.INSTANCE, _controller.transform.parent.gameObject);
                    }
                }
                else
                {
                    _controller.gameObject.SendMessageTo(DeactivateDashMessage.INSTANCE, _controller.transform.parent.gameObject);
                }
            }
            else
            {
                _controller.gameObject.SendMessageTo(DeactivateDashMessage.INSTANCE, _controller.transform.parent.gameObject);
            }
        }

        private void UpdateDirection(UpdateDirectionMessage msg)
        {
            if (_unitState == UnitState.Active && msg.Direction != Vector2.zero)
            {
                _direction = msg.Direction;
            }
        }

        private void ActivateDash(ActivateDashMessage msg)
        {
            if (_unitState == UnitState.Active && _cooldownTimer == null)
            {
                _currentDashDistance = 0f;
                _setUnitStateMsg.State = UnitState.Dashing;
                _controller.gameObject.SendMessageTo(_setUnitStateMsg, _controller.transform.parent.gameObject);
                _controller.transform.parent.gameObject.UnsubscribeFromFilter<UpdateDirectionMessage>(_instanceId);
                _controller.transform.parent.gameObject.SubscribeWithFilter<UnitFixedUpdateMessage>(UnitFixedUpdate, _instanceId);

            }
        }

        private void DeactivateDash(DeactivateDashMessage msg)
        {
            if (_unitState == UnitState.Dashing)
            {
                _cooldownTimer = DOTween.Sequence().AppendInterval(_cooldown).OnComplete(() =>
                {
                    _cooldownTimer = null;
                });
                _setUnitStateMsg.State = UnitState.Active;
                _controller.gameObject.SendMessageTo(_setUnitStateMsg, _controller.transform.parent.gameObject);
                _controller.transform.parent.gameObject.UnsubscribeFromFilter<UnitFixedUpdateMessage>(_instanceId);
                _controller.transform.parent.gameObject.SubscribeWithFilter<UpdateDirectionMessage>(UpdateDirection, _instanceId);
            }
        }

        private void UpdateUnitState(UpdateUnitStateMessage msg)
        {
            _unitState = msg.State;
        }

        public override void Destroy()
        {
            if (_cooldownTimer != null)
            {
                if (_cooldownTimer.IsActive())
                {
                    _cooldownTimer.Kill();
                }
                _cooldownTimer = null;
            }
            base.Destroy();
        }
    }
}