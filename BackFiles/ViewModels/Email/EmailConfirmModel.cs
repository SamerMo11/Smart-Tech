namespace E_CommerceWeb.ViewModels.Email
{
    public class EmailConfirmModel
    {
        public string email { get; set; }
        public bool IsVerify { get; set; }
        public bool IsResended { get; set; }
        public bool IsSuccess { get; set; }
    }
}
