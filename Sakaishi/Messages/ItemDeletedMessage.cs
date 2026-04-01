using CommunityToolkit.Mvvm.Messaging.Messages;
using Sakaishi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakaishi.Messages
{
    public class ItemDeletedMessage : ValueChangedMessage<Item>
    {
        public ItemDeletedMessage(Item item)
            : base(item) { }
    }
}
