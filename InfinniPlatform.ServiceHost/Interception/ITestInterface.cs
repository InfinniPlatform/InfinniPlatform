using System.Threading.Tasks;

namespace InfinniPlatform.ServiceHost.Interception
{
    public interface ITestInterface
    {
        void DoSyncWork();
        int DoGenericSyncWork();

        Task DoTaskWork();
        Task<int> DoGenericTaskWork();

        Task DoAsyncWork();
        Task<int> DoGenericAsyncWork();
    }

    public interface IInterceptedTestInterface
    {
        void DoSyncWork();
        int DoGenericSyncWork();

        Task DoTaskWork();
        Task<int> DoGenericTaskWork();

        Task DoAsyncWork();
        Task<int> DoGenericAsyncWork();
    }
}