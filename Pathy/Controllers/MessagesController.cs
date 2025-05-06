using Microsoft.AspNetCore.Mvc;
using Pathy.Models;
using Pathy_BusinessAccessLayer;

namespace Pathy.Controllers
{
    public class MessagesController : Controller
    {
        [HttpGet]
        public IActionResult MessagesInbox()
        {
            clsMessagesViewModel model = new clsMessagesViewModel();
            model.loadMessages();

            return View(model);
        }


        [HttpPost]
        public IActionResult OpenChatWithUser(clsMessagesViewModel model)
        {            
            clsChatViewModel chatViewModel = new clsChatViewModel();

            chatViewModel.userToChatWithID = model.userToChatWithID;

            chatViewModel.loadUserToChatWith();

            chatViewModel.userToChatWith.loadUserCompositeObjects();
            chatViewModel.loadUserToChatWithAccount();

            chatViewModel.loadSentMessages();
            chatViewModel.loadRecievedMessages();

            return View("Chat",chatViewModel);
        }

        [HttpPost]
        public IActionResult SendMessageTo(clsChatViewModel model)
        {
  
            model.loadUserToChatWithAccount();
         

            clsMessage message = new clsMessage();

            message.mode = clsMessage.enMode.AddNew;
            message.senderAccountID = clsGlobal.account.accountID;
            message.recieverAccountID = model.userToChatWithAccount.accountID;
            message.message = model.message;

            message.save();

            clsChatViewModel chatViewModel = new clsChatViewModel();

            chatViewModel.userToChatWithID = model.userToChatWithID;

            chatViewModel.loadUserToChatWith();

            chatViewModel.userToChatWith.loadUserCompositeObjects();
            chatViewModel.loadUserToChatWithAccount();

            chatViewModel.loadSentMessages();
            chatViewModel.loadRecievedMessages();


            return View("Chat",chatViewModel);
        }


        [HttpPost]
        public IActionResult DeleteMessage(clsChatViewModel model)
        {

            clsMessage message = clsMessage.getMessageByMessageID(model.messageID);
            message.mode = clsMessage.enMode.Delete;
            message.save();

            clsChatViewModel chatViewModel = new clsChatViewModel();

            chatViewModel.userToChatWithID = model.userToChatWithID;

            chatViewModel.loadUserToChatWith();

            chatViewModel.userToChatWith.loadUserCompositeObjects();
            chatViewModel.loadUserToChatWithAccount();

            chatViewModel.loadSentMessages();
            chatViewModel.loadRecievedMessages();

            return View("Chat", chatViewModel);
        }


        [HttpPost]
        public IActionResult OpenEditMessage(clsChatViewModel model)
        {           
            clsEditMessageViewModel editMessageViewModel = new clsEditMessageViewModel();
            editMessageViewModel.message = model.message;
            editMessageViewModel.messageID = model.messageID;
            editMessageViewModel.userToChatWithID = model.userToChatWithID;

            return View("EditMessage", editMessageViewModel);
        }

        [HttpPost]
        public IActionResult EditMessage(clsEditMessageViewModel model)
        {
  

            clsMessage message = clsMessage.getMessageByMessageID(model.messageID);
            message.mode = clsMessage.enMode.Update;
            message.message = model.message;
            message.save();

            clsChatViewModel chatViewModel = new clsChatViewModel();

            chatViewModel.userToChatWithID = model.userToChatWithID;
           

            chatViewModel.loadUserToChatWith();

            chatViewModel.userToChatWith.loadUserCompositeObjects();
            chatViewModel.loadUserToChatWithAccount();

            chatViewModel.loadSentMessages();
            chatViewModel.loadRecievedMessages();


            return View("Chat", chatViewModel);
        }


    }
}
