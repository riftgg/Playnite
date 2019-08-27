using Playnite.SDK;
using System.Collections.ObjectModel;
using System.Linq;

namespace PlayniteCore
{
    public class NotificationsAPI : INotificationsAPI
    {
        public ObservableCollection<NotificationMessage> Messages { get; } = new ObservableCollection<NotificationMessage>();

        public int Count => Messages.Count;

        public void Add(NotificationMessage message)
        {
            Messages.Add(message);
        }

        public void Add(string id, string text, NotificationType type)
        {
            Messages.Add(new NotificationMessage(id, text, type));
        }

        public void Remove(string id)
        {
            var message = Messages.SingleOrDefault(m => m.Id == id);
            if (message != null)
            {
                Messages.Remove(message);
            }
        }

        public void RemoveAll()
        {
            Messages.Clear();
        }
    }
}
