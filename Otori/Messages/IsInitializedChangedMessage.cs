using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otori.Messages
{
    public class IsInitializedChangedMessage : ValueChangedMessage<bool?>
    {
        public IsInitializedChangedMessage(bool? value) : base(value) { }
    }
}
