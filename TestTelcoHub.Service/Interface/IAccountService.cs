using TestTelcoHub.Constant;
using TestTelcoHub.Model.Model.Login;
using TestTelcoHub.Model.Model.Signup;
using TestTelcoHub.Service.Constant;

namespace TestTelcoHub.Service.Interface
{
    public interface IAccountService
    {
        public Task<ReponseBase> SignUpAsync(RegisterUser model);
        public Task<ResponseBaseToken> SignInAsync(LoginModel model);
        public Task<dynamic> GetInfo(string userId);
        Task<ResponseBaseToken> RefreshAccessToken(string refreshToken);
        //Task<Customer> GetCustomerByUserIdAsync(string userId);
    }
}
