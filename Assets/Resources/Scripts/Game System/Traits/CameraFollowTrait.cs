using Assets.Ancible_Tools.Scripts.Traits;
using MessageBusLib;
using UnityEngine;

namespace Assets.Resources.Scripts.Game_System.Traits
{
    [CreateAssetMenu(fileName = "Camera Follow Trait", menuName = "Deus Animex/Traits/Camera Follow")]
    public class CameraFollowTrait : Trait
    {
        private SetCameraPositionMessage _setCameraPositionMsg = new SetCameraPositionMessage();

        public override void SetupController(TraitController controller)
        {
            base.SetupController(controller);
            SubscribeToMessages();
        }

        private void SubscribeToMessages()
        {
            _controller.transform.parent.gameObject.SubscribeWithFilter<UpdatePositionMessage>(UpdatePosition, _instanceId);
        }

        private void UpdatePosition(UpdatePositionMessage msg)
        {
            _setCameraPositionMsg.Position = msg.Position;
            _controller.gameObject.SendMessage(_setCameraPositionMsg);
        }
    }
}