using MessageBusLib;
using UnityEngine;

namespace Assets.Resources.Scripts.Game_System.Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float _interpolation = 1f;

        private Vector2 _position = Vector2.zero;

        void Awake()
        {
            SubscribeToMessages();
        }

        void LateUpdate()
        {
            var position = transform.position.ToVector2();
            if (position != _position)
            {
                var lerpedPosition = Vector2.Lerp(position,_position, _interpolation);
                var pos = transform.position;
                pos.x = lerpedPosition.x;
                pos.y = lerpedPosition.y;
                transform.position = pos;
            }
        }

        private void SubscribeToMessages()
        {
            gameObject.Subscribe<SetCameraPositionMessage>(SetCameraPosition);
        }

        private void SetCameraPosition(SetCameraPositionMessage msg)
        {
            _position = msg.Position;
        }

        void OnDestroy()
        {
            gameObject.UnsubscribeFromAllMessages();
        }
    }
}