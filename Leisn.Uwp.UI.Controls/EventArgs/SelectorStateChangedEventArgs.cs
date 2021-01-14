﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leisn.Uwp.UI.Controls
{

    public class SelectorStateChangedEventArgs
    {
        public bool Selected { get; }

        public SelectorStateChangedEventArgs(bool selected)
        {
            this.Selected = selected;
        }
    }
}
