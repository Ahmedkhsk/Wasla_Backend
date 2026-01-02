

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
        public ReviewService(IMapper mapper,IReviewRepository reviewRepository, IResidentRepository residentRepository, IUserRepository userRepository,
            IGenericRepository<ServiceProvider> serviceProviderRepositpry, IHubContext<ReviewHub> hub)
        {
            _reviewRepository = reviewRepository;
            this._resididentRepository = residentRepository;
            _UserRepository = userRepository;
            this._mapper = mapper;
            _serviceProviderRepositpry = serviceProviderRepositpry;
            _hub = hub;
        }

        public async Task AddReviewAsync(AddReviewDto review)
        {
            var user = await _resididentRepository.GetByIdAsync(review.userId);
            if (user == null)
                throw new NotFoundException("UserNotFound");
            var serviceProvider=await _UserRepository.GetUserByIdAsync(review.serviceProviderId);
            if (serviceProvider == null)
                throw new NotFoundException("ServiceProviderNotFound");
            var numberOfReviews = await _reviewRepository.CountByServiceProviderAndUserId(review.serviceProviderId, review.userId);
            if (numberOfReviews >=3)
                throw new BadRequestException("CannotAddMoreThan3Reviews");
            var Review = ReviewFactory.createReview();
           _mapper.Map(review,Review);
            Review.ReviewerName = user.FullName;
            Review.User = user;
            await _reviewRepository.AddAsync(Review);
            await _reviewRepository.SaveChangesAsync();
            if (serviceProvider is ServiceProvider provider)
            {
                provider.Rating = await _reviewRepository.GetRatingAvgByServiceProvider(provider.Id);
            }
            await _serviceProviderRepositpry.SaveChangesAsync();
            var AddReview = new ReviewHubData { residentId = user.Id, serviceProviderId = serviceProvider.Id, reviewId = Review.Id };
           await _hub.Clients.All.SendAsync("ReviewAdded", AddReview);

        }

        public async Task DeleteReviewAsync(int reviewId)
        {
            var review = await _reviewRepository.GetByIdAsync(reviewId);
            if (review == null)
                throw new NotFoundException("ReviewNotFound");
            _reviewRepository.Delete(review);
            await _reviewRepository.SaveChangesAsync();
            var deleteReview = new ReviewHubData { residentId = review.UserId, serviceProviderId = review.ServiceProviderId,
                reviewId = review.Id };
            await _hub.Clients.All.SendAsync("ReviewDeleted", deleteReview);

        }

        public async Task<IEnumerable<ReviewResponseDto>> GetReviewsByRating(int rating)
        {
            var reviews =await _reviewRepository.GetReviewsByRating(rating);
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
                throw new NotFoundException("ReviewNotFound");
            var serviceprovider=await _UserRepository.GetUserByIdAsync(review.ServiceProviderId);
            if(serviceprovider==null)
            throw new NotFoundException("ServiceProviderNotFound");

            review.Content = updatereview.content;
            review.Rating=updatereview.rating;
            
           await _reviewRepository.SaveChangesAsync();

            if (serviceprovider is ServiceProvider provider)
            {
                provider.Rating =await _reviewRepository.GetRatingAvgByServiceProvider(provider.Id);
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
