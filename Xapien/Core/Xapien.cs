using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xapien.Core
{
    public class Xapien
    {
        public void Run(ref CancellationToken cancelationToken) {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            cancelationToken = token;
            

        }
    }
}
