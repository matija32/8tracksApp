using EightTracksPlayer.Communication.Requests;
using EightTracksPlayer.Communication.Responses;

namespace EightTracksPlayer.Communication
{
    public interface IRequestExecutor
    {
        IResponse Execute(IRequest request);
    }
}