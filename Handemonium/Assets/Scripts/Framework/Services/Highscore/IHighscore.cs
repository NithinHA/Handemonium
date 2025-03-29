namespace RPSLS.Framework.Services
{
    public interface IHighscore : IService
    {
        int GetHighscore();
        
        void SetHighscore(int value);
    }
}