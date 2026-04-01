using CommunityToolkit.Mvvm.Messaging.Messages;
using Sakaishi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakaishi.Messages
{
    public class SmallCategoryAddedMessage : ValueChangedMessage<SmallCategory>
    {
        public SmallCategoryAddedMessage(SmallCategory category)
            : base(category) { }
    }
}
