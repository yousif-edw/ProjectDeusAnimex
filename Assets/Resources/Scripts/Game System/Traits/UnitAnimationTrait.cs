using Assets.Ancible_Tools.Scripts.System;
using Assets.Ancible_Tools.Scripts.Traits;
using Assets.Resources.Scripts.Game_System.Controllers;
using MessageBusLib;
using UnityEngine;

namespace Assets.Resources.Scripts.Game_System.Traits
{
    [CreateAssetMenu(fileName = "Unit Animation Trait", menuName = "Deus Animex/Traits/Unit Animation")]
    public class UnitAnimationTrait : Trait
    {
        private const string UNIT_ANIMATION_STATE = "Unit Animation State";
        private const string X = "X";
        private const string Y = "Y";

        [SerializeField] private RuntimeAnimatorController _runtimeController;
        [SerializeField] private bool _mirrorLeft;
        [SerializeField] private Color _color = Color.white;

        private UnitAnimationController _animationController = null;
        private Vector2 _direction = Vector2.down;
        private UnitAnimationState _animationState = UnitAnimationState.Idle;


        public override void SetupController(TraitController controller)
        {
            base.SetupController(controller);
            _animationController = Instantiate(FactoryController.UNIT_ANIMATION_CONTROLLER, _controller.transform.parent);
            _animationController.Animator.runtimeAnimatorController = _runtimeController;
            _animationController.SpriteRenderer.color = _color;
            SubscribeToMessages();
        }

        private void SubscribeToMessages()
        {
            _controller.transform.parent.gameObject.SubscribeWithFilter<UpdateDirectionMessage>(UpdateDirection, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetUnitAnimationStateMessage>(SetUnitAnimationState, _instanceId);
        }

        private void UpdateDirection(UpdateDirectionMessage msg)
        {
            if (msg.Direction != Vector2.zero)
            {
                var direction = msg.Direction;
                if (direction.x > 0)
                {
                    direction.x = 1;
                }
                else if (direction.x < 0)
                {
                    direction.x = -1;
                }

                if (direction.y > 0)
                {
                    direction.y = 1;
                }
                else if (direction.y < 0)
                {
                    direction.y = -1;
                }
                if (_direction != direction)
                {
                    _direction = direction;
                    if (_mirrorLeft && _direction.x > 0 || _direction.x < 0)
                    {
                        _animationController.SpriteRenderer.flipX = _direction.x < 0;
                    }
                    _animationController.Animator.SetFloat(X, _direction.x);
                    _animationController.Animator.SetFloat(Y, _direction.y);
                    _animationController.Animator.Play(0);
                }
            }
        }

        private void SetUnitAnimationState(SetUnitAnimationStateMessage msg)
        {
            if (_animationState != msg.State)
            {
                _animationState = msg.State;
                _animationController.Animator.SetFloat(UNIT_ANIMATION_STATE, (float)_animationState);
                _animationController.Animator.Play(0);
            }
        }

        public override void Destroy()
        {
            Destroy(_animationController.gameObject);
            base.Destroy();
        }
    }
}