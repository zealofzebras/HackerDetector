using System.Threading.Tasks;

namespace HackerDetector.Functions
{
    public interface IHackerBlocker
    {
        Task Block(string ip);
    }
}
