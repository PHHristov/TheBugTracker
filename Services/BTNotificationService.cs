using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBugTracker.Data;
using TheBugTracker.Models;
using TheBugTracker.Services.Interfaces;

namespace TheBugTracker.Services
{
    public class BTNotificationService : IBTNotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IBTRolesService _rolesService;
        public BTNotificationService(ApplicationDbContext context,
                              IEmailSender emailSender,
                              IBTRolesService rolesService)
        {
            _context = context;
            _emailSender = emailSender;
            _rolesService = rolesService;
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            try
            {
                await _context.AddAsync(notification);
                await _context.SaveChangesAsync();
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public async Task<List<Notification>> GetReceivedNotificationsAsync(string userId)
        {
            try
            {
                List<Notification> notifications = await _context.Notifications
                                                                 .Include(n => n.Recipient)
                                                                 .Include(n => n.Sender)
                                                                 .Include(n => n.Ticket)
                                                                     .ThenInclude(t => t.Project)
                                                                 .Where(n => n.RecipientId == userId).ToListAsync();
                return notifications;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public async Task<List<Notification>> GetSentNotificationsAsync(string userId)
        {
            try
            {
                List<Notification> notifications = await _context.Notifications
                                                                 .Include(n => n.Recipient)
                                                                 .Include(n => n.Sender)
                                                                 .Include(n => n.Ticket)
                                                                     .ThenInclude(t => t.Project)
                                                                 .Where(n => n.SenderId == userId).ToListAsync();
                return notifications;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public async Task <bool> SendEmailNotificationAsync(Notification notification, string emailSubject)
        {
            BTUser btUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == notification.RecipientId);

            if (btUser != null)
            {
                string btUserEmail = btUser.Email;
                string message = notification.Message;

                //Send Email
                try
                {
                    await _emailSender.SendEmailAsync(btUserEmail, emailSubject, message);
                    return true;
                }
                catch (System.Exception)
                {

                    throw;
                }
            }
            else
            {
                return false;
            }
        }

        public async Task SendEmailNotificationsByRoleAsync(Notification notification, int companyId, string role)
        {
            try
            {
                List<BTUser> members = await _rolesService.GetUsersInRoleAsync(role, companyId);
                
                foreach (BTUser member in members)
                {
                    notification.RecipientId = member.Id;
                    await SendEmailNotificationAsync(notification, notification.Title);
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public async Task SendMemebersEmailNotificationsAsync(Notification notification, List<BTUser> members)
        {
            foreach(BTUser member in members)
            {
                try
                {
                    notification.RecipientId = member.Id;
                    await SendEmailNotificationAsync(notification, notification.Title);
                }
                catch (System.Exception)
                {

                    throw;
                }
            }
        }
    }
}
