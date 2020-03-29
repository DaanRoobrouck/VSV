using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI
{
   public class DestinationEventArgs
   {
       public Transform Destination;

       public DestinationEventArgs(Transform destination)
       {
           Destination = destination;
       }
   }
}
