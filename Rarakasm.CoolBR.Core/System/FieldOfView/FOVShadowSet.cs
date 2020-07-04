using System;
using System.Collections.Generic;

namespace Rarakasm.CoolBR.Core.System.FieldOfView
{
    public class FOVShadowSet
    {
        // May be optimized with sorted set?
        private readonly List<ShadowInterval> _shadows = new List<ShadowInterval>();
        private readonly Queue<ShadowInterval> _deferredQueue = new Queue<ShadowInterval>();
        private struct ShadowInterval
        {
            public double start, end;

            public ShadowInterval(double start, double end)
            {
                this.start = start;
                this.end = end;
                if (end < start) throw new Exception();
            }

            public double ContainRatio(ShadowInterval other)
            {
                if (start >= other.end || end <= other.start) return 0;
                var l = Math.Max(start, other.start);
                var r = Math.Min(end, other.end);
                return (r - l) / (other.end - other.start);
            }
            
            public static ShadowInterval FromProjection(double row, double col)
            {
                return new ShadowInterval(
                    col / (row + 1),
                    (col + 1) / (row + 1)
                );
            }
        }

        public void Add(int row, int col)
        {
            Add(ShadowInterval.FromProjection(row, col));
        }
        
        private void Add(ShadowInterval shadow)
        {
            var idx = 0;
            while (idx < _shadows.Count
                   && _shadows[idx].start < shadow.start)
            {
                idx++;
            }

            var overlappingPrev = idx > 0
                                  && _shadows[idx - 1].end - shadow.start >= 0;
            var overlappingNext = idx < _shadows.Count
                                  && shadow.end - _shadows[idx].start >= 0;

            if (overlappingNext)
            {
                if (overlappingPrev)
                {
                    // Merge shadow with prev and next
                    _shadows[idx - 1] = new ShadowInterval(
                        _shadows[idx - 1].start, _shadows[idx].end);
                    _shadows.RemoveAt(idx);
                }
                else
                {
                    // Merge shadow with next
                    _shadows[idx] = new ShadowInterval(
                        shadow.start, _shadows[idx].end);
                }
            }
            else
            {
                if (overlappingPrev)
                {
                    // Merge shadow with prev
                    _shadows[idx - 1] = new ShadowInterval(
                        _shadows[idx - 1].start, shadow.end);
                }
                else
                {
                    // Insert new shadow
                    _shadows.Insert(idx, shadow);
                }
            }
        }

        public void AddDeferred(int row, int col)
        {
            _deferredQueue.Enqueue(ShadowInterval.FromProjection(row, col));
        }

        public void FlushDeferred()
        {
            while (_deferredQueue.Count > 0)
            {
                Add(_deferredQueue.Dequeue());
            }
        }

        public bool IsInShadow(int row, int col, double ratio, out double outResult)
        {
            var shadow = ShadowInterval.FromProjection(row, col);
            outResult = .0;
            foreach (var s in _shadows)
            {
                outResult += s.ContainRatio(shadow);
            }

            return outResult >= ratio;
        }

        public bool IsFullShadow()
        {
            return _shadows.Count == 1 && Math.Abs(_shadows[0].start) < .0001 && Math.Abs(_shadows[0].end - 1) < .0001;
        }
    }
}