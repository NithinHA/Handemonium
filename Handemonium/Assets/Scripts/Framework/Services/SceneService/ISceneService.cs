using System;

namespace RPSLS.Framework.Services
{
    public interface ISceneService : IService
    {
        void LoadScene(string sceneName, Action onComplete = null);
    }
}