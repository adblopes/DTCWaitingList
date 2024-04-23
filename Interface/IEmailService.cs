﻿using DTCWaitingList.Models;
using Google.Apis.Gmail.v1.Data;

namespace DTCWaitingList.Interface
{
    //since there's no API to call, appointments are read from templated gmail messages
    public interface IEmailService
    {
        //send Email reply
        Task SendEmailAsync(string userEmail, string userName);

        //read and process appointment email, returning a new Patient object 
        Patient ReadEmail(string messageId);

        //read, reply and trash unread gmail messages
        Task ProcessInboxUnreadAsync();

        //list all gmail messages
        List<Message> ListMessages(string userId, string query);
    }
}
