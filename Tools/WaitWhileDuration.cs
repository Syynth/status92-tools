using System;
using UnityEngine;

namespace Status92.Tools
{
    public class WaitWhileDuration : CustomYieldInstruction
    {
        protected Action<float, float> Action;
        protected float Duration { get; set; }
        protected float StartTime { get; set; }
        
        protected float ElapsedPercentage { get; set; }
        protected float DeltaPercentage { get; set; }
        
        public WaitWhileDuration(float duration, Action<float, float> action) =>
            (StartTime, Duration, Action) = (Time.time, duration, action);

        public override bool keepWaiting
        {
            get
            {
                DeltaPercentage = (Time.time - StartTime) / Duration - ElapsedPercentage;
                ElapsedPercentage += DeltaPercentage;
                
                if (Time.time < StartTime + Duration)
                {
                    Action(ElapsedPercentage, DeltaPercentage);
                    return true;
                }
                
                return false;
            }
        }
    }
}