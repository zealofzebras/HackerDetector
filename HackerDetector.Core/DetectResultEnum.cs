using System;
using System.Collections.Generic;
using System.Text;

namespace HackerDetector
{
    [Flags]
    public enum DetectResultEnum
    {
        Clean = 0,
        Blocked = 1,
        FellInTrap = 2,
        FellInTrapAndBlocked = Blocked + FellInTrap,
        HammeredPathAndBlocked = Blocked + (FellInTrap^2)
    }
}
