using EventManagementSystem.Data;
using EventManagementSystem.Models;
using BCrypt.Net;



namespace EventManagementSystem.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        // Kullanıcı kaydetme metodu (şifre hash'lenir)

        public void RegisterUser(User user)
        {
            // Şifreyi bcrypt ile hashle
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        // Kullanıcıları listeleme metodu
        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        // Kullanıcıyı ID ile getirme metodu
        public User? GetUserById(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserID == userId);
            if (user == null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }
            return user;
        }

        // Kullanıcı güncelleme metodu
        public void UpdateUser(User user)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.UserID == user.UserID);
            if (existingUser != null)
            {
                existingUser.Name = user.Name;
                existingUser.Email = user.Email;

                // Şifre boş değilse güncelle (şifre hashlenir)
                if (!string.IsNullOrEmpty(user.Password))
                {
                    existingUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                }

                existingUser.Role = user.Role;

                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }
        }

        // Kullanıcı silme metodu
        public void DeleteUser(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserID == userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }
        }

        // Kullanıcı giriş yapma metodu
        public User? LoginUser(string email, string password)
        {
            // Veritabanında e-posta adresine göre kullanıcıyı bulma
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                // Şifreyi bcrypt kullanarak doğrulama
                if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    // Şifre doğru ise kullanıcı bilgilerini döndür
                    return user;
                }
                else
                {
                    throw new Exception("Geçersiz kullanıcı adı veya şifre.");
                }
            }
            else
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }
        }

    }
}
