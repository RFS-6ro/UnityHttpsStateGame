using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Utils.Coroutines
{
    public static class CoroutineUtils
    {
        public static IEnumerator WaitAny(CustomYieldInstruction y1, CustomYieldInstruction y2)
        {
            yield return new WaitUntil(() => IsAnyReady(new []{ y1, y2 }));
        }

        public static IEnumerator WaitAny(IEnumerable<CustomYieldInstruction> yields)
        {
            yield return new WaitUntil(() => IsAnyReady(yields));
        }

        //TODO: optimize
        private static bool IsAnyReady(IEnumerable<CustomYieldInstruction> yields)
        {
            bool isReady = false;
            var yieldInstructions = yields as CustomYieldInstruction[] ?? yields.ToArray();

            foreach (var yieldInstruction in yieldInstructions)
            {
                isReady = isReady || yieldInstruction.keepWaiting;
            }

            if (isReady)
            {
                foreach (var yieldInstruction in yieldInstructions)
                {
                    if (yieldInstruction.keepWaiting == false)
                    {
                        yieldInstruction.Reset();
                    }
                }
            }

            return isReady;
        }
    }

    public class WaitForTimeStamp : CustomYieldInstruction
    {
        private long _targetTimeStamp;

        public WaitForTimeStamp(long targetTimeStamp)
        {
            _targetTimeStamp = targetTimeStamp;
        }

        public override bool keepWaiting => GetCurrentTimeStamp < _targetTimeStamp;

        private long GetCurrentTimeStamp => new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
    }

    public class WaitForAction<T> : CustomYieldInstruction
    {
        private Action<T> _action;
        private Func<bool> _keepWaiting;
        private Func<T> _getNextArg;

        public WaitForAction(
            [NotNull] Action<T> action,
            [NotNull] Func<bool> keepWaiting,
            [NotNull] Func<T> getNextArg
        )
        {
            _action = action;
            _keepWaiting = keepWaiting;
            _getNextArg = getNextArg;
        }

        public override bool keepWaiting
        {
            get
            {
                bool complete = _keepWaiting();
                if (complete == false)
                {
                    _action(_getNextArg());
                }
                return complete;
            }
        }
    }
}
