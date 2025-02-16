using EventHorizon.Application.UseCases.Interfaces.Dev;
using EventHorizon.Domain.Entities;
using EventHorizon.Infrastructure.Data;
using EventHorizon.Infrastructure.Helpers;
using EventHorizon.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using NLipsum.Core;

namespace EventHorizon.Application.UseCases.Dev
{
    public class SeedDatabaseUseCase : ISeedDatabaseUseCase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ImageUploadOptions _imageUploadOptions;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordService _passwordService;
        private readonly static int EVENT_GENERATION_COUNT = 20;
        private readonly static int USER_GENERATION_COUNT = 90;

        public SeedDatabaseUseCase(
            IWebHostEnvironment environment,
            IOptions<ImageUploadOptions> imageUploadOptions,
            IUnitOfWork unitOfWork,
            IPasswordService passwordService) { 
            _environment = environment;
            _imageUploadOptions = imageUploadOptions.Value;
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
        }
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var categoryIds = new List<Guid>();

            for (int i = 0; i < 10; i++)
            {
                categoryIds.Add(Guid.NewGuid());
            }

            var categories = new List<EventCategory>
            {
                new() {
                    Id = categoryIds[0],
                    Name = "Networking Events",
                    Description = 
                        "Gatherings designed to help professionals connect, " +
                        "exchange ideas, and build relationships. Examples include business mixers, " +
                        "industry conferences, and career fairs."
                },

                new() {
                    Id = categoryIds[1],
                    Name = "Charity Galas",
                    Description =
                        "Formal events organized to raise funds for a cause. " +
                        "They often feature auctions, dinners, and entertainment, " +
                        "attracting donors and philanthropists."
                },

                new() {
                    Id = categoryIds[2],
                    Name = "Baby Showers and Gender Reveals",
                    Description =
                        "Celebrations held in anticipation of a new baby. " +
                        "These events often include games, gifts, and the revelation " +
                        "of the baby’s gender (if applicable)."
                },

                new() {
                    Id = categoryIds[3],
                    Name = "Cultural Festivals",
                    Description =
                        "Celebrations of art, music, food, and traditions from specific cultures or regions. " +
                        "Examples include film festivals, food fairs, and heritage days."
                },

                new() {
                    Id = categoryIds[4],
                    Name = "Weddings and Receptions",
                    Description =
                        " Ceremonies and parties celebrating the union of two individuals. " +
                        "These events often include rituals, dining, dancing, and toasts."
                },

                new() {
                    Id = categoryIds[5],
                    Name = "Corporate Retreats",
                    Description =
                        "Off-site gatherings for employees to bond, strategize, and relax. " +
                        "Activities may include team-building exercises, workshops, and leisure outings."
                },

                new() {
                    Id = categoryIds[6],
                    Name = "Holiday Parties",
                    Description =
                        "Seasonal celebrations for occasions like Christmas, New Year’s, or Thanksgiving. " +
                        "These events often involve gift exchanges, festive meals, and themed decorations."
                },

                new() {
                    Id = categoryIds[7],
                    Name = "Themed Parties",
                    Description =
                        " Social gatherings centered around a specific theme, such as a decade (e.g., 80s night), a movie, " +
                        "or a costume party. Guests often dress up and participate in themed activities."
                },

                new() {
                    Id = categoryIds[8],
                    Name = "Community Fundraisers",
                    Description =
                        "Local events aimed at raising money for community projects or individuals in need. " +
                        "Examples include bake sales, fun runs, and charity sports tournaments."
                },

                new() {
                    Id = categoryIds[9],
                    Name = "Reunions",
                    Description =
                        " Gatherings of people who share a common history, such as family reunions, " +
                        "school reunions, or military reunions. These events focus on reconnecting and reminiscing."
                },

            };

            foreach (var category in categories)
            {
                await _unitOfWork.Categories.AddAsync(category, cancellationToken);
            }

            var events = await GenerateEvents(EVENT_GENERATION_COUNT, categories);
            var users = await GenerateUsers(USER_GENERATION_COUNT);
            await GenerateEntries(users, events);
            _unitOfWork.Save();
        }

        private async Task<IEnumerable<User>> GenerateUsers(int userCount)
        {
            var users = new List<User>();

            for (int i = 0; i < userCount; i++)
            {
                var newUser = new User
                {
                    Id = Guid.NewGuid(),
                    Email = $"testuseremail{i}@horizon.com",
                    FirstName = $"firstname{i}",
                    LastName = $"lastname{i}",
                    RefreshToken = $"eylalala",
                    PasswordHash = _passwordService.HashPassword("qwerty"),
                    Role = Domain.Enums.UserRole.Admin,
                    DateOfBirth = DateOnly.FromDateTime(
                        new DateTime((int)Random.Shared.NextInt64(1971, 2020), 1, 1, 0, 0, 0)
                    )
                };

                try
                {
                    await _unitOfWork.Users.AddAsync(newUser, CancellationToken.None);
                    users.Add(newUser);
                    _unitOfWork.Save();
                } catch
                {
                    continue;
                }
            }

            return users.AsEnumerable();
        }

        private async Task<IList<string>> GetImageUrls()
        {
            var path = Path.Combine(
               _environment.WebRootPath,
               _imageUploadOptions.Url,
               "default"
            );

            var filePaths = Directory.GetFiles(path);

            return filePaths
                .Select(p => 
                    Path.Combine(
                        _imageUploadOptions.AccessibleUrl,
                        _imageUploadOptions.Url,
                        "default",
                        Path.GetFileName(p)
                    ))
                .ToList();
        }

        private async Task<IEnumerable<Event>> GenerateEvents(int eventCount, IEnumerable<EventCategory> categories)
        {
            var events = new List<Event>();
            var imageUrls = await GetImageUrls();
            var generator = new LipsumGenerator();

            for (int i = 0; i < eventCount; i++)
            {
                var newEvent = new Event
                {
                    Id = Guid.NewGuid(),
                    Name = string.Join("", generator.GenerateCharacters((int) Random.Shared.NextInt64(5, 40))),
                    Description = string.Join("", generator.GenerateCharacters((int)Random.Shared.NextInt64(10, 900))),
                    Address = string.Join("", generator.GenerateCharacters((int)Random.Shared.NextInt64(5, 100))),
                    DateTime = DateTime.UtcNow.AddDays(Random.Shared.NextInt64(1, 180)),
                    CategoryId = categories.ElementAt((int) Random.Shared.NextInt64(1, categories.Count())).Id,
                    MaxParticipantCount = (int) Random.Shared.NextInt64(1, 100),
                    ImageUrls = imageUrls
                        .Skip((int) Random.Shared.NextInt64(0, imageUrls.Count - 5))
                        .Take((int) Random.Shared.NextInt64(1, 5))
                        .ToList(),
                };

                try
                {
                    await _unitOfWork.Events.AddAsync(newEvent, CancellationToken.None);
                    events.Add(newEvent);
                    _unitOfWork.Save();
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return events.AsEnumerable();
        }

        private async Task GenerateEntries(IEnumerable<User> users, IEnumerable<Event> events)
        {
            
            for (int i = 0; i < users.Count(); i++)
            {
                var random = (int)Random.Shared.NextInt64(0, events.Count());
                for (int j = 0; j < events.Count() - random; j++)
                {
                    if (events.ElementAt(j).Entries.Count >= events.ElementAt(j).MaxParticipantCount)
                    {
                        continue;
                    }

                    var newEntry = new EventEntry
                    {
                        UserId = users.ElementAt(i).Id,
                        EventId = events.ElementAt(j).Id,
                        SubmissionDate = DateTime.UtcNow,
                    };

                    try
                    {
                        await _unitOfWork.Entries.AddAsync(newEntry, CancellationToken.None);
                        _unitOfWork.Save();
                    }
                    catch
                    {
                        continue;
                    }
                }
                    
            }
        }

    }
}
