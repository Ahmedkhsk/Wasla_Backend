namespace Wasla_Backend.Services.Implementation
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IUserRepository _userRepository;
        private readonly IReactionRepository _reactionRepository;
        private readonly IFileService _fileService;
        private readonly IFileUrlBuilderService _fileUrlBuilderService;
        private readonly ICommentRepository _commentRepository;
        private readonly ToxicityClassifier _toxicityClassifier;


        public PostService(
            IPostRepository postRepository,
            IMapper mapper,
            DateTimeHelper dateTimeHelper,
            IUserRepository userRepository,
            IReactionRepository reactionRepository,
            IFileService fileService,
            IFileUrlBuilderService fileUrlBuilderService,
            ICommentRepository commentRepository,
            ToxicityClassifier toxicityClassifier
        )
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _dateTimeHelper = dateTimeHelper;
            _userRepository = userRepository;
            _reactionRepository = reactionRepository;
            _fileService = fileService;
            _fileUrlBuilderService = fileUrlBuilderService;
            _commentRepository = commentRepository;
            _toxicityClassifier = toxicityClassifier;
        }

        public async Task AddPost(AddPostDto dto)
        {
            var user = await _userRepository.GetUserByIdAsync(dto.userId);
            if (user == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            if (dto.content != null)
            {
                var isToxic = _toxicityClassifier.IsBadWord(dto.content);
                if (isToxic)
                    throw new BadRequestException(LocalizationKey.PostContentIsInappropriate);
            }

            var post = _mapper.Map<Post>(dto);
            post.createdAt = _dateTimeHelper.Now;

            if (dto.filesDto != null && dto.filesDto.Any())
                post.files = await _fileService.AddFilesAsync(
                    dto.filesDto,
                    _fileUrlBuilderService.GetPath(MediaType.postFile)
                );

            await _postRepository.AddAsync(post);
            await _postRepository.SaveChangesAsync();
        }

        public async Task UpdatePost(UpdatePostDto dto)
        {
            var post = await _postRepository.GetByIdAsync(dto.id);
            if (post == null)
                throw new NotFoundException(LocalizationKey.PostNotFound);

            if(dto.content != null)
            {
                var isToxic = _toxicityClassifier.IsBadWord(dto.content);
                if (isToxic)
                    throw new BadRequestException(LocalizationKey.PostContentIsInappropriate);
            }

            var existingFileNames = _fileService.ExtractFileNames(dto.files.existingFiles);

            _mapper.Map(dto, post);

            post.files = await _fileService.ReplaceFilesAsync(
                post.files,
                existingFileNames,
                dto.files.newFiles,
                _fileUrlBuilderService.GetPath(MediaType.postFile)
            );

            _postRepository.Update(post);
            await _postRepository.SaveChangesAsync();
        }

        public async Task DeletePost(int postId)
        {
            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null)
                throw new NotFoundException(LocalizationKey.PostNotFound);

            _fileService.DeleteFiles(post.files, _fileUrlBuilderService.GetPath(MediaType.postFile));

            _postRepository.Delete(post);
            await _postRepository.SaveChangesAsync();
        }

        public async Task<PagedResult<PostGeneralResponse>> GetPostsGeneral(string userId, PaginationParams paginationParams)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            var pagedPosts = await _postRepository.GetPostsGeneral(paginationParams);
            var postIds = pagedPosts.Data.Select(p => p.id).ToList();

            var reactionsDictionary = await _reactionRepository.GetReactionCountsForPosts(postIds, ReactionTargetType.post, ReactionType.love);
            var userReactedPosts = await _reactionRepository.GetUserReactedPostIds(userId, postIds, ReactionTargetType.post, ReactionType.love);
            var commentsDictionary = await _commentRepository.GetCommentCountsForPosts(postIds);
            var savesDictionary = await _reactionRepository.GetReactionCountsForPosts(postIds, ReactionTargetType.post, ReactionType.save);
            var userSavedPosts = await _reactionRepository.GetUserReactedPostIds(userId, postIds, ReactionTargetType.post, ReactionType.save);

            var mappedPosts = pagedPosts.Data.Select(post => new PostGeneralResponse
            {
                postId = post.id,
                userName = post.user.FullName,
                content = post.content,
                files = post.files?
                    .Select(file => _fileUrlBuilderService.GetMediaUrl(file, MediaType.postFile))
                    .ToList(),
                numberofReacts = reactionsDictionary.TryGetValue(post.id, out var count) ? count : 0,
                numberofSaves = savesDictionary.TryGetValue(post.id, out var c) ? c : 0,
                numberofComments = commentsDictionary.TryGetValue(post.id, out var cc) ? cc : 0,
                isLoved = userReactedPosts.Contains(post.id),
                isSaved = userSavedPosts.Contains(post.id),
                createdAt = post.createdAt,
                updatedAt = post.updatedAt,
                profilePhoto = _fileUrlBuilderService.GetMediaUrl(post.user.ProfilePhoto, MediaType.userImage),
                userId = post.userId
            }).ToList();

            return new PagedResult<PostGeneralResponse>
            {
                PageNumber = pagedPosts.PageNumber,
                PageSize = pagedPosts.PageSize,
                TotalCount = pagedPosts.TotalCount,
                Data = mappedPosts
            };
        }

        public async Task<PostByUserIdResponse> GetPostsByUserId(string userId, string currentUserId, int pageNumber, int pageSize)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            var currentUser = await _userRepository.GetUserByIdAsync(currentUserId);
            if (currentUser == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            var pagedPosts = await _postRepository.GetPostsByUserId(userId, pageNumber, pageSize);
            var postIds = pagedPosts.Data.Select(p => p.id).ToList();

            var reactionsDictionary = await _reactionRepository.GetReactionCountsForPosts(postIds, ReactionTargetType.post, ReactionType.love);
            var userReactedPosts = await _reactionRepository.GetUserReactedPostIds(userId, postIds, ReactionTargetType.post, ReactionType.love);
            var commentsDictionary = await _commentRepository.GetCommentCountsForPosts(postIds);
            var savesDictionary = await _reactionRepository.GetReactionCountsForPosts(postIds, ReactionTargetType.post, ReactionType.save);
            var userSavedPosts = await _reactionRepository.GetUserReactedPostIds(userId, postIds, ReactionTargetType.post, ReactionType.save);

            var mappedPosts = pagedPosts.Data.Select(post => new PostRespnse
            {
                postId = post.id,
                content = post.content,
                files = post.files?
                    .Select(file => _fileUrlBuilderService.GetMediaUrl(file, MediaType.postFile))
                    .ToList(),
                numberofReacts = reactionsDictionary.TryGetValue(post.id, out var count) ? count : 0,
                numberofSaves = savesDictionary.TryGetValue(post.id, out var c) ? c : 0,
                numberofComments = commentsDictionary.TryGetValue(post.id, out var cc) ? cc : 0,
                isLoved = userReactedPosts.Contains(post.id),
                isSaved = userSavedPosts.Contains(post.id),
                createdAt = post.createdAt,
                updatedAt = post.updatedAt
            }).ToList();

            return new PostByUserIdResponse
            {
                userId = user.Id,
                userName = user.FullName,
                profilePhoto = _fileUrlBuilderService.GetMediaUrl(user.ProfilePhoto, MediaType.userImage),
                posts = new PagedResult<PostRespnse>
                {
                    PageNumber = pagedPosts.PageNumber,
                    PageSize = pagedPosts.PageSize,
                    TotalCount = pagedPosts.TotalCount,
                    Data = mappedPosts
                }
            };
        }

        public async Task<InformationProfileResponse> InformationProfileResponse(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            var postsCount = await _postRepository.GetPostsCountByUserId(userId);
            var reactionsCount = await _reactionRepository.GetReactionCountForUserPosts(userId, ReactionTargetType.post, ReactionType.love);
            var savesCount = await _reactionRepository.GetReactionCountForUserPosts(userId, ReactionTargetType.post, ReactionType.save);

            return new InformationProfileResponse
            {
                userName = user.FullName,
                profilePhoto = _fileUrlBuilderService.GetMediaUrl(user.ProfilePhoto, MediaType.userImage),
                postsCount = postsCount,
                reactionsCount = reactionsCount,
                savesCount = savesCount
            };
        }

        public async Task<PagedResult<PostGeneralResponse>> GetPostsByUsingReactionType(GetPostsByUsingReactionTypeDto dto)
        {
            return await _postRepository.GetPostsByUsingReactionType(dto);
        }
    }
}