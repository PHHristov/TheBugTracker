using System.Collections.Generic;
using System.Threading.Tasks;
using TheBugTracker.Data;
using TheBugTracker.Models;
using TheBugTracker.Services.Interfaces;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TheBugTracker.Services
{
    public class BTTicketHistoryService : IBTTicketHistoryService
    {
        private readonly ApplicationDbContext _context;
        public BTTicketHistoryService(ApplicationDbContext context)
        {
            this._context = context;

        }
        public async Task AddHistoryAsync(Ticket oldTicket, Ticket newTicket, string userId)
        {
            if (oldTicket == null && newTicket != null)
            {
                TicketHistory history = new()
                {
                    TicketId = newTicket.Id,
                    Property = "",
                    OldValue = "",
                    NewValue = "",
                    Created = System.DateTimeOffset.Now,
                    UserId = userId,
                    Description = "New Ticket Created"
                };

                try
                {
                    await _context.TicketHistories.AddAsync(history);
                    await _context.SaveChangesAsync();
                }
                catch (System.Exception)
                {

                    throw;
                }
            }
            else
            {
                //NEW Title
                if (oldTicket.Title != newTicket.Title)
                {
                    TicketHistory history = new()
                    {
                        TicketId = newTicket.Id,
                        Property = "Title",
                        OldValue = oldTicket.Title,
                        NewValue = newTicket.Title,
                        Created = System.DateTimeOffset.Now,
                        UserId = userId,
                        Description = $"New Ticket Title: {newTicket.Title}"
                    };
                    await _context.TicketHistories.AddAsync(history);
                }
                
                //NEW Description
                if (oldTicket.Description != newTicket.Description)
                {
                    TicketHistory history = new()
                    {
                        TicketId = newTicket.Id,
                        Property = "Description",
                        OldValue = oldTicket.Description,
                        NewValue = newTicket.Description,
                        Created = System.DateTimeOffset.Now,
                        UserId = userId,
                        Description = $"New Ticket Description: {newTicket.Description}"
                    };
                    await _context.TicketHistories.AddAsync(history);
                }

                //NEW Priority
                if (oldTicket.TicketPriority != newTicket.TicketPriority)
                {
                    TicketHistory history = new()
                    {
                        TicketId = newTicket.Id,
                        Property = "TicketPriority",
                        OldValue = oldTicket.TicketPriority.Name,
                        NewValue = newTicket.TicketPriority.Name,
                        Created = System.DateTimeOffset.Now,
                        UserId = userId,
                        Description = $"New Ticket Priority: {newTicket.TicketPriority.Name}"
                    };
                    await _context.TicketHistories.AddAsync(history);
                }

                //NEW Status
                if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
                {
                    TicketHistory history = new()
                    {
                        TicketId = newTicket.Id,
                        Property = "TicketStatus",
                        OldValue = oldTicket.TicketStatus.Name,
                        NewValue = newTicket.TicketStatus.Name,
                        Created = System.DateTimeOffset.Now,
                        UserId = userId,
                        Description = $"New Ticket Status: {newTicket.TicketStatus.Name}"
                    };
                    await _context.TicketHistories.AddAsync(history);
                }

                //NEW Type
                if (oldTicket.TicketTypeId != newTicket.TicketTypeId)
                {
                    TicketHistory history = new()
                    {
                        TicketId = newTicket.Id,
                        Property = "TicketTypeId",
                        OldValue = oldTicket.TicketType.Name,
                        NewValue = newTicket.TicketType.Name,
                        Created = System.DateTimeOffset.Now,
                        UserId = userId,
                        Description = $"New Ticket Type: {newTicket.TicketType.Name}"
                    };
                    await _context.TicketHistories.AddAsync(history);
                }

                //NEW Developer
                if (oldTicket.DeveloperUserId != newTicket.DeveloperUserId)
                {
                    TicketHistory history = new()
                    {
                        TicketId = newTicket.Id,
                        Property = "Developer",
                        OldValue = oldTicket.DeveloperUser?.FullName ?? "Not Assigned",
                        NewValue = newTicket.DeveloperUser?.FullName,
                        Created = System.DateTimeOffset.Now,
                        UserId = userId,
                        Description = $"New Ticket Developer: {newTicket.DeveloperUser.FullName}"
                    };
                    await _context.TicketHistories.AddAsync(history);
                }

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (System.Exception)
                {

                    throw;
                }
                
            }
        }

        public async Task<List<TicketHistory>> GetCompanyTicketsHistories(int companyId)
        {
            try
            {
                List<Project> projects = (await _context.Companies
                                                       .Include(c => c.Projects)
                                                           .ThenInclude(p => p.Tickets)
                                                               .ThenInclude(t => t.History)
                                                                   .ThenInclude(h => h.User)
                                                       .FirstOrDefaultAsync(c => c.Id == companyId)).Projects.ToList();
                List<Ticket> tickets = projects.SelectMany(p => p.Tickets).ToList();

                List<TicketHistory> ticketsHistories = tickets.SelectMany(t => t.History).ToList();
             
                return ticketsHistories;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<TicketHistory>> GetProjectTicketsHistoriesAsync(int projectId, int companyId)
        {
            try
            {
                Project project = await _context.Projects.Where(p => p.CompanyId == companyId)
                                                         .Include(p => p.Tickets)
                                                             .ThenInclude(t => t.History)
                                                                 .ThenInclude(h => h.User)
                                                         .FirstOrDefaultAsync(p => p.Id == projectId);

                List<TicketHistory> history = project.Tickets.SelectMany(t => t.History).ToList();

                return history;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
    
}
