using System;
using System.Collections.Generic;
using EightTracksPlayer.Communication;
using EightTracksPlayer.Communication.Requests;
using EightTracksPlayer.Communication.Responses;

namespace EightTracksTests.Stubs
{
    public class RequestExecutorStub : IRequestExecutor
    {
        private int index = 0;
        private List<IRequest> requests = new List<IRequest>();

        public RequestExecutorStub()
        {
            Responses = new List<IResponse>();
        }

        public IResponse Execute(IRequest request)
        {
            requests.Add(request);
            return Responses[index++];
        }

        public List<IResponse> Responses { get; set; }
        public List<IRequest> Requests
        {
            get { return requests; }
        }
    }
}