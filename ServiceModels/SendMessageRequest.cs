﻿using System;
using System.Runtime.Serialization;

namespace Alek.ChatService
{
    [DataContract]
    public class SendMessageRequest
    {
        [DataMember]
        public Guid SenderGuid { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public Guid RecipientGuid { get; set; }
    }
}