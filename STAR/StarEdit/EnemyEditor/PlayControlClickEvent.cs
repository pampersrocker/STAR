using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarEdit.EnemyEditor
{
    public class PlayControlClickEvent
    {
        public delegate void PlayControlEventHandler(object sender, PlayControls pressedControls);

        public event PlayControlEventHandler PlayControlClicked;

        protected virtual void RaisePlayControlClicked(PlayControls clickedControl)
        {
            if (PlayControlClicked != null)
                PlayControlClicked(this, clickedControl);
        }
    }
}
