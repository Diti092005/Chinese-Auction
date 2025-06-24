using Microsoft.Extensions.Logging;
using server.Bll.Interfaces;
using server.Dal.Interfaces;
using server.Models;
using server.Models.DTO;

namespace server.Bll
{
    public class GiftService : IGiftService
    {
        private readonly IGiftDal _giftDal;
        private readonly ITicketDal _ticketDal;
        private readonly ILogger<GiftService> _logger;

        public GiftService(IGiftDal giftRepository, ITicketDal ticketDal, ILogger<GiftService> logger)
        {
            _giftDal = giftRepository;
            _ticketDal = ticketDal;
            _logger = logger;
        }
        public async Task<IEnumerable<Gift>> Get()
        {
            _logger.LogInformation("Getting all gifts");
            try
            {
                return await _giftDal.Get();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all gifts");
                throw;
            }
        }
        public async Task<Gift> Get(int id)
        {
            _logger.LogInformation($"Getting gift with id {id}");
            try
            {
                return await _giftDal.Get(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting gift with id {id}");
                throw;
            }
        }
        public async Task Add(Gift gift)
        {
            _logger.LogInformation($"Adding gift: {gift.Title}");
            try
            {
                await _giftDal.Add(gift);
                _logger.LogInformation($"Gift {gift.Title} added successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding gift: {gift.Title}");
                throw;
            }
        }
        public async Task Update(int id, GiftDTO gift)
        {
            _logger.LogInformation($"Updating gift with id {id}");
            try
            {
                await _giftDal.Update(id, gift);
                _logger.LogInformation($"Gift with id {id} updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating gift with id {id}");
                throw;
            }
        }
        public async Task<bool> Delete(int id)
        {
            _logger.LogInformation($"Deleting gift with id {id}");
            try
            {
                var result = await _giftDal.Delete(id);
                _logger.LogInformation($"Gift with id {id} deleted successfully");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting gift with id {id}");
                throw;
            }
        }
        public async Task<IEnumerable<Gift>> Search(string giftName = null, string donorName = null, int? buyerCount = null)
        {
            _logger.LogInformation($"Searching gifts by giftName: {giftName}, donorName: {donorName}, buyerCount: {buyerCount}");
            try
            {
                return await _giftDal.Search(giftName, donorName, buyerCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching gifts");
                throw;
            }
        }
        public async Task<Donor> GetDonor(int giftId)
        {
            _logger.LogInformation($"Getting donor for gift id {giftId}");
            try
            {
                return await _giftDal.GetDonor(giftId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting donor for gift id {giftId}");
                throw;
            }
        }
        public async Task<bool> TitleExists(string title)
        {
            _logger.LogInformation($"Checking if gift title exists: {title}");
            try
            {
                return await _giftDal.TitleExists(title);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking if gift title exists: {title}");
                throw;
            }
        }
        public async Task<IEnumerable<Gift>> SortByPrice()
        {
            _logger.LogInformation("Sorting gifts by price");
            try
            {
                return await _giftDal.SortByPrice();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sorting gifts by price");
                throw;
            }
        }
        public async Task<IEnumerable<Gift>> SortByCategory()
        {
            _logger.LogInformation("Sorting gifts by category");
            try
            {
                return await _giftDal.SortByCategory();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sorting gifts by category");
                throw;
            }
        }

        public async Task raffle(int id)
        {
            var gift = await _giftDal.Get(id);
            if (gift == null)
            {
                throw new InvalidOperationException("Gift not found");
            }
            if (gift.Winner != null)
            {
                throw new InvalidOperationException("Gift already won");
            }
            var numOfTickets = gift.Tickets.Count;
            if (numOfTickets == 0)
            {
                throw new InvalidOperationException("No tickets sold");
            }
            var random = new Random();
            var winnerIndex = random.Next(0, numOfTickets);
            var winnerTicket = gift.Tickets[winnerIndex];

            await _ticketDal.Win(winnerTicket.Id);
            await _giftDal.UpdateWinnerId(id, winnerTicket.UserId);
        }

    }
}
