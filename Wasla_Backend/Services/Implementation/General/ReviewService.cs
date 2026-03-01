using ServiceProvider = Wasla_Backend.Models.ServiceProvider;

namespace Wasla_Backend.Services.Implementation
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IResidentRepository _resididentRepository;
        private readonly IUserRepository _UserRepository;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<ServiceProvider> _serviceProviderRepositpry;
        private readonly IHubContext<ReviewHub> _hub;
        private readonly ToxicityClassifier _toxicityClassifier;

        public ReviewService(IMapper mapper, IReviewRepository reviewRepository, IResidentRepository residentRepository,
            IUserRepository userRepository, IGenericRepository<ServiceProvider> serviceProviderRepositpry,
            IHubContext<ReviewHub> hub, ToxicityClassifier toxicityClassifier)
        {
            _reviewRepository = reviewRepository;
            _resididentRepository = residentRepository;
            _UserRepository = userRepository;
            _mapper = mapper;
            _serviceProviderRepositpry = serviceProviderRepositpry;
            _hub = hub;
            _toxicityClassifier = toxicityClassifier;
        }

        public async Task AddReviewAsync(AddReviewDto review,string lan="en")
        {
            var user = await _resididentRepository.GetByIdAsync(review.userId);
            if (user == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            var serviceProvider = await _UserRepository.GetUserByIdAsync(review.serviceProviderId);
            if (serviceProvider == null)
                throw new NotFoundException(LocalizationKey.ServiceProviderNotFound);

            var numberOfReviews = await _reviewRepository.CountByServiceProviderAndUserId(review.serviceProviderId, review.userId);
            if (numberOfReviews >= 3)
                throw new BadRequestException(LocalizationKey.CannotAddMoreThan3Reviews);

            if (user.CountViolations >= 5)
                throw new BadRequestException(LocalizationKey.UserBlockedDueToViolations);

            var isToxic = _toxicityClassifier.IsBadWord(review.content);
            if (isToxic)
            {
                user.CountViolations += 1;
                await _UserRepository.UpdateUserAsync(user);
                await _resididentRepository.SaveChangesAsync();
                throw new BadRequestException(LocalizationKey.ReviewContainsToxicContent);
            }

            var Review = ReviewFactory.createReview();
            _mapper.Map(review, Review);
            Review.ReviewerName = user.FullName;
            Review.User = user;

            await _reviewRepository.AddAsync(Review);
            await _reviewRepository.SaveChangesAsync();

            if (serviceProvider is ServiceProvider provider)
            {
                provider.Rating = await _reviewRepository.GetRatingAvgByServiceProvider(provider.Id);
            }

            await _serviceProviderRepositpry.SaveChangesAsync();
            var metadata = new Dictionary<string, string>
            {
    { "UserName", user.FullName },
    { "Rating", review.rating.ToString("0.0") }
            };

            Hangfire.BackgroundJob.Enqueue<NotificationFunction>(
                x => x.ReviewNotification(
                serviceProvider.Id, NotificationType.reviewScreen,Review.Id.ToString(), user.ProfilePhoto, lan, metadata
                    ));

            var AddReview = new ReviewHubData
            {
                residentId = user.Id,
                serviceProviderId = serviceProvider.Id,
                reviewId = Review.Id
            };

            await _hub.Clients.All.SendAsync("ReviewAdded", AddReview);
        }

        public async Task DeleteReviewAsync(int reviewId)
        {
            var review = await _reviewRepository.GetByIdAsync(reviewId);
            if (review == null)
                throw new NotFoundException(LocalizationKey.ReviewNotFound);

            var serviceprovider = await _UserRepository.GetUserByIdAsync(review.ServiceProviderId);

            _reviewRepository.Delete(review);
            await _reviewRepository.SaveChangesAsync();

            if (serviceprovider is ServiceProvider provider)
            {
                provider.Rating = await _reviewRepository.GetRatingAvgByServiceProvider(provider.Id);
            }

            await _serviceProviderRepositpry.SaveChangesAsync();

            var deleteReview = new ReviewHubData
            {
                residentId = review.UserId,
                serviceProviderId = review.ServiceProviderId,
                reviewId = review.Id
            };

            await _hub.Clients.All.SendAsync("ReviewDeleted", deleteReview);
        }

        public async Task<IEnumerable<ReviewResponseDto>> GetReviewsByRating(int rating, string serviceProviderId)
        {
            var reviews = await _reviewRepository.GetReviewsByRating(rating, serviceProviderId);
            return reviews;
        }

        public async Task<IEnumerable<ReviewResponseDto>> GetReviewsByServiceProviderIdAsync(string serviceProviderId)
        {
            var reviews = await _reviewRepository.GetReviewsByServiceProviderIdAsync(serviceProviderId);
            return reviews;
        }

        public async Task UpdateReview(UpdateReviewDto updatereview)
        {
            var review = await _reviewRepository.GetByIdAsync(updatereview.reviewId);
            if (review == null)
                throw new NotFoundException(LocalizationKey.ReviewNotFound);

            var serviceprovider = await _UserRepository.GetUserByIdAsync(review.ServiceProviderId);
            var user = await _resididentRepository.GetByIdAsync(review.UserId);

            if (user == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            if (serviceprovider == null)
                throw new NotFoundException(LocalizationKey.ServiceProviderNotFound);

            if (user.CountViolations >= 5)
                throw new BadRequestException(LocalizationKey.UserBlockedDueToViolations);

            var isToxic = _toxicityClassifier.IsBadWord(updatereview.content);
            if (isToxic)
            {
                user.CountViolations += 1;
                await _UserRepository.UpdateUserAsync(user);
                await _resididentRepository.SaveChangesAsync();
                throw new BadRequestException(LocalizationKey.ReviewContainsToxicContent);
            }

            review.Content = updatereview.content;
            review.Rating = updatereview.rating;

            await _reviewRepository.SaveChangesAsync();

            if (serviceprovider is ServiceProvider provider)
            {
                provider.Rating = await _reviewRepository.GetRatingAvgByServiceProvider(provider.Id);
            }

            await _serviceProviderRepositpry.SaveChangesAsync();

            var UpdateReview = new ReviewHubData
            {
                residentId = review.UserId,
                serviceProviderId = review.ServiceProviderId,
                reviewId = review.Id
            };

            await _hub.Clients.All.SendAsync("ReviewUpdated", UpdateReview);
        }
    }
}