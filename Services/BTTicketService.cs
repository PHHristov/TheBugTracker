using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBugTracker.Data;
using TheBugTracker.Models;
using TheBugTracker.Services.Interfaces;
using System.Linq;
using TheBugTracker.Models.Enums;

namespace TheBugTracker.Services
{
    public class BTTicketService : IBTTicketService
    {
        private readonly ApplicationDbContext _context;
        private readonly IBTRolesService _roleService;
        private readonly IBTProjectService _projectService;
        public BTTicketService(ApplicationDbContext context, IBTRolesService roleService, IBTProjectService projectService)
        {
            _context = context;
            _roleService = roleService;
            _projectService = projectService;
        }
        public async Task AddNewTicketAsync(Ticket ticket)
        {
            _context.Add(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task ArchiveTicketAsync(Ticket ticket)
        {
            ticket.Archived = true;
            _context.Update(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task AssignTicketAsync(int ticketId, string userId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<Ticket>> GetAllTicketsByCompanyAsync(int companyId)
        {
            try
            {
                List<Ticket> tickets = await _context.Projects
                                                     .Where(p => p.CompanyId == companyId)        
                                                     .SelectMany(p => p.Tickets)
                                                         .Include(t => t.Attachments)
                                                         .Include(t => t.Comments)
                                                         .Include(t => t.DeveloperUser)
                                                         .Include(t => t.History)
                                                         .Include(t => t.OwnerUser)
                                                         .Include(t => t.TicketPriority)
                                                         .Include(t => t.TicketStatus)
                                                         .Include(t => t.TicketType)
                                                         .Include(t => t.Project)
                                                     .ToListAsync();
                return tickets;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Ticket>> GetAllTicketsByPriorityAsync(int companyId, string priorityName)
        {
            int priorityId = (await LookupTicketPriorityIdAsync(priorityName)).Value;

            try
            {
                List<Ticket> tickets = await _context.Projects
                                                     .Where(p => p.CompanyId == companyId)
                                                     .SelectMany(p => p.Tickets)
                                                         .Include(t => t.Attachments)
                                                         .Include(t => t.Comments)
                                                         .Include(t => t.DeveloperUser)
                                                         .Include(t => t.History)
                                                         .Include(t => t.OwnerUser)
                                                         .Include(t => t.TicketPriority)
                                                         .Include(t => t.TicketStatus)
                                                         .Include(t => t.TicketType)
                                                         .Include(t => t.Project)
                                                     .Where(t => t.TicketPriorityId == priorityId)
                                                     .ToListAsync();
                return tickets;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Ticket>> GetAllTicketsByStatusAsync(int companyId, string statusName)
        {
            int statusId = (await LookupTicketStatusIdAsync(statusName)).Value;
            try
            {
                List<Ticket> tickets = await _context.Projects
                                                     .Where(p => p.CompanyId == companyId)
                                                     .SelectMany(p => p.Tickets)
                                                         .Include(t => t.Attachments)
                                                         .Include(t => t.Comments)
                                                         .Include(t => t.DeveloperUser)
                                                         .Include(t => t.History)
                                                         .Include(t => t.OwnerUser)
                                                         .Include(t => t.TicketPriority)
                                                         .Include(t => t.TicketStatus)
                                                         .Include(t => t.TicketType)
                                                         .Include(t => t.Project)
                                                     .Where(t => t.TicketStatusId == statusId)
                                                     .ToListAsync();
                return tickets;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Ticket>> GetAllTicketsByTypeAsync(int companyId, string typeName)
        {
            int typeId = (await LookupTicketTypeIdAsync(typeName)).Value;

            try
            {
                List<Ticket> tickets = await _context.Projects
                                                     .Where(p => p.CompanyId == companyId)
                                                     .SelectMany(p => p.Tickets)
                                                         .Include(t => t.Attachments)
                                                         .Include(t => t.Comments)
                                                         .Include(t => t.DeveloperUser)
                                                         .Include(t => t.History)
                                                         .Include(t => t.OwnerUser)
                                                         .Include(t => t.TicketPriority)
                                                         .Include(t => t.TicketStatus)
                                                         .Include(t => t.TicketType)
                                                         .Include(t => t.Project)
                                                     .Where(t => t.TicketTypeId == typeId)
                                                     .ToListAsync();
                return tickets;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Ticket>> GetArchivedTicketsAsync(int companyId)
        {
            try
            {
                List<Ticket> tickets = (await GetAllTicketsByCompanyAsync(companyId)).Where(t => t.Archived == true).ToList();
                return tickets;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Ticket>> GetProjectTicketsByPriorityAsync(string priorityName, int companyId, int projectId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<Ticket>> GetProjectTicketsByRoleAsync(string role, string userId, int projectId, int companyId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<Ticket>> GetProjectTicketsByStatusAsync(string statusName, int companyId, int projectId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<Ticket>> GetProjectTicketsByTypeAsync(string typeName, int companyId, int projectId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Ticket> GetTicketByIdAsync(int ticketId)
        {
            return await _context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);
            
        }

        public async Task<BTUser> GetTicketDeveloperAsync(int ticketId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<Ticket>> GetTicketsByRoleAsync(string role, string userId, int companyId)
        {
            List<Ticket> tickets = new();

            try
            {
                if(role == Roles.Admin.ToString())
                {
                    tickets = await GetAllTicketsByCompanyAsync(companyId);
                }
                else if(role == Roles.Developer.ToString())
                {
                    tickets = (await GetAllTicketsByCompanyAsync(companyId)).Where( t => t.DeveloperUserId == userId).ToList();
                }
                else if(role == Roles.Submitter.ToString())
                {
                    tickets = (await GetAllTicketsByCompanyAsync(companyId)).Where(t => t.OwnerUserId == userId).ToList();
                }
                else if(role == Roles.ProjectManager.ToString())
                {
                    tickets = (await GetTicketsByUserIdAsync(userId, companyId));
                }

                return tickets;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Ticket>> GetTicketsByUserIdAsync(string userId, int companyId)
        {
            BTUser btuser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            List<Ticket> tickets = new();
            try
            {
                if (await _roleService.IsUserInRoleAsync(btuser, Roles.Admin.ToString()))
                {
                    tickets = (await _projectService.GetAllProjectsByCompany(companyId)).SelectMany(p => p.Tickets).ToList();
                }
                else if (await _roleService.IsUserInRoleAsync(btuser, Roles.Developer.ToString()))
                {
                    tickets  = (await _projectService.GetAllProjectsByCompany(companyId))
                                                     .SelectMany(p => p.Tickets)
                                                     .Where(t => t.DeveloperUserId == userId)
                                                     .ToList();
                }
                else if (await _roleService.IsUserInRoleAsync(btuser, Roles.Submitter.ToString()))
                {
                    tickets = (await _projectService.GetAllProjectsByCompany(companyId))
                                                     .SelectMany(p => p.Tickets)
                                                     .Where(t => t.OwnerUserId == userId)
                                                     .ToList();
                }
                else if (await _roleService.IsUserInRoleAsync(btuser, Roles.ProjectManager.ToString()))
                {
                    tickets = (await _projectService.GetUserProjectsAsync(userId)).SelectMany(t => t.Tickets).ToList();
                }

                return tickets;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int?> LookupTicketPriorityIdAsync(string priorityName)
        {
            try
            {
                TicketPriority priority = await _context.TicketPriorities.FirstOrDefaultAsync(p => p.Name == priorityName);
                return priority?.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int?> LookupTicketStatusIdAsync(string statusName)
        {
            try
            {
                TicketStatus status = await _context.TicketStatuses.FirstOrDefaultAsync(s => s.Name == statusName);
                return status?.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int?> LookupTicketTypeIdAsync(string typeName)
        {
            try
            {
                TicketType type = await _context.TicketTypes.FirstOrDefaultAsync(s => s.Name == typeName);
                return type?.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateTicketAsync(Ticket ticket)
        {
            _context.Update(ticket);
            await _context.SaveChangesAsync();
        }
    }
}
