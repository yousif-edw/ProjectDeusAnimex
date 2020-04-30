using Assets.Ancible_Tools.Scripts.Traits;
using MessageBusLib;
using UnityEngine;

namespace Assets.Resources.Scripts.Game_System.Traits
{
    [CreateAssetMenu(fileName = "Input Movement Trait", menuName = "Deus Animex/Traits/Input/Input Movement")]
    public class InputMovementTrait : Trait
    {
        private SetDirectionMessage _setDirectionMsg = new SetDirectionMessage();

        public override void SetupController(TraitController controller)
        {
            base.SetupController(controller);
            SubscribeToMessages();
        }

        private void SubscribeToMessages()
        {
            _controller.gameObject.Subscribe<UpdateInputMessage>(UpdateInput);
        }

        private void UpdateInput(UpdateInputMessage msg)
        {
            var direction = Vector2.zero;
            if (msg.Current.Up)
            {
                direction.y = 1;
            }
            else if (msg.Current.Down)
            {
                direction.y = -1;
            }

            if (msg.Current.Left)
            {
                direction.x = -1;
            }
            else if (msg.Current.Right)
            {
                direction.x = 1;
            }
            _setDirectionMsg.Direction = direction;
            _controller.gameObject.SendMessageTo(_setDirectionMsg, _controller.transform.parent.gameObject);
        }
    }
}