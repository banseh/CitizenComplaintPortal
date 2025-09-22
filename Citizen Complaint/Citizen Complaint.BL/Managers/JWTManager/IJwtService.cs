namespace Citizen_Complaint.BL.Managers.JWTManager
{
    public interface IJwtService
    {
        string GenerateToken(string email , string userId);
    }
}
