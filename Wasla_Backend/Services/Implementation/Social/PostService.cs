namespace Wasla_Backend.Services.Implementation
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        private readonly DateTimeHelper _dateTimeHelper;
        private readonly IUserRepository _userRepository;
        private readonly string _filePath;

        public PostService(IPostRepository postRepository,
                            IMapper mapper,
                            DateTimeHelper dateTimeHelper,
                            IWebHostEnvironment webHostEnvironment,
                            IUserRepository userRepository
                          )
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _filePath = Path.Combine(webHostEnvironment.WebRootPath, FileSetting.FilesPosts.TrimStart('/'));
            _dateTimeHelper = dateTimeHelper;
            _userRepository = userRepository;
        }


        public async Task AddPost(AddPostDto dto)
        {
            var user = await _userRepository.GetUserByIdAsync(dto.userId);
            if (user == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            var post = _mapper.Map<Post>(dto);
            post.createdAt = _dateTimeHelper.Now;

            if (dto.files != null && dto.files.Count > 0)
            {
                post.files = new List<string>();
                foreach (var file in dto.files)
                {
                    var savedFileName = await FileOperation.SaveFile(file, _filePath);
                    post.files.Add(savedFileName);
                }
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

        //public async Task<List<PostGeneralResponse>> GetPostsGeneral()
        //{
        //    var posts = await _postRepository.GetPostsByUserIdAsync(userId);
        //    return _mapper.Map<List<PostGeneralResponse>>(posts);

        //}
    }
}
