using MessageBusLib;
using UnityEngine;

namespace Assets.Ancible_Tools.Scripts.Hitbox
{
    public class TriggerHitboxController : HitboxController
    {
        void OnTriggerEnter(Collider col)
        {
            if (col.transform.parent)
            {
                var enterCollisionMsg = new EnterCollisionWithObjectMessage{Object = col.transform.parent.gameObject};
                for (var i = 0; i < _subscribers.Count; i++)
                {
                    gameObject.SendMessageTo(enterCollisionMsg, _subscribers[i]);
                }
            }
        }

        void OnTriggerExit(Collider col)
        {
            if (col.transform.parent)
            {
                var exitCollisionMsg = new ExitCollisionWithObjectMessage{Object = col.transform.parent.gameObject};
                for (var i = 0; i < _subscribers.Count; i++)
                {
                    gameObject.SendMessageTo(exitCollisionMsg, _subscribers[i] );
                }
            }
        }
    }
}