using InquiryAPI.Configuration;
using InquiryAPI.DomainObjects.Inquiries;
using InquiryAPI.Services.InquiriesService;
using InquiryStatusUpdater.SignalRHubs;
using Microsoft.AspNetCore.SignalR;

namespace InquiryStatusUpdater
{
    public class Worker : BackgroundService
    {
        private readonly int _statusSetterWorkerDelayInMs;
        private readonly int _statusChangeDelayInS;
        private readonly string _notificationTopic;
        private readonly IServiceProvider _serviceProvider;


        public Worker(AppConfiguration appConfiguration, IServiceProvider serviceProvider)
        {
            _statusSetterWorkerDelayInMs = appConfiguration.StatusSetterWorkerDelayInMs;
            _statusChangeDelayInS = appConfiguration.StatusChangeDelayInS;
            _notificationTopic = appConfiguration.SignalRConfiguration.InquiryListRefreshTopic;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                IInquiriesService inquiriesService = scope.ServiceProvider.GetRequiredService<IInquiriesService>();
                IHubContext<NotificationHub> notificationHubContext = scope.ServiceProvider.GetRequiredService<IHubContext<NotificationHub>>();
                while (!stoppingToken.IsCancellationRequested)
                {
                    IEnumerable<Inquiry> issuedInquiries = inquiriesService
                        .Search(new InquirySearchPredicate()
                        {
                            InquiryStatus = InquiryStatus.Issued
                        })
                        .OrderByDescending(q => q.CreationDate);

                    if (issuedInquiries.Any())
                    {
                        Inquiry inquiry = issuedInquiries.First();
                        if ((DateTimeOffset.UtcNow - inquiry.CreationDate).Seconds >= _statusChangeDelayInS)
                        {
                            inquiry.Status = InquiryStatus.Issued;
                            inquiriesService.Save(inquiry);

                            await notificationHubContext.Clients
                                .User(inquiry.UserId.ToString())
                                .SendAsync(_notificationTopic, inquiry.Id);
                        }
                    }

                    await Task.Delay(_statusSetterWorkerDelayInMs, stoppingToken);
                }
            }
        }
    }
}