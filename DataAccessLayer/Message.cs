//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Alek.ChatService
{
    using System;
    using System.Collections.Generic;
    
    public partial class Message
    {
        public int MessageID { get; set; }
        public string Message1 { get; set; }
        public int SenderUserID { get; set; }
        public System.DateTime SentTime { get; set; }
        public int ConversationID { get; set; }
    
        public virtual Conversation Conversation { get; set; }
        public virtual User User { get; set; }
    }
}
