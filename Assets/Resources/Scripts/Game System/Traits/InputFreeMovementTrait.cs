using Assets.Ancible_Tools.Scripts.Traits;
using MessageBusLib;
using UnityEngine;

namespace Assets.Resources.Scripts.Game_System.Traits
{
    [CreateAssetMenu(fileName = "Input Free Movement Trait", menuName = "Deus Animex/Traits/Input/Input Free Movement")]
    public class InputFreeMovementTrait : Trait
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
            _setDirectionMsg.Direction = msg.Current.Direction;
            _controller.gameObject.SendMessageTo(_setDirectionMsg, _controller.transform.parent.gameObject);
        }
    }
}