namespace Wasla_Backend.Services.Implementation
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        private readonly DateTimeHelper _dateTimeHelper;
        private readonly IUserRepository _userRepository;
        private readonly IReactionRepository _reactionRepository;
        private readonly IFileService _fileService;
        private readonly ICommentRepository _commentRepository;

        public PostService(IPostRepository postRepository,
                            IMapper mapper,
                            DateTimeHelper dateTimeHelper,
                            IUserRepository userRepository,
                            IReactionRepository reactionRepository,
                            IFileService fileService,
                            ICommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _dateTimeHelper = dateTimeHelper;
            _userRepository = userRepository;
            _reactionRepository = reactionRepository;
            _fileService = fileService;
            _commentRepository = commentRepository;
        }

        public async Task AddPost(AddPostDto dto)
        {
            var user = await _userRepository.GetUserByIdAsync(dto.userId);
            if (user == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            var post = _mapper.Map<Post>(dto);
            post.createdAt = _dateTimeHelper.Now;

            if (dto.filesDto != null && dto.filesDto.Any())
            {
                post.files = await _fileService.AddFilesAsync(
                    dto.filesDto,
                    FileSetting.FilesPosts);
            }

            await _postRepository.AddAsync(post);
            await _postRepository.SaveChangesAsync();
        }

        public async Task UpdatePost(UpdatePostDto dto)
        {
            var post = await _postRepository.GetByIdAsync(dto.id);
            if (post == null)
                throw new NotFoundException(LocalizationKey.PostNotFound);

            var existingFileNames = _fileService.ExtractFileNames(dto.existingFiles);

            _mapper.Map(dto, post);

            post.files = await _fileService.ReplaceFilesAsync(
                post.files,
                existingFileNames,
                dto.newFiles,
                FileSetting.FilesPosts
            );

            _postRepository.Update(post);
            await _postRepository.SaveChangesAsync();
        }

        public async Task DeletePost(int postId)
        {
            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null)
                throw new NotFoundException(LocalizationKey.PostNotFound);

            _fileService.DeleteFiles(post.files, FileSetting.FilesPosts);

            _postRepository.Delete(post);
            await _postRepository.SaveChangesAsync();
        }

        public async Task<PagedResult<PostGeneralResponse>> GetPostsGeneral(string userId, int pageNumber, int pageSize)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            var pagedPosts = await _postRepository.GetPostsGeneral(pageNumber, pageSize);

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
                    .Select(file => FileSetting.GetMediaUrl(file, MediaType.postFile))
                    .ToList(),

                numberofReacts = reactionsDictionary.TryGetValue(post.id, out var count) ? count : 0,
                numberofSaves = savesDictionary.TryGetValue(post.id, out var c) ? c : 0,
                numberofComments = commentsDictionary.TryGetValue(post.id, out var cc) ? cc : 0,

                isLoved = userReactedPosts.Contains(post.id),
                isSaved = userSavedPosts.Contains(post.id),
                createdAt = post.createdAt,
                updatedAt = post.updatedAt,
                profilePhoto = FileSetting.GetMediaUrl(post.user.ProfilePhoto, MediaType.userImage),
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

            var pagedPosts =
                await _postRepository.GetPostsByUserId(userId, pageNumber, pageSize);

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
                    .Select(file => FileSetting.GetMediaUrl(file, MediaType.postFile))
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
                profilePhoto =
                    FileSetting.GetMediaUrl(user.ProfilePhoto, MediaType.userImage),

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
                profilePhoto = FileSetting.GetMediaUrl(user.ProfilePhoto, MediaType.userImage),
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