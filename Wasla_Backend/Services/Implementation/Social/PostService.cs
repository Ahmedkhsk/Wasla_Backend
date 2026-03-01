namespace Wasla_Backend.Services.Implementation
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        private readonly DateTimeHelper _dateTimeHelper;
        private readonly IUserRepository _userRepository;
        private readonly IReactionRepository _reactionRepository;
        private readonly string _filePath;

        public PostService(IPostRepository postRepository,
                            IMapper mapper,
                            DateTimeHelper dateTimeHelper,
                            IWebHostEnvironment webHostEnvironment,
                            IUserRepository userRepository,
                            IReactionRepository reactionRepository
                          )
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _filePath = Path.Combine(webHostEnvironment.WebRootPath, FileSetting.FilesPosts.TrimStart('/'));
            _dateTimeHelper = dateTimeHelper;
            _userRepository = userRepository;
            _reactionRepository = reactionRepository;
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
                var files = post.files ?? new List<string>();

                foreach (var photo in dto.filesDto)
                {
                    var imagePath = await FileOperation.SaveFile(photo, _filePath);
                    files.Add(imagePath);
                }

                post.files = files;
            }

            await _postRepository.AddAsync(post);
            await _postRepository.SaveChangesAsync();
        }

        public async Task UpdatePost(UpdatePostDto dto)
        {
            var post = await _postRepository.GetByIdAsync(dto.id);
            if (post == null)
                throw new NotFoundException(LocalizationKey.PostNotFound);

            _mapper.Map(dto, post);

            if (dto.files != null && dto.files.Count > 0)
            {
                if (post.files != null && post.files.Count > 0)
                    foreach (var oldFile in post.files)
                        FileOperation.DeleteFile(oldFile, _filePath);

                post.files = new List<string>();

                foreach (var file in dto.files)
                {
                    var savedFileName = await FileOperation.SaveFile(file, _filePath);
                    post.files.Add(savedFileName);
                }
            }

            _postRepository.Update(post);
            await _postRepository.SaveChangesAsync();
        }

        public async Task DeletePost(int postId)
        {
            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null)
                throw new NotFoundException(LocalizationKey.PostNotFound);

            if (post.files != null && post.files.Count > 0)
                foreach (var oldFile in post.files)
                    FileOperation.DeleteFile(oldFile, _filePath);

            _postRepository.Delete(post);
            await _postRepository.SaveChangesAsync();
        }

        public async Task<PagedResult<PostGeneralResponse>> GetPostsGeneral(string userId ,int pageNumber, int pageSize)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            var pagedPosts = await _postRepository.GetPostsGeneral(pageNumber, pageSize);

            var postIds = pagedPosts.Data.Select(p => p.id).ToList();

            var reactionsDictionary = await _reactionRepository.GetReactionCountsForPosts(postIds,ReactionTargetType.post,ReactionType.love);

            var userReactedPosts = await _reactionRepository.GetUserReactedPostIds(userId,postIds,ReactionTargetType.post, ReactionType.love);
            
            var savesDictionary = await _reactionRepository.GetReactionCountsForPosts(postIds,ReactionTargetType.post, ReactionType.save);
            
            var userSavedPosts = await _reactionRepository.GetUserReactedPostIds(userId, postIds, ReactionTargetType.post, ReactionType.save);

            var mappedPosts = pagedPosts.Data.Select(post => new PostGeneralResponse
            {
                postId = post.id,
                userName = post.user.FullName,
                content = post.content,

                files = post.files?
                    .Select(file => FileSetting.GetMediaUrl(file, MediaType.postFile))
                    .ToList(),

                numberofReacts = reactionsDictionary.TryGetValue(post.id, out var count) ? count: 0,
                numberofSaves = savesDictionary.TryGetValue(post.id, out var c) ? c : 0,


                isLoved = userReactedPosts.Contains(post.id),
                isSaved  = userSavedPosts.Contains(post.id),
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

        public async Task<PostByUserIdResponse> GetPostsByUserId(string userId, string currentUserId,int pageNumber,int pageSize)
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
    }
}
