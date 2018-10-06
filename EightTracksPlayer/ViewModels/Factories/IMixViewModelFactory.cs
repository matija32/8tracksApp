using EightTracksPlayer.Domain.Entities;

namespace EightTracksPlayer.ViewModels.Factories
{
    public interface IMixViewModelFactory
    {
        MixViewModel CreateMixViewModel(Mix mix);
    }
}