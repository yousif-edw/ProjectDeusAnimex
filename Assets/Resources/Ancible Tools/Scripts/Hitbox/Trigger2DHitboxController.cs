﻿using MessageBusLib;
using UnityEngine;

namespace Assets.Ancible_Tools.Scripts.Hitbox
{
    public class Trigger2DHitboxController : HitboxController
    {
        void OnTriggerEnter2D(Collider2D col)
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

        void OnTriggerExit2D(Collider2D col)
        {
            if (col.transform.parent)
            {
                var exitCollisionMsg = new ExitCollisionWithObjectMessage{Object = col.transform.parent.gameObject};
                for (var i = 0; i < _subscribers.Count; i++)
                {
                    gameObject.SendMessageTo(exitCollisionMsg, _subscribers[i]);
                }
            }
        }
    }
}