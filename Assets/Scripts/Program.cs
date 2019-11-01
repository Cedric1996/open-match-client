// Copyright 2015 gRPC authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using OpenMatch;
using UnityEngine;

namespace Program
{
    public class OpenMatchClient
    {
        readonly Frontend.FrontendClient client;

        public OpenMatchClient()
        {

        }

        public OpenMatchClient(Frontend.FrontendClient client)
        {
            this.client = client;
        }

        public Ticket CreateTicket(Ticket ticket)
        {
            // Log("Calling CreateTicket with player {0}", ticket.Id);
            var request = new CreateTicketRequest();
            request.Ticket = ticket;
            Debug.Log("Calling CreateTicket with player");
            Ticket m_ticket = client.CreateTicket(request).Ticket;
            Debug.Log(m_ticket);
            return m_ticket;
            // return client.CreateTicket(request);
        }

        private async Task GetAssignment(String Id)
        {
            Debug.Log("*** GetAssignments: ticket: "+Id);
            GetAssignmentsRequest request = new GetAssignmentsRequest
            {
                TicketId = Id
            };
            var responseStream = client.GetAssignments(request).ResponseStream; 
            Debug.Log(responseStream);
            await responseStream.MoveNext();

            Assignment assignment = responseStream.Current.Assignment;
            Debug.Log("Getting assignment"+assignment);

        }

        public async Task GetUpdates(Ticket ticket)
        {
            try
            {
                Debug.Log("*** GetAssignments: ticket: "+ticket.Id);
                var request = new GetAssignmentsRequest
                {
                    TicketId = ticket.Id
                };

                using (var call = client.GetAssignments(request))
                {
                    var responseStream = call.ResponseStream;
                    StringBuilder responseLog = new StringBuilder("Result: ");
                    Log("waiting");
                    await responseStream.MoveNext();

                    Log("awaiting");
                    var assignment = responseStream.Current;
                    Log(assignment.Assignment);
                    //responseLog.Append(assignment.ToString());

                    Log(responseLog.ToString());
                }
            }
            catch (RpcException e)
            {
                Log("RPC failed " + e);
                throw;
            }
        }

        private void Log(string s, params object[] args)
        {
            Console.WriteLine(string.Format(s, args));
        }

        private void Log(object s)
        {
            Debug.Log(s.ToString());
        }

        public static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            System.Random random = new System.Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        public static async void exec()
        {
          // var channel = new Channel("10.86.34.241:30888", ChannelCredentials.Insecure);
            var channel = new Channel("172.17.129.6:30593", ChannelCredentials.Insecure);
            Debug.Log(channel);
            var client = new OpenMatchClient(new Frontend.FrontendClient(channel));
            Ticket ticket = TicketUtil.GenerateTicket(RandomString(6, true));
            Ticket m_ticket = client.CreateTicket(ticket);
            Debug.Log("Ticket created: " + m_ticket.Id);
            await client.GetAssignment(m_ticket.Id);
            //client.GetUpdates(m_ticket).Wait();

            await channel.ShutdownAsync();
            Debug.Log("Press any key to exit...");
        }
    }
}
