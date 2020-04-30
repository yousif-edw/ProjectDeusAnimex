using System.Collections.Generic;
using Assets.Ancible_Tools.Scripts.System;
using Assets.Ancible_Tools.Scripts.Traits;
using MessageBusLib;
using UnityEngine;

namespace Assets.Resources.Scripts.Game_System.Controllers
{
    public class DevController : MonoBehaviour
    {
        [SerializeField] private List<Trait> _playerStartingTraits;
        [SerializeField] private Transform _startingPosition;


        void Start()
        {
            var playerController = Instantiate(FactoryController.UNIT_CONTROLLER, _startingPosition.transform.position.ToVector2(), Quaternion.identity);
            var addTraitToUnitMsg = new AddTraitToUnitMessage();
            for (var i = 0; i < _playerStartingTraits.Count; i++)
            {
                addTraitToUnitMsg.Trait = _playerStartingTraits[i];
                gameObject.SendMessageTo(addTraitToUnitMsg, playerController.gameObject);
            }
        }
    }
}