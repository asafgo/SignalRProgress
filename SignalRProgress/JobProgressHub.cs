using Microsoft.AspNetCore.SignalR;

namespace SignalRProgress
{
    public class JobProgressHub : Hub
    {
        public async Task AssociateJob(string jobId)
        {
            if (jobId != null) await Groups.AddToGroupAsync(Context.ConnectionId, jobId).ConfigureAwait(false);
        }
    }
}
