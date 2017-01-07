using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using System.Net.Http.Headers;
using FakeIdentityBot.Model;
using FakeIdentityBot.IdentityClient;

namespace FakeIdentityBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                Activity reply;

                if (string.IsNullOrEmpty(activity.Text))
                {
                    reply = activity.CreateReply($"You send an empty message. Were you trying to ask me for something?");
                }
                else if (activity.Text.ToUpper().Contains("HI") || activity.Text.ToUpper().Contains("HELLO"))
                {
                    reply = activity.CreateReply("Nice to see you ^^");
                }
                else {
                    var client = new FakeIdentityClient();
                    FakeIdentity fakeIdentity = await client.GetFakeIdentity(); 
                    if (fakeIdentity != null)
                    {
                        reply = activity.CreateReply($"Your fake identity is {fakeIdentity.name} {fakeIdentity.surname}, a {fakeIdentity.gender} from {fakeIdentity.region}");
                    }
                    else
                    {
                        reply = activity.CreateReply("Sorry, I could not create a fake identity this time :(. It is so shameful...");
                    }
                }

                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}