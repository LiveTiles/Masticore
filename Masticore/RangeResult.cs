using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masticore
{
    /// <summary>
    /// Result object that is the result of an asynchronous paged query.
    /// There may be multiple ranges within a given set of data.
    /// </summary>
    /// <typeparam name="ModelType"></typeparam>
    public class RangeResult<ModelType>
    {
        public IEnumerable<ModelType> Values;

        public int StartsAt;

        public int EndsAt;

        public int Total;
    }
}
