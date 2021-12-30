using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Management.HR_Libs
{
    public class StrategyOpeningDialogHost : IStrategyOpeningDialogHost
    {
        Action _action;

        public StrategyOpeningDialogHost(Action action)
        {
            this._action = action;
        }

        public void DoAlgorithm()
        {
            this._action();
        }
    }
}
