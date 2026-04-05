using CommunityToolkit.Mvvm.Messaging.Messages;
using Sakaishi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakaishi.Messages
{
    public class LargeCategoryInvokedMessage : ValueChangedMessage<LargeCategory>
    {
        public LargeCategoryInvokedMessage(LargeCategory value) : base(value) { }
    }
}
