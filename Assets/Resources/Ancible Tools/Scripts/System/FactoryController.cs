using Assets.Ancible_Tools.Scripts.Traits;
using Assets.Resources.Scripts.Game_System.Controllers;
using UnityEngine;

namespace Assets.Ancible_Tools.Scripts.System
{
    public class FactoryController : MonoBehaviour
    {
        public static UnitController UNIT_CONTROLLER { get; private set; }
        public static TraitController TRAIT_CONTROLLER { get; private set; }
        public static UnitAnimationController UNIT_ANIMATION_CONTROLLER { get; private set; }

        private static FactoryController _instance;

        [Header("Unit Templates")]
        public UnitController UnitControllerTemplate;
        public TraitController TraitControllerTemplate;
        public UnitAnimationController UnitAnimationControllerTemplate;

        void Awake()
        {
            if (_instance)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            SetupStatics();
        }

        protected internal virtual void SetupStatics()
        {
            UNIT_CONTROLLER = UnitControllerTemplate;
            TRAIT_CONTROLLER = TraitControllerTemplate;
            UNIT_ANIMATION_CONTROLLER = UnitAnimationControllerTemplate;
        }
    }
}