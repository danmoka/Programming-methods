using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace searchAlgorithmsOfSubstring
{
    public interface IStringSearcher
    {
        IEnumerable<int> Find();
    }
}
