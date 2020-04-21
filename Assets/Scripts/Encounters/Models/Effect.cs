using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Encounter;

namespace Models { 
    public abstract class Effect {
        public enum Frequency {
            Immediate,
            EndOfTurn,
            StartOfTurn
        }
        public virtual int Duration { get; set; }
        public virtual Frequency PointOfAction { get; set; }
        public abstract void Execute(Effectable actor);
        public abstract void Remove(Effectable actor);
    }

    
}
