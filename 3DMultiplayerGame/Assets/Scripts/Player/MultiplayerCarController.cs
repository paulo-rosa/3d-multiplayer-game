using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Player
{
    public class MultiplayerCarController: MyCarController
    {
        protected override void Update()
        {
            if (!isLocalPlayer)
            {
                return;
            }

            Accelerate();
            Turn();
            Jump();
        }

    }
}
