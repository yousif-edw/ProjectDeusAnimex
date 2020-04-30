using System;
using System.Collections.Generic;
using MessageBusLib;

namespace Assets.Ancible_Tools.Scripts.Traits
{
    public class AddTraitToUnitMessage : EventMessage
    {
        public Trait Trait { get; set; }
    }

    public class RemoveTraitFromUnitMessage : EventMessage
    {
        public Trait Trait { get; set; }
    }

    public class RemoveTraitFromUnitByControllerMessage : EventMessage
    {
        public TraitController Controller { get; set; }
    }

    public class TraitCheckMessage : EventMessage
    {
        public List<Trait> TraitsToCheck { get; set; }
        public Action DoAfter { get; set; }
    }
}